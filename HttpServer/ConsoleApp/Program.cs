using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVCFramework;
using SUSHTTP;
using SUSHTTP.Enums;

namespace ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.Write("Port: ");
            int port = int.Parse(Console.ReadLine());

            await new Host().RunAsync(new StartUp(), port);
        }
    }
}
