namespace OAuthServer
{
	using System.Data.Entity;
	using System.Web;
	using System.Web.Mvc;
	using System.Web.Optimization;
	using System.Web.Routing;
	using Shared.DataAccess;

	public class MvcApplication : HttpApplication
	{
        protected void Application_Start()
        {
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

			UnityMvcActivator.Start();
			Database.SetInitializer(new DatabaseInitializer());
		}

		protected void Application_End()
		{
			UnityMvcActivator.Shutdown();
		}
	}
}
