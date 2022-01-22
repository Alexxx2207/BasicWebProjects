using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.HTTP;

namespace WebServer.Server.Routing
{
    public interface IRoutingTable
    {
        IRoutingTable Map(string url, Method method, Func<Request, Response> controller);
        IRoutingTable MapGet(string url, Func<Request, Response> controller);
        IRoutingTable MapPost(string url, Func<Request, Response> controller);
    }
}
