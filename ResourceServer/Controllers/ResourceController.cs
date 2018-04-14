namespace ResourceServer.Controllers
{
    using System.IO;
    using System.Web.Http;
    using Filters;
    using Shared;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Hosting;

    [RoutePrefix("resource")]
    public class ResourceController : ApiController
    {
        [HttpGet]
        [Route("bio")]
        [Scope(Scope.Bio)]
        public string Bio()
        {
            return "Специально для Константина Алексашина";
        }

        [HttpGet]
        [Route("notes")]
        [Scope(Scope.Notes)]
        public string Notes()
        {
            return "notes";
        }

        [HttpGet]
        [Route("images")]
        [Scope(Scope.Images)]
        public HttpResponseMessage Images()
        {
            var response = new HttpResponseMessage
            {
                Content = new StreamContent(new FileStream(HostingEnvironment.MapPath(@"~/App_Data/images/image.png"), FileMode.Open))
            };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            return response;
        }
    }
}
