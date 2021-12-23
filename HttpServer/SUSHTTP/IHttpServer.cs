using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUSHTTP
{
    public interface IHttpServer
    { 
        Task StartAsync(int port);
    }
}
