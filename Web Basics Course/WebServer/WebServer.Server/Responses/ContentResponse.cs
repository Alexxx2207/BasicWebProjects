using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.Common;
using WebServer.Server.HTTP;

namespace WebServer.Server.Responses
{
    public class ContentResponse : Response
    {
        public ContentResponse(string content, string contentType) : base(StatusCode.OK)
        {
            Guard.AgainstNull(content);
            Guard.AgainstNull(contentType);

            this.Headers.Add(Header.ContentType, contentType);
        
            this.Body = content;
        }

        public override string ToString()
        {
            if (Body != null)
            {
                var contentLength = Encoding.UTF8.GetByteCount(Body).ToString();

                Headers.Add(Header.ContentLength, contentLength);
            }

            return base.ToString();
        }
    }
}
