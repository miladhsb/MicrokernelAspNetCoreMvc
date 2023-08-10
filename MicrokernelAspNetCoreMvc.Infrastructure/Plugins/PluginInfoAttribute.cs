using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrokernelAspNetCoreMvc.Infrastructure.Plugins
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginInfoAttribute : Attribute
    {
        public string Group { get; set; } = string.Empty;
        public string FriendlyName { get; set; } = string.Empty;
        public string SystemName { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string SupportedVersion { get; set; }
        public string Version { get; set; }
    }
}
