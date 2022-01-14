﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WebServer.Server.HTTP
{
    public class Request
    {
        public Method Method { get; private set; }
        public string Url { get; private set; }
        public HeaderCollection Headers { get; private set; }
        public string Body { get; private set; }

        public IReadOnlyDictionary<string, string> Form { get; private set; }

        public static Request Parse(string request)
        {
            var lines = request.Split("\r\n");

            var firstLine = lines.First().Split(" ");

            var method = ParseMethod(firstLine.First());

            var url = firstLine[1];

            var headers = ParseHeaders(lines.Skip(1));

            var bodyLines = lines.Skip(headers.Count + 2);

            var body = string.Join("\r\n", bodyLines);

            var form = ParseForm(headers, body);

            return new Request
            { 
                Method = method,
                Url = url,
                Headers = headers,
                Body = body,
                Form = form,
            };
        }

        private static Dictionary<string,string> ParseForm(HeaderCollection headers, string body)
        {
            var formCollection = new Dictionary<string,string>();

            if (headers.Contains(Header.ContentType) &&
                headers[Header.ContentType] == ContentType.FormUrlEncoded)
            {
                var parsedResult = ParseFormData(body);

                foreach (var (name, value) in parsedResult)
                {
                    formCollection.Add(name, value);
                }
            }

            return formCollection;
        }

        private static Dictionary<string, string> ParseFormData(string bodyLines)
        {
            return HttpUtility.UrlDecode(bodyLines)
                .Split('&')
                .Select(part => part.Split("="))
                .Where(part => part.Length == 2)
                .ToDictionary(
                    part => part[0],
                    part => part[1],
                    StringComparer.InvariantCultureIgnoreCase
                );
        }

        private static HeaderCollection ParseHeaders(IEnumerable<string> enumerable)
        {
            var headers = new HeaderCollection();

            foreach (var line in enumerable)
            { 
                if(line == string.Empty)
                {
                    break;
                }

                var headerParts = line.Split(":", 2);

                if (headerParts.Length != 2)
                {
                    throw new InvalidOperationException("Request is not valid.");
                }

                var headerName = headerParts[0];
                var headerValue = headerParts[1].Trim();
            
                headers.Add(headerName, headerValue);

            }
            return headers;
        }

        private static Method ParseMethod(string method)
        {
            try
            {
                return (Method)Enum.Parse(typeof(Method), method, true);
            }
            catch (Exception)
            {

                throw new InvalidOperationException($"Method {method} is not suppored");
            }
        }
    }

    
}