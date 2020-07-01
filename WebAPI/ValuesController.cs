
using System.Collections.Specialized;
using System.Configuration;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;


namespace SampleWebApiB
{
   // [EnableCors(origins: "*", headers: "Accept, Authorization", methods: "GET, OPTIONS")]
   [RoutePrefix("api/Values")]
    public class ValuesController : ApiController
    {

        NameValueCollection appSettings = ConfigurationManager.AppSettings;

        [HttpGet]
        [Route("unprotected")]
        public IHttpActionResult NotSecured()
        {
            return this.Ok("Web Api unprotected endpoint");
        }

        
        [Authorize]
        //[OktaAuthorize]
        //[OktaGroupAuthorize]
        [HttpGet]
        [Route("protected")]
        public IHttpActionResult Secured()
        {
            string login = string.Empty;
            if (Thread.CurrentPrincipal != null)
            {
                ClaimsPrincipal principal = Thread.CurrentPrincipal as ClaimsPrincipal;// HttpContext.Current.User as ClaimsPrincipal;
      
            }
          
            return this.Ok("Web Api protected endpoint");
        }
    }
}
