using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCFramework;
using SUSHTTP;

namespace ConsoleApp.Controllers
{
    public class UsersController : Controller
    {
        public HttpResponse LogIn(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse Register(HttpRequest request)
        {
            return this.View();
        }

        public HttpResponse DoLogIn(HttpRequest request)
        {
            return this.Redirect("/");
        }
    }
}
