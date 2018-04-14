namespace ResourceServer.Filters
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    using Shared.DataAccess;
    using Shared.DataAccess.Repositories;

    public class BearerAuthorizeFilter : AuthorizeAttribute
    {
        private readonly IAuthCodeGrantRepository _authCodeGrantRepository;

        public BearerAuthorizeFilter(/*IAuthCodeGrantRepository authCodeGrantRepository*/)
        {
            _authCodeGrantRepository = new AuthCodeGrantRepository(new DataContext());
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var authorization = actionContext.Request?.Headers?.Authorization;

            if (authorization?.Scheme != "Bearer")
            {
                Forbidden(actionContext, "Authorization scheme must be \"Bearer\"");
                return false;
            }

            var model = _authCodeGrantRepository.GetByAccessToken(authorization.Parameter);

            if (model == null)
            {
                Forbidden(actionContext, "Access token is invalid or expired");
                return false;
            }

            actionContext.Request.Properties.Add(new KeyValuePair<string, object>("scope", model.Scope));
            
            return true;
        }

        private void Forbidden(HttpActionContext context, string message)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(message)
            };
        }
    }
}