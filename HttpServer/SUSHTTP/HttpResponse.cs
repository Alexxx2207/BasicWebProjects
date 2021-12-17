using System;
using System.Collections.Generic;
using System.Linq;
using SUSHTTP.Enums;
using System.Text;
using System.Threading.Tasks;

namespace SUSHTTP
{
    public class HttpResponse
    {
        public HttpResponse(string contentType, byte[] body, HttpStatusCode statusCode = HttpStatusCode.Ok)
        {
            if (body == null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            HttpStatusCode = statusCode;

            Body = body;

            Headers = new List<Header>();
            Cookies = new List<Cookie>();

            Headers.Add(new Header("Content-Type", contentType));
            Headers.Add(new Header("Content-Lenth", body.Length.ToString()));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"HTTP/1.1 {(int)HttpStatusCode} {HttpStatusCode}");
            sb.Append(GlobalConstants.GlobalConstants.NewLine);

            foreach (var header in Headers)
            {
                sb.Append($"{header}" + GlobalConstants.GlobalConstants.NewLine);
            }

            foreach (var cookie in Cookies)
            {
                sb.Append($"Set-Cookie: {cookie}" + GlobalConstants.GlobalConstants.NewLine);
            }

            sb.Append(GlobalConstants.GlobalConstants.NewLine);

            return sb.ToString();
        }

        public HttpStatusCode HttpStatusCode { get; set; }

        public ICollection<Header> Headers { get; set; }
        public ICollection<Cookie> Cookies { get; set; }

        public byte[] Body { get; set; }
    }
}
