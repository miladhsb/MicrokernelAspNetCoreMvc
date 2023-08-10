using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrokernelAspNetCoreMvc.Infrastructure.Extensions
{
    public class CommonPath
    {

        public static string PluginsCopyPath
        {
            get
            {
                return Path.Combine(BaseDirectory, PluginsPath, "bin");
            }
        }

        public static string ThemePath
        {
            get
            {
                return Path.Combine(BaseDirectory, "Themes");
            }
        }

        /// <summary>
        /// Maps a theme path to a physical disk path.
        /// </summary>
        /// <returns>The physical path.</returns>
        public static string PluginsPath
        {
            get
            {
                return Path.Combine(BaseDirectory, "Plugins");
            }
        }

        public static string WebRootPath
        {
            get
            {
                return Path.Combine(WebHostEnvironment, Param);
            }
        }

        public static string Param { get; set; } = "";
        public static string WebHostEnvironment { get; set; }
        public static string BaseDirectory { get; set; }
    }
}
