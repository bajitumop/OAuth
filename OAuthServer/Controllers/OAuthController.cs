namespace OAuthServer.Controllers
{
	using System;
	using System.Net;
	using System.Web;
	using System.Web.Mvc;
	using Microsoft.AspNet.Identity;
	using Models;
	using Newtonsoft.Json;
	using Services;
	using Shared;
	using Shared.DataAccess;
	using Shared.Models;

	public class OAuthController : Controller
	{
		private readonly Config _config;
		private readonly CryptographicService _cryptoService;
		private readonly IAuthCodeGrantRepository _accessTokenRepository;

		public OAuthController(CryptographicService cryptographicService, IConfigRepository configRepository, IAuthCodeGrantRepository accessTokenRepository)
		{
			_accessTokenRepository = accessTokenRepository;
			_config = configRepository.GetConfig();
			_cryptoService = cryptographicService;
		}

		[HttpGet]
		public ActionResult Authorize(string response_type, string client_id, string redirect_uri, string scope,
			string state = "")
		{
			if (response_type != "code")
			{
				return ErrorRedirect(redirect_uri, "unsupported_response_type", state, "response_type must be \"code\"");
			}

			if (string.IsNullOrEmpty(client_id))
			{
				return ErrorRedirect(redirect_uri, "unauthorized_client", state);
			}

			Scope scopes;
			if (!Scopes.TryParse(scope, out scopes))
			{
				return ErrorRedirect(redirect_uri, "invalid_scope", state);
			}
            
		    ViewBag.ClientId = client_id;
		    ViewBag.RedirectUri = redirect_uri;
		    ViewBag.ScopeStr = ((int) scopes).ToString();
		    ViewBag.State = state;

			return View();
		}

		[HttpPost]
		public ActionResult Authorize(AuthorizationCodeGrantModel model)
		{
			if (!string.IsNullOrEmpty(Request.Form["Deny"]))
			{
				return ErrorRedirect(model.RedirectUri, "access_denied", model.State);
			}
			
			var authCodeGrantModel = new AuthCodeGrantModel
			{
				RedirectUri = model.RedirectUri,
				ClientId = model.ClientId,
				Scope = (Scope)Enum.Parse(typeof(Scope), model.ScopeStr),
				AccessToken = HttpServerUtility.UrlTokenEncode(_cryptoService.GenerateBytes(_config.AccessTokenSize)),
				ExpirationDateTimeUtc = DateTime.UtcNow.Add(_config.AccessTokenExpiration),
				UserId = Guid.Parse(User.Identity.GetUserId()),
				Lock = false,
				RefreshToken = HttpServerUtility.UrlTokenEncode(_cryptoService.GenerateBytes(_config.RefreshTokenSize))
			};

			_accessTokenRepository.Create(authCodeGrantModel);
			_accessTokenRepository.SaveChanges();

			var codeBytes = _cryptoService.Encrypt(authCodeGrantModel.Id.ToString());
			var code = HttpServerUtility.UrlTokenEncode(codeBytes);
			return SuccessRedirect(model.RedirectUri, code, model.State);
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Token(string grant_type, string code, string redirect_uri, string client_id)
		{
			if (grant_type != "authorization_code")
			{
				return BadRequest(new Error("unsupported_grant_type", "grant_type must be \"authorization_code\""));
			}

			if (string.IsNullOrEmpty(client_id))
			{
				return BadRequest(new Error("unauthorized_client"));
			}

			var codeBytes = HttpServerUtility.UrlTokenDecode(code);
			var id = Guid.Parse(_cryptoService.Decrypt(codeBytes));
			var model = _accessTokenRepository.Read(id);

			if (model == null)
			{
				return BadRequest(new Error("invalid_request", "code is invalid"));
			}

			if (model.Lock)
			{
				return BadRequest(new Error("invalid_request", "code expired"));
			}

			if (model.RedirectUri != redirect_uri)
			{
				return BadRequest(new Error("invalid_request", "wrong redirect_uri"));
			}

			if (model.ClientId != client_id)
			{
				return BadRequest(new Error("unauthorized_client"));
			}

			model.Lock = true;
			_accessTokenRepository.SaveChanges();

			return Content(JsonConvert.SerializeObject(new {model.AccessToken, model.RefreshToken}));
		}

		[HttpGet]
		[AllowAnonymous]
		public ActionResult Refresh(string grant_type, string refresh_token, string scope)
		{
			if (grant_type != "refresh_token")
			{
				return BadRequest(new Error("unsupported_grant_type", "grant_type must be \"refresh_token\""));
			}

			if (string.IsNullOrEmpty(refresh_token))
			{
				return BadRequest(new Error("invalid_request", "refresh_token is empty"));
			}

			var model = _accessTokenRepository.GetByRefreshToken(refresh_token);
			if (model == null)
			{
				return BadRequest(new Error("invalid_request", "refresh_token is invalid"));
			}

			model.AccessToken = HttpServerUtility.UrlTokenEncode(_cryptoService.GenerateBytes(_config.AccessTokenSize));
			model.RefreshToken = HttpServerUtility.UrlTokenEncode(_cryptoService.GenerateBytes(_config.RefreshTokenSize));
		    model.ExpirationDateTimeUtc = DateTime.UtcNow.Add(_config.AccessTokenExpiration);
            _accessTokenRepository.SaveChanges();

			return Content(JsonConvert.SerializeObject(new {model.AccessToken, model.RefreshToken}));
		}
		
		private HttpStatusCodeResult BadRequest(Error error)
		{
			return new HttpStatusCodeResult(HttpStatusCode.BadRequest, JsonConvert.SerializeObject(error));
		}

		private RedirectResult SuccessRedirect(string redirectUri, string code, string state)
		{
			return Redirect($"{redirectUri}?code={code}&state={state}");
		}

		private RedirectResult ErrorRedirect(string redirectUri, string error, string state, string description = null, string errorUri = null)
		{
			var uri = $"{redirectUri}?error={error}&state={state}";
			if (!string.IsNullOrEmpty(description))
			{
				uri += $"&error_description={description}";
			}

			if (!string.IsNullOrEmpty(errorUri))
			{
				uri += $"&error_uri={errorUri}";
			}

			return Redirect(uri);
		}

		private class Error
		{
			[JsonProperty("error")]
			public string Message { get; set; }

			[JsonProperty("error_description")]
			public string Description { get; set; }

			[JsonProperty("error_uri")]
			public string Uri { get; set; }

			public Error(string message, string description = null, string uri = null)
			{
				Message = message;
				Description = description;
				Uri = uri;
			}
		}
	}
}