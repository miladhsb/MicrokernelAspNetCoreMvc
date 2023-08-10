using MicrokernelAspNetCoreMvc.Infrastructure.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplePlugin01
{
    public class PluginRegister : IPlugin
    {
        public PluginInfo PluginInfo { get; set; }

        public string ConfigurationUrl()
        {
            return "/SamplePlugin01/Configure";
        }

        public Task Install()
        {
            Console.WriteLine("Installed SamplePlugin01");
            return  Task.CompletedTask;
        }

        public Task Uninstall()
        {
            Console.WriteLine("unistalled SamplePlugin01");
            return Task.CompletedTask;
        }
    }
}
