using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WebServer.Server;
using WebServer.Server.HTTP;
using WebServer.Server.Responses;

namespace WebServer
{
    public class StartUp
    {
        private const string HtmlForm = @"<form action='/HTML' method='POST'>
   Name: <input type='text' name='Name'/>
   Age: <input type='number' name ='Age'/>
<input type='submit' value ='Save' />
</form>";

        private const string DownloadForm = @"<form action='/Content' method='Post' >
<input type='submit' value='Download Sites Content' />
</form>";

        private const string fileName = "content.txt";

        private const string LoginForm = @"<form action='/Login' method='POST'>
   Username: <input type='text' name='Username'/>
   Password: <input type='text' name='Password'/>
   <input type='submit' value ='Log In' /> 
</form>";

        private const string Username = "user";
        private const string Password = "user123";

        public static async Task Main()
        {
            await DownloadSitesAsTextFile(fileName, new string[]
                { "https://judge.softuni.org/", "https://softuni.org" });

            await new HttpServer(routes => routes
                .MapGet("/", new TextResponse("Hello from the server!"))
                .MapGet("/Redirect", new RedirectResponse("https://softuni.org/"))
                .MapGet("/HTML", new HtmlResponse(HtmlForm))
                .MapPost("/HTML", new TextResponse("", AddFormDataAction))
                .MapGet("/Content",  new HtmlResponse(DownloadForm))
                .MapPost("/Content", new TextFileResponse(fileName))
                .MapGet("/Cookies", new HtmlResponse("",AddCookiesAction))
                .MapGet("/Session", new HtmlResponse("", DisplaySessionInfoAction))
                .MapGet("/Login", new HtmlResponse(LoginForm))
                .MapPost("/Login", new HtmlResponse("", LoginAction))
                .MapGet("/Logout", new HtmlResponse("", LogoutAction))
                .MapGet("/UserProfile", new HtmlResponse("", GetUserDataAction))
                )
            .Start();
        }

        private static void GetUserDataAction(Request request, Response response)
        {
            if (request.Session.ContainsKey(Session.SessionUserKey))
            {
                response.Body = $"<h3>Currently logged-in user is username '{Username}'</h3>";
            }
            else
            {
                response.Body = $"<h3>You should first log in - <a href='/Login'>Login</a></h3>";
            }
        }

        private static void LogoutAction(Request request, Response response)
        {
            request.Session.Clear();

            response.Body = "<h3>Logged out successfully!</h3>";
        }

        private static void LoginAction(Request request, Response response)
        { 
            request.Session.Clear();

            var bodyText = "";

            var usernameMatches = request.Form["Username"] == Username;
            var passwordMatches = request.Form["Password"] == Password;

            if (usernameMatches && passwordMatches)
            {
                request.Session[Session.SessionUserKey] = "MyUserId";
                response.Cookies.Add(Session.SessionCookieName, request.Session.Id);

                bodyText = "<h3>Logged successfully!</h3>";
            }
            else 
            {
                bodyText = LoginForm;
            }

            response.Body = bodyText;
            
        }

        private static void DisplaySessionInfoAction(Request request, Response response)
        { 
            var sessionExists = request.Session
                                .ContainsKey(Session.SessionCurrentDateKey);

            var bodyText = "";

            if (sessionExists)
            {
                var currentDate = request.Session[Session.SessionCurrentDateKey];

                bodyText = $"Stored date: {currentDate}!";
            }
            else
            {
                bodyText = "Current date stored!";
            }

            response.Body = bodyText;
        }

        private static void AddCookiesAction(Request request, Response response)
        { 
            var requestHasCookies = request.Cookies.Any(c => c.Name != Session.SessionCookieName);

            var bodyText = "";

            if (requestHasCookies)
            {
                var cookiesString = new StringBuilder();

                cookiesString.AppendLine("<h1>Cookies</h1>");

                cookiesString.Append("<table border='1'><tr><th>Name</th><th>Value</th></tr>");

                foreach (var cookie in request.Cookies)
                {

                    cookiesString.Append("<tr>");
                    cookiesString.Append($"<td>{HttpUtility.HtmlEncode(cookie.Name)}</td>");
                    cookiesString.Append($"<td>{HttpUtility.HtmlEncode(cookie.Value)}</td>");
                    cookiesString.Append("</tr>");
                }
                cookiesString.Append($"</table>");

                bodyText = cookiesString.ToString();
            }
            else
            {
                bodyText = "<h1>Cookies set!</h1>";
            }

            if (!requestHasCookies)
            {
                response.Cookies.Add("My-Cookie", "My-Value");
                response.Cookies.Add("My-Second-Cookie", "My-Second-Value");
            }

            response.Body = bodyText;
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
            var  downloads = new List<Task<string>>();

            foreach (var url in urls)
            {
                downloads.Add(DownloadWebSiteContent(url));
            }

            var responses = await Task.WhenAll(downloads);

            var fileContent = string.Join(Environment.NewLine + new string('-', 100), responses);

            await File.WriteAllTextAsync(fileName, fileContent);
        }

        private static void AddFormDataAction(Request request,  Response response)
        {
            response.Body = "";

            foreach (var (key, value) in request.Form)
            {
                response.Body += $"{key} - {value}";
                response.Body += Environment.NewLine;
            }
        }
    }
}
