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
        private readonly Dictionary<Method, Dictionary<string, Func<Request, Response>>> routes;
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

        public IRoutingTable Map(string url, Method method, Func<Request, Response> controller)
        {
            Guard.AgainstNull(url, nameof(url));
            Guard.AgainstNull(controller, nameof(controller));

            this.routes[method][url] = controller;

            return this;
        }

        public IRoutingTable MapGet(string url, Func<Request, Response> controller)
        {
            return Map(url, Method.Get, controller);
        }

        public IRoutingTable MapPost(string url, Func<Request, Response> controller)
        {
            return Map(url, Method.Post, controller);
        }

        public Response MatchRequest(Request request)
        {
            var requestMethod = request.Method;
            var requestPath = request.Url;

            if (!this.routes.ContainsKey(requestMethod) || !this.routes[requestMethod].ContainsKey(requestPath))
            { 
                return new NotFoundResponse();
            }

            return this.routes[requestMethod][requestPath](request);
        }
    }
}
