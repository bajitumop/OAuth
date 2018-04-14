namespace OAuthServer.Controllers
{
	using System.Threading.Tasks;
	using System.Web;
	using System.Web.Mvc;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.Owin;
	using Microsoft.Owin.Security;
	using Models;
	using Models.AccountModels;
	using Shared.Models;
	
	public class AccountController : Controller
    {
	    private IAuthenticationManager _authenticationManager => HttpContext.GetOwinContext().Authentication;
        private ApplicationSignInManager _signInManager => HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        private ApplicationUserManager _userManager => HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
		
		[HttpGet]
		[AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
			return View();
        }
		
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
			
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
           
	        if (result == SignInStatus.Success)
	        {
					if (Url.IsLocalUrl(returnUrl))
	                {
		                return Redirect(returnUrl);
	                }

	                return RedirectToAction("Index", "Home");
	        }

	        ModelState.AddModelError("", "Неудачная попытка входа.");
	        return View(model);
		}
		
		[HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
		
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent:false, rememberBrowser:false);
                    return RedirectToAction("Index", "Home");
                }

	            foreach (var error in result.Errors)
	            {
		            ModelState.AddModelError("", error);
	            }
            }
			
            return View(model);
        }
		
        [HttpGet]
        public ActionResult LogOff()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
	}
}