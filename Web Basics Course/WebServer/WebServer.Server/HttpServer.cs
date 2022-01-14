using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.HTTP;
using WebServer.Server.Routing;

namespace WebServer.Server
{
    public class HttpServer
    {
        private readonly IPAddress ipAddress;
        private readonly int port;
        private readonly TcpListener serverListener;

        private readonly RoutingTable routingTable;

        public HttpServer(string ipAddress, int port, Action<IRoutingTable> routingTableConfiguration)
        {
            this.ipAddress = IPAddress.Parse(ipAddress);
            this.port = port;

            this.serverListener = new TcpListener(this.ipAddress, port);

            routingTableConfiguration(routingTable = new RoutingTable());
        }

        public HttpServer(int port, Action<IRoutingTable> routingTableConfiguration)
            : this("127.0.0.1", port, routingTableConfiguration)
        {

        }

        public HttpServer(Action<IRoutingTable> routingTableConfiguration)
            : this(8080, routingTableConfiguration)
        {

        }

        public void Start()
        { 
            serverListener.Start();

            while (true)
            { 
                var connection = serverListener.AcceptTcpClient();

                var networkStream = connection.GetStream();

                var strRequest = ReadRequest(networkStream);

                Console.WriteLine(strRequest);

                var request = Request.Parse(strRequest);

                var response = routingTable.MatchRequest(request);

                if (response.PreRenderAction != null)
                {
                    response.PreRenderAction(request, response);
                }

                WriteResponse(networkStream, response);
            }
        }

        private string ReadRequest(NetworkStream networkStream)
        {
            int bufferLength = 1024;
            byte[] buffer = new byte[bufferLength];

            int totalBytes = 0;

            var requestBuilder = new StringBuilder();

            do
            {
                var bytesRead = networkStream.Read(buffer, 0, buffer.Length);

                totalBytes += bytesRead;

                if (totalBytes > 10 * bufferLength)
                {
                    throw new InvalidOperationException($"Request is too large.");
                }

                requestBuilder.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

            } while (networkStream.DataAvailable);

            return requestBuilder.ToString();
        }

        private void WriteResponse(NetworkStream networkStream, Response response)
        {
            var responseBytes = Encoding.UTF8.GetBytes(response.ToString());

            networkStream.Write(responseBytes);
        }
    }
}
