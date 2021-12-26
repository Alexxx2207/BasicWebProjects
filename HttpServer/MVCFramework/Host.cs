using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUSHTTP;

namespace MVCFramework
{
    public class Host
    {
        public async Task RunAsync(IMvcApplication application, int port = 80)
        {
            List<Route> tableRoutes = new List<Route>();

            application.ConfigureServices();
            application.Configure(tableRoutes);
            
            IHttpServer server = new HttpServer(tableRoutes);

            await server.StartAsync(port);
        }
    }
}
