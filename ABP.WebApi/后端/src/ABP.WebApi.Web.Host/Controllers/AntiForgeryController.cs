using Microsoft.AspNetCore.Antiforgery;
using ABP.WebApi.Controllers;

namespace ABP.WebApi.Web.Host.Controllers
{
    public class AntiForgeryController : WebApiControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
