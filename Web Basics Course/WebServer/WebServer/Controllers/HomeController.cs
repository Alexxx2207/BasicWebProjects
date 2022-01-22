using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebServer.Models;
using WebServer.Server.Controllers;
using WebServer.Server.HTTP;

namespace WebServer.Controllers
{
    public class HomeController : Controller
    {
        private const string fileName = "content.txt";

        public HomeController(Request request) : base(request)
        {
        }

        public Response Index()
        {
            return Text("Hello from the server!");
        }

        public Response Redirect()
        { 
            return Redirect("Https://softuni.org/");
        }

        public Response Html() => View();

        public Response HtmlFormPost()
        {
            var name = this.Request.Form["Name"];
            var age = this.Request.Form["Age"];

            var model = new FormViewModel()
            {
                Name = name,
                Age = int.Parse(age)
            };

            return View(model);
        }

        public Response Content() => View();

        public Response DownloadContent()
        {
            DownloadSitesAsTextFile(fileName, new string[]
                { "https://judge.softuni.org/", "https://softuni.org" }).Wait();

            return File(fileName);
        }

        public Response Cookies()
        {
            if (Request.Cookies.Any(c => c.Name != WebServer.Server.HTTP.Session.SessionCookieName))
            {
                var cookiesString = new StringBuilder();

                cookiesString.AppendLine("<h1>Cookies</h1>");

                cookiesString.Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in Request.Cookies)
                {

                    cookiesString.Append("<tr>");
                    cookiesString.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookiesString.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookiesString.Append("</tr>");
                }
                cookiesString.Append($"</table>");

                return Html(cookiesString.ToString());
            }

            var cookies = new CookieCollection();
            cookies.Add("My-Cookie", "My-Value");
            cookies.Add("My-Second-Cookie", "My-Second-Value");

            return Html("<h1>Cookies set!</h1>", cookies);
        }

        public Response Session()
        {
            var sessionExists = Request.Session
                                .ContainsKey(WebServer.Server.HTTP.Session.SessionCurrentDateKey);

            var bodyText = "";

            if (sessionExists)
            {
                var currentDate = Request.Session[WebServer.Server.HTTP.Session.SessionCurrentDateKey];

                bodyText = $"Stored date: {currentDate}!";
            }
            else
            {
                bodyText = "Current date stored!";
            }

            return Text(bodyText);
        }

        private static async Task<string> DownloadWebSiteContent(string url)
        {
            var httpClient = new HttpClient();

            using (httpClient)
            {
                var response = await httpClient.GetAsync(url);

                var responseHtml = await response.Content.ReadAsStringAsync();

                return responseHtml.Substring(0, 2000);
            }
        }

        private static async Task DownloadSitesAsTextFile(string fileName, string[] urls)
        {
            var downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }

            var responses = await Task.WhenAll(downloads);

            var fileContent = string.Join(Environment.NewLine + new string('-', 100), responses);

            await System.IO.File.WriteAllTextAsync(fileName, fileContent);
        }
    }
}
