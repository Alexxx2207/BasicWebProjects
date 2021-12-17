using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUSHTTP
{
    public class ResponseCookie : Cookie
    {
        public ResponseCookie(string name, string value)
            : base(name, value)
        {
            Path = "/";
        }

        public string Path { get; set; }

        public bool HttpOnly { get; set; }

        public int MaxAge { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append($"{Name}={Value}; Path={Path};");

            if (MaxAge != 0)
            {
                sb.Append($" Max-Age={MaxAge};");
            }

            if (HttpOnly)
            {
                sb.Append($" HttpOnly;");
            }

            return sb.ToString();
        }
    }
}
