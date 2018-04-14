[assembly: Microsoft.Owin.OwinStartup(typeof(OAuthServer.Startup))]
namespace OAuthServer
{
	using System;
	using System.Web.Mvc;
	using Microsoft.AspNet.Identity;
	using Microsoft.AspNet.Identity.Owin;
	using Microsoft.Owin;
	using Microsoft.Owin.Security.Cookies;
	using Models.AccountModels;
	using Owin;

	public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
			app.CreatePerOwinContext(() =>
			{
				var store = (UserStore)DependencyResolver.Current.GetService(typeof(UserStore));
				return new ApplicationUserManager(store);
			});

	        app.CreatePerOwinContext<ApplicationSignInManager>(
		        (options, context) =>
			        new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication)
	        );

	        app.UseCookieAuthentication(new CookieAuthenticationOptions
	        {
		        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
		        CookieName = "Authentication",
		        LoginPath = new PathString("/Account/Login"),
				SlidingExpiration = true,
		        ExpireTimeSpan = TimeSpan.FromMinutes(5)
	        });
		}
    }
}
