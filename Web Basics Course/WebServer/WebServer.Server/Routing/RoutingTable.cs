using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.Common;
using WebServer.Server.HTTP;
using WebServer.Server.Responses;

namespace WebServer.Server.Routing
{
    public class RoutingTable : IRoutingTable
    {
        private readonly Dictionary<Method, Dictionary<string, Response>> routes;
        public RoutingTable()
        {
            routes = new()
            {
                [Method.Get] = new(),
                [Method.Post] = new(),
                [Method.Put] = new(),
                [Method.Delete] = new()
            };
        }

        public IRoutingTable Map(string url, Method method, Response response)
        {
            return method switch
            {
                Method.Get => this.MapGet(url, response),
                Method.Post => this.MapPost(url, response),
                _ => throw new InvalidOperationException
                        ($"Method '{method}' is not implemented.")
            };
        }

        public IRoutingTable MapGet(string url, Response response)
        {
            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            routes[Method.Get][url] = response;

            return this;
        }

        public IRoutingTable MapPost(string url, Response response)
        {
            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(response, nameof(response));

            routes[Method.Post][url] = response;

            return this;
        }
        public Response MatchRequest(Request request)
        {
            var method = request.Method;
            var url = request.Url;

            if (!routes.ContainsKey(method) || !routes[method].ContainsKey(url))
            {
                return new NotFoundResponse();
            }

            return routes[method][url];
        }
    }
}
