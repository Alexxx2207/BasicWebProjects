using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Server.HTTP
{
    public class Response
    {
        public StatusCode StatusCode { get; set; }

        public HeaderCollection Headers { get; }

        public string Body { get; set; }

        public Action<Request, Response> PreRenderAction { get; protected set; }

        public Response(StatusCode statusCode)
        {
            StatusCode = statusCode;
            Headers = new HeaderCollection();

            Headers.Add(Header.Server, "My Web Server");
            Headers.Add(Header.Date, $"{DateTime.UtcNow:r}");
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"HTTP/1.1 {(int)StatusCode} {StatusCode}");

            foreach (var header in Headers)
            {
                sb.AppendLine(header.ToString());
            }

            sb.AppendLine();

            if (!string.IsNullOrEmpty(Body))
            { 
                sb.Append(Body);
            }

            return sb.ToString();
        }
    }
}
