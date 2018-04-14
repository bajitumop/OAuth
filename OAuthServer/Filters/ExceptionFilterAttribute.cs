namespace OAuthServer.Filters
{
	using System.Net;
	using System.Web.Mvc;

	public class ExceptionFilterAttribute : IExceptionFilter
	{
		public void OnException(ExceptionContext filterContext)
		{
			var exception = filterContext.Exception;
			filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Something went wrong");
		}
	}
}