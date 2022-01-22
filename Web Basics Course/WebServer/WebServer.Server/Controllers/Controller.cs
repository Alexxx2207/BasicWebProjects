using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.HTTP;
using WebServer.Server.Responses;

namespace WebServer.Server.Controllers
{
    public class Controller
    {
        public Request Request { get; set; }
        protected Controller(Request request)
        {
            Request = request;
        }

        protected Response Text(string text) => new TextResponse(text);

        protected Response View([CallerMemberName] string viewName = "")
        {
            return new ViewResponse(viewName, this.GetControllerName());
        }

        protected Response View(object model, [CallerMemberName] string viewName = "")
        {
            return new ViewResponse(viewName, this.GetControllerName(), model);
        }

        protected Response Html(string html, CookieCollection cookies = null)
        {
            var response = new HtmlResponse(html);

            if (cookies != null)
            {
                foreach (var cookie in cookies)
                {
                    response.Cookies.Add(cookie.Name, cookie.Value);
                }
            }

            return response;
        }
        protected Response BadRequest() => new BadRequestResponse();
        protected Response Unauthorized() => new UnauthorizedResponse();
        protected Response NotFound() => new NotFoundResponse();
        protected Response Redirect(string location) => new RedirectResponse(location);
        protected Response File(string fileName) => new TextFileResponse(fileName);

        private string GetControllerName()
        {
            return this.GetType().Name.Replace(nameof(Controller), string.Empty);
        }
    }
}
