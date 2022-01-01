using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MVCFramework.ViewEngine;
using SUSHTTP;

namespace MVCFramework
{
    public class Controller
    {
        private IViewEngine viewEngine;
        private const string UserIdSessionName = "UserId";

        public Controller()
        {
            viewEngine = new SUSViewEngine();
        }

        protected HttpResponse View(object viewModel = null, [CallerMemberName] string viewPath = null)
        {
            var layout = System.IO.File.ReadAllText("Views/Shared/_Layout.cshtml");

            layout = layout.Replace("@RenderBody()", "__VIEW_CONTENT_HERE__");


            layout = viewEngine.GetHtml(layout, viewModel);

            var viewContent = System.IO.File.ReadAllText("Views/" +
                this.GetType().Name.Replace("Controller", string.Empty) + "/" +
                viewPath + ".cshtml");

            viewContent = viewEngine.GetHtml(viewContent, viewModel);


            var htmlResponseBody = layout.Replace("__VIEW_CONTENT_HERE__", viewContent);

            var bodyByteArray = Encoding.UTF8.GetBytes(htmlResponseBody);
            var httpResponse = new HttpResponse("text/html", bodyByteArray);

            return httpResponse;
        }

        protected HttpResponse File(string path, string contentType)
        {
            var fileBytes = System.IO.File.ReadAllBytes(path);
            var response = new HttpResponse(contentType, fileBytes);
            return response;
        }

        protected HttpResponse Redirect(string url)
        {
            var response = new HttpResponse(SUSHTTP.Enums.HttpStatusCode.Found);

            response.Headers.Add(new Header("Location", url));

            return response;
        }

    }
}
