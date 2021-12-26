using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SUSHTTP;

namespace MVCFramework
{
    public interface IMvcApplication
    {
        void ConfigureServices();

        void Configure(List<Route> routes);
    }
}
