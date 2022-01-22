using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebServer.Server.Controllers;
using WebServer.Server.HTTP;

namespace WebServer.Controllers
{
    public class UsersController : Controller
    {
        private const string Username = "user";
        private const string Password = "user123";

        public UsersController(Request request) : base(request)
        {
        }

        public Response Login() => View();

        public Response LogInUser()
        {
            Request.Session.Clear();

            var cookies = new CookieCollection();
            string text;

            var usernameMatches = Request.Form["Username"] == Username;
            var passwordMatches = Request.Form["Password"] == Password;

            if (usernameMatches && passwordMatches)
            {
                Request.Session[Session.SessionUserKey] = "MyUserId";
                cookies.Add(Session.SessionCookieName, Request.Session.Id);

                text = "<h3>Logged successfully!</h3>";
            }
            else
            {
                return Redirect("/Login");
            }

            return Html(text, cookies);
        }

        public Response Logout()
        {
            Request.Session.Clear();

            return Html("<h3>Logged out successfully!</h3>");
        }

        public Response GetUserData()
        {
            if (Request.Session.ContainsKey(Session.SessionUserKey))
            {
                return Html($"<h3>Currently logged-in user is username '{Username}'</h3>");
            }
            else
            {
                return Html($"<h3>You should first log in - <a href='/Login'>Login</a></h3>");
            }
        }
    }
}
