using MicrokernelAspNetCoreMvc.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MicrokernelAspNetCoreMvc.Infrastructure.Plugins
{
    public static class PluginManager
    {
       

        private static object _synLock = new object();

    

        private static DirectoryInfo _pluginFolder;
       

      

        #region Methods

        public static IEnumerable<PluginInfo> ReferencedPlugins { get; set; }


        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Load(IMvcBuilder mvcCoreBuilder, IConfiguration configuration)
        {
           
            lock (_synLock)
            {
                if (mvcCoreBuilder == null)
                    throw new ArgumentNullException(nameof(mvcCoreBuilder));

                _pluginFolder = new DirectoryInfo(CommonPath.PluginsPath);
             

                var referencedPlugins = new List<PluginInfo>();
                try
                {

                    Directory.CreateDirectory(_pluginFolder.FullName);
                   

                    //load description files
                    foreach (var plugin in GetPluginInfo())
                    {
                      
                        
                        //some validation
                        if (string.IsNullOrWhiteSpace(plugin.SystemName))
                            throw new Exception($"The plugin '{plugin.SystemName}' has no system name.");
                        if (referencedPlugins.Contains(plugin))
                            throw new Exception($"The plugin with '{plugin.SystemName}' system name is already defined");

                        //set 'Installed' property
                        plugin.Installed = false;
                        try
                        {
                         
                                //remove deps.json files 
                                var depsFiles = plugin.OriginalAssemblyFile.Directory!.GetFiles("*.deps.json", SearchOption.TopDirectoryOnly);
                                foreach (var f in depsFiles)
                                {
                                    try
                                    {
                                        File.Delete(f.FullName);
                                    }
                                    catch (Exception exc)
                                    {
                                     Console.WriteLine(exc.Message);
                                    }
                                }
                           

                            //main plugin file
                            AddApplicationPart(mvcCoreBuilder, plugin.ReferencedAssembly, plugin.SystemName, plugin.PluginFileName);

                            //register interface for IPlugin 
                            RegisterPluginInterface(mvcCoreBuilder, plugin.ReferencedAssembly);

                            //init plugin type
                            foreach (var t in plugin.ReferencedAssembly.GetTypes())
                                if (typeof(IPlugin).IsAssignableFrom(t))
                                    if (!t.GetTypeInfo().IsInterface)
                                        if (t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract)
                                        {
                                            plugin.PluginType = t;
                                            break;
                                        }

                            referencedPlugins.Add(plugin);
                        }
                        catch (ReflectionTypeLoadException ex)
                        {
                            var msg = $"Plugin '{plugin.FriendlyName}'. ";
                            msg = ex.LoaderExceptions.Aggregate(msg, (current, e) => current + (e!.Message + Environment.NewLine));

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                        catch (Exception ex)
                        {
                            var msg = $"Plugin '{plugin.FriendlyName}'. {ex.Message}";

                            var fail = new Exception(msg, ex);
                            throw fail;
                        }
                    }
                }
                catch (Exception ex)
                {
                    var msg = string.Empty;
                    for (var e = ex; e != null; e = e.InnerException)
                        msg += e.Message + Environment.NewLine;

                    var fail = new Exception(msg, ex);
                    throw fail;
                }

                ReferencedPlugins = referencedPlugins;
            }
        }
      

        public static PluginInfo FindPlugin(Type typeAssembly)
        {
            if (typeAssembly == null)
                throw new ArgumentNullException(nameof(typeAssembly));

            return ReferencedPlugins?.FirstOrDefault(plugin => plugin.ReferencedAssembly != null
                                                               && plugin.ReferencedAssembly.FullName!.Equals(typeAssembly.GetTypeInfo().Assembly.FullName, StringComparison.OrdinalIgnoreCase));
        }


        #endregion

        #region Utilities

        private static IList<PluginInfo> GetPluginInfo()
        {
            if (_pluginFolder == null)
                throw new ArgumentNullException(nameof(_pluginFolder));

            var result = new List<PluginInfo>();
            foreach (var pluginFile in _pluginFolder.GetFiles("*.dll", SearchOption.AllDirectories))
            {
                if (!IsPackagePluginFolder(pluginFile.Directory))
                    continue;
                //prepare plugin info
                var plugins = PreparePluginInfo(pluginFile);
                if (plugins == null)
                    continue;

                result.Add(plugins);
            }

            return result;
        }

        private static PluginInfo PreparePluginInfo(FileInfo pluginFile)
        {
            var plug =  pluginFile;

            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(plug.FullName);

            var pluginInfo = assembly.GetCustomAttribute<PluginInfoAttribute>();
            if (pluginInfo == null)
            {
                return null;
            }

            var plugin = new PluginInfo
            {
                FriendlyName = pluginInfo.FriendlyName,
                Group = pluginInfo.Group,
                SystemName = pluginInfo.SystemName,
                Version = pluginInfo.Version,
                SupportedVersion = pluginInfo.SupportedVersion,
                Author = pluginInfo.Author,
                PluginFileName = plug.Name,
                OriginalAssemblyFile = pluginFile,
                ReferencedAssembly = assembly
            };

            return plugin;
        }

      
        private static bool Matches(string fullName, string pattern)
        {
            return Regex.IsMatch(fullName, pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private static void AddApplicationPart(IMvcBuilder mvcCoreBuilder,
            Assembly assembly, string systemName, string filename)
        {
            try
            {
              
                mvcCoreBuilder.AddApplicationPart(assembly);

                var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(assembly, throwOnError: false);
                foreach (var relatedAssembly in relatedAssemblies)
                {
                    var applicationPartFactory = ApplicationPartFactory.GetApplicationPartFactory(relatedAssembly);
                    foreach (var part in applicationPartFactory.GetApplicationParts(relatedAssembly))
                    {
                        mvcCoreBuilder.PartManager.ApplicationParts.Add(part);
                    }
                }
            }
            catch (Exception ex)
            {
             
                throw new InvalidOperationException($"The plugin directory for the {systemName} file exists in a folder outside of the allowed grandnode folder hierarchy - exception because of {filename} - exception: {ex.Message}");
            }
        }

        private static void RegisterPluginInterface(IMvcBuilder mvcCoreBuilder, Assembly assembly)
        {
            try
            {
                foreach (var t in assembly.GetTypes())
                    if (typeof(IPlugin).IsAssignableFrom(t))
                        mvcCoreBuilder.Services.AddScoped(t);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

     
        private static bool IsPackagePluginFolder(DirectoryInfo folder)
        {
            if (folder == null) return false;
            if (folder.Name.Equals("bin", StringComparison.InvariantCultureIgnoreCase)) return false;
            return folder.Parent != null && folder.Parent.Name.Equals("Plugins", StringComparison.OrdinalIgnoreCase);
        }


        #endregion
    }
}
