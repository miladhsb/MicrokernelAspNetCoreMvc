using MicrokernelAspNetCoreMvc.Infrastructure.Plugins;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin01
{
    public class PluginStartup : IStartupApplication
    {
        public int Priority => 2;

        public bool BeforeConfigure => true;

        public void Configure(IApplicationBuilder application, IWebHostEnvironment webHostEnvironment)
        {
            Console.WriteLine("PluginStartup SamplePlugin01 Configure");
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine("PluginStartup SamplePlugin01 ConfigureServices");
        }
    }
}
