using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AAA_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
              .ConfigureWebHostDefaults(webBuilder =>
               {
                webBuilder.UseStartup<Startup>();
               });
    }
}
//WebHost.CreateDefaultBuilder(args)
              // .UseKestrel()
             //  .UseContentRoot(Directory.GetCurrentDirectory())
             //  .UseUrls("http://*:5001")
             //  .UseStartup<Startup>()
             //  .Build();

//Host.CreateDefaultBuilder(args)
              // .ConfigureWebHostDefaults(webBuilder =>
              // {
                //   webBuilder.UseStartup<Startup>();
             //  });