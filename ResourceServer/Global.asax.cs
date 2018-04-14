namespace ResourceServer
{
    using System.Web.Http;
    using Filters;

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configuration.Filters.Add(new BearerAuthorizeFilter());
        }
    }
}
