namespace OAuthServer
{
	using System.Web.Mvc;
	using Filters;

	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
			filters.Add(new AuthorizeAttribute());
			filters.Add(new ExceptionFilterAttribute());
		}
	}
}
