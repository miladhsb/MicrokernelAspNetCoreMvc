using MicrokernelAspNetCoreMvc.Infrastructure.Plugins;
using MicrokernelAspNetCoreMvc.Infrastructure.TypeSearch;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrokernelAspNetCoreMvc.Infrastructure
{
    public static class StartupBaseApp
    {

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
           
            var typeSearcher = new TypeSearcher();
            services.AddSingleton<ITypeSearcher>(typeSearcher);

            var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

            //Register startup
            var instancesBefore = startupConfigurations
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
                .Where(startup => startup!.BeforeConfigure)
                .OrderBy(startup => startup.Priority);

            //configure services
            foreach (var instance in instancesBefore)
                instance.ConfigureServices(services, configuration);

  
          
        }

        public static void ConfigureRequestPipeline(IApplicationBuilder application,
            IWebHostEnvironment webHostEnvironment)
        {
            //find startup configurations provided by other assemblies
            var typeSearcher = new TypeSearcher();
            var startupConfigurations = typeSearcher.ClassesOfType<IStartupApplication>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (IStartupApplication)Activator.CreateInstance(startup))
                .OrderBy(startup => startup!.Priority);

            //configure request pipeline
            foreach (var instance in instances)
                instance.Configure(application, webHostEnvironment);
        }
    }
}
