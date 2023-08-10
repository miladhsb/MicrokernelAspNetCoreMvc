using MicrokernelAspNetCoreMvc.Infrastructure.Plugins;
using MicrokernelAspNetCoreMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace MicrokernelAspNetCoreMvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public HomeController(ILogger<HomeController> logger, IServiceProvider serviceProvider, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            this._serviceProvider = serviceProvider;
            this._hostApplicationLifetime = hostApplicationLifetime;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PluginsList()
        {
            var pluginInfos = PluginManager.ReferencedPlugins.ToList();

            return View(pluginInfos);
        }


        public  IActionResult ConfigurePlugin(string SystemName)
        {
            var Pluginitem = PluginManager.ReferencedPlugins.FirstOrDefault(p => p.SystemName == SystemName);
            if (Pluginitem == null)
            {
                throw new NullReferenceException("پلاگین یافت نشد");
            }

            var plugin = Pluginitem.Instance<IPlugin>(_serviceProvider);
            var PluginUrl =   plugin.ConfigurationUrl();
            return Redirect(PluginUrl);
        }

        public async Task<IActionResult> PluginsInstall(string SystemName)
        {

            var Pluginitem = PluginManager.ReferencedPlugins.FirstOrDefault(p => p.SystemName == SystemName);
            if (Pluginitem == null)
            {
                throw new NullReferenceException("پلاگین یافت نشد");
            }

            var plugin = Pluginitem.Instance<IPlugin>(_serviceProvider);
            Pluginitem.Installed = true;
            await plugin.Install();



            // _hostApplicationLifetime.StopApplication();
            return RedirectToAction("PluginsList");
        }

        public async Task<IActionResult> PluginsUnistall(string SystemName)
        {

            var Pluginitem = PluginManager.ReferencedPlugins.FirstOrDefault(p => p.SystemName == SystemName);
            if (Pluginitem == null)
            {
                throw new NullReferenceException("پلاگین یافت نشد");
            }

            var plugin = Pluginitem.Instance<IPlugin>(_serviceProvider);
            Pluginitem.Installed = false;
            await plugin.Uninstall();



            // _hostApplicationLifetime.StopApplication();
            return RedirectToAction("PluginsList");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}