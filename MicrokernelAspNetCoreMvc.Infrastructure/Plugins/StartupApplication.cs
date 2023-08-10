using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrokernelAspNetCoreMvc.Infrastructure.Plugins
{
    public class StartupApplication : IStartupApplication
    {
        public int Priority => 0;

        public bool BeforeConfigure => true;


        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
            Console.WriteLine("main StartupApplication Configure");
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine("main StartupApplication ConfigureServices");
        }

    
    }
}
