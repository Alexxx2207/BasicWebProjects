using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.HTTP;

namespace WebServer.Server.Responses
{
    public class NotFoundResponse : Response
    {
        public NotFoundResponse() : base(StatusCode.NotFound)
        {
        }
    }
}
