using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUSHTTP.Enums;

namespace SUSHTTP
{
    public class HttpRequest
    {
        public HttpRequest(string request)
        {
            Headers = new List<Header>();
            Cookies = new List<Cookie>();

            var lines = request.Split(GlobalConstants.GlobalConstants.NewLine, StringSplitOptions.None);

            var requestLineParts = lines[0].Split(' ', StringSplitOptions.None);

            Method = (QueryMethod)Enum.Parse(typeof(QueryMethod), requestLineParts[0], true);
            Path = requestLineParts[1];

            bool isHeaderLine = true;

            StringBuilder bodyBuilder = new StringBuilder();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    isHeaderLine = false;
                    continue;
                }
                else
                {
                    if (isHeaderLine)
                    {
                        Headers.Add(new Header(line));
                    }
                    else
                    {
                        bodyBuilder.AppendLine(line);
                    }
                }
            }

            if (Headers.Any(h => h.Name == GlobalConstants.GlobalConstants.CookieHeaderName))
            {
                var cookies = Headers
                                .FirstOrDefault(h => h.Name == GlobalConstants.GlobalConstants.CookieHeaderName)
                                .Value
                                .Split("; ", StringSplitOptions.RemoveEmptyEntries)
                                .Select(c => new Cookie(c))
                                .ToArray();

                foreach (var cookie in cookies)
                {
                    Cookies.Add(cookie);
                }
            }

            Body = bodyBuilder.ToString();
        }

        public string Path { get; set; }
        public QueryMethod Method { get; set; }
        public ICollection<Header> Headers { get; set; }
        public ICollection<Cookie> Cookies { get; set; }
        public string Body { get; set; }
    }
}
