using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUSHTTP
{
    public class Cookie
    {
        public Cookie(string name, string value)
        {
            Name = name;
            Value = value;
        }
        public Cookie(string cookieAsString)
        {
            var parts = cookieAsString.Split('=', 2);

            Name = parts[0];
            Value = parts[1];
        }
        public string Name { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Name}={Value}";
        }
    }
}
