using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SUSHTTP
{
    public class HttpServer : IHttpServer
    {
        IDictionary<string, Func<HttpRequest, HttpResponse>> routes;

        public HttpServer()
        {
            routes = new Dictionary<string, Func<HttpRequest, HttpResponse>>();
        }

        public void AddRoute(string path, Func<HttpRequest, HttpResponse> action)
        {
            if (routes.ContainsKey(path))
            {
                routes[path] = action;
            }
            else
            {
                routes.Add(path, action);
            }
        }

        public async Task StartAsync(int port)
        {
            TcpListener tcpListener = new TcpListener(IPAddress.Loopback, port);

            tcpListener.Start();

            while (true)
            {
                var client = await tcpListener.AcceptTcpClientAsync();

                ProcessTcpClient(client);
            }
        }

        private async Task ProcessTcpClient(TcpClient client)
        {
            using (NetworkStream stream = client.GetStream())
            {
                byte[] buffer = new byte[GlobalConstants.GlobalConstants.BufferSize];

                List<byte> data = new List<byte>();

                int position = 0;

                while (true)
                {
                    int count = await stream.ReadAsync(buffer, position, buffer.Length);
                    position += count;

                    data.AddRange(buffer);

                    if (count < buffer.Length)
                    {
                        var partialBuffer = new byte[count];
                        Array.Copy(buffer, partialBuffer, count);
                        data.AddRange(partialBuffer);
                        break;
                    }
                    else
                    {
                        data.AddRange(buffer);
                    }
                }

                var requestAsString = Encoding.UTF8.GetString(data.ToArray());

                var httpRequest = new HttpRequest(requestAsString);

                Console.WriteLine(
                    $"{httpRequest.Method} {httpRequest.Path} => {httpRequest.Headers.Count} headers");

                HttpResponse response;

                if (routes.ContainsKey(httpRequest.Path))
                {
                    var action = routes[httpRequest.Path];

                    response = action(httpRequest);
                }
                else
                {
                    var body = "<h1>Missing Page</h1>";
                    var bodyByteArray = Encoding.UTF8.GetBytes(body);
                    response = new HttpResponse("text/html", bodyByteArray, SUSHTTP.Enums.HttpStatusCode.NotFound);

                }

                response.Cookies.Add(new ResponseCookie("sid", Guid.NewGuid().ToString())
                { 
                    HttpOnly = true,
                    MaxAge = 60 * 24 * 60 * 60
                });
                response.Headers.Add(new Header($"Server", GlobalConstants.GlobalConstants.ServerName));

                var responseHeaderByteArray = Encoding.UTF8.GetBytes(response.ToString());

                await stream.WriteAsync(responseHeaderByteArray, 0, responseHeaderByteArray.Length);
                await stream.WriteAsync(response.Body, 0, response.Body.Length);
            }

            client.Close();
        }
    }
}
