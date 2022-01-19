using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.HTTP;

namespace WebServer.Server.Responses
{
    public class HtmlResponse : ContentResponse
    {
        public HtmlResponse(string content, Action<Request, Response> addCookiesAction = null) 
            :base(content, ContentType.Html, addCookiesAction)
        {
        }
    }
}
