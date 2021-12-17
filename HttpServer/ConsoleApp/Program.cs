using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUSHTTP;

namespace ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());

            var server = new HttpServer();

            server.AddRoute("/", HomePage);
            server.AddRoute("/about", AboutPage);
            server.AddRoute("/favicon.ico", Favicon);

            await server.StartAsync(port);
        }

        private static HttpResponse HomePage(HttpRequest request)
        {
            var body = "<h1>Welcome</h1>";
            var bodyByteArray = Encoding.UTF8.GetBytes(body);
            var httpResponse = new HttpResponse("text/html", bodyByteArray);

            return httpResponse;
        }
        private static HttpResponse AboutPage(HttpRequest request)
        {
            var body = "<h1>About...</h1>";
            var bodyByteArray = Encoding.UTF8.GetBytes(body);
            var httpResponse = new HttpResponse("text/html", bodyByteArray);

            return httpResponse;
        }
       
        private static HttpResponse Favicon(HttpRequest request)
        {
            var fileBytes = File.ReadAllBytes(@"wwwroot/favicon.ico");
            var response = new HttpResponse("image/vnd.microsoft.icon", fileBytes);
            return response;
        }
    }
}
