using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrokernelAspNetCoreMvc.Infrastructure.Plugins
{
    public interface IPlugin
    {
        /// <summary>
        /// Gets a configuration URL
        /// </summary>
        string ConfigurationUrl();

        /// <summary>
        /// Gets or sets the plugin info
        /// </summary>
        PluginInfo PluginInfo { get; set; }

        /// <summary>
        /// Install plugin
        /// </summary>
        Task Install();

        /// <summary>
        /// Uninstall plugin
        /// </summary>
        Task Uninstall();
    }
}
