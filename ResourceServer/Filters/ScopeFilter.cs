namespace ResourceServer.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using Shared;

    public class ScopeAttribute : AuthorizeAttribute
    {
        private readonly IEnumerable<Scope> _allowedScopes;

        public ScopeAttribute(Scope allowedScopes)
        {
            _allowedScopes = allowedScopes.AsEnumerable();
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent("Invalid scopes")
            };
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var accessTokenScopes = ((Scope) actionContext.Request.Properties["scope"]).AsEnumerable().ToArray();
            return _allowedScopes.All(x => accessTokenScopes.Contains(x));
        }
    }
}