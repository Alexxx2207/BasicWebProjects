using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleApp.Controllers;
using MVCFramework;
using SUSHTTP;
using SUSHTTP.Enums;

namespace ConsoleApp
{
    public class StartUp : IMvcApplication
    {
        public void Configure(List<Route> routes)
        {
            routes.Add(new Route("/", HttpMethod.Get, new HomeController().Index));
            routes.Add(new Route("/users/login", HttpMethod.Get, new UsersController().LogIn));
            routes.Add(new Route("/users/login", HttpMethod.Post, new UsersController().DoLogIn));
            routes.Add(new Route("/users/register", HttpMethod.Get, new UsersController().Register));
            routes.Add(new Route("/users/register", HttpMethod.Post, new UsersController().DoLogIn));
            routes.Add(new Route("/cards/add", HttpMethod.Get, new CardsController().Add));
            routes.Add(new Route("/cards/all", HttpMethod.Get, new CardsController().All));
            routes.Add(new Route("/cards/collection", HttpMethod.Get, new CardsController().Collection));


            routes.Add(new Route("/favicon.ico", HttpMethod.Get, new StaticFilesController().Favicon));
            routes.Add(new Route("/css/bootstrap.min.css", HttpMethod.Get, new StaticFilesController().BootstrapCSS));
            routes.Add(new Route("/css/custom.css", HttpMethod.Get, new StaticFilesController().CustomCSS));
            routes.Add(new Route("/js/bootstrap.bundle.min.js", HttpMethod.Get, new StaticFilesController().BootstrapJS));
            routes.Add(new Route("/js/custom.js", HttpMethod.Get, new StaticFilesController().CustomJS));

        }

        public void ConfigureServices()
        {
        }
    }
}
