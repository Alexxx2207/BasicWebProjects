using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCFramework;
using SUSHTTP;

namespace ConsoleApp.Controllers
{
    public class StaticFilesController : Controller
    {
        public HttpResponse Favicon(HttpRequest request)
        {
            return this.File("wwwroot/favicon.ico", "image/vnd.microsoft.icon");
        }

        public HttpResponse BootstrapCSS(HttpRequest request)
        {
            return this.File("wwwroot/css/bootstrap.min.css", "text/css");

        }

        public HttpResponse CustomCSS(HttpRequest request)
        {
            return this.File("wwwroot/css/custom.css", "text/css");

        }

        public HttpResponse BootstrapJS(HttpRequest request)
        {
            return this.File("wwwroot/js/bootstrap.bundle.min.js", "text/javascript");

        }

        public HttpResponse CustomJS(HttpRequest request)
        {
            return this.File("wwwroot/js/custom.js", "text/javascript");

        }
    }
}
