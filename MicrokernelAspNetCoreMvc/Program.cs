using MicrokernelAspNetCoreMvc.AppViewExpander;
using MicrokernelAspNetCoreMvc.Infrastructure;
using MicrokernelAspNetCoreMvc.Infrastructure.Extensions;
using MicrokernelAspNetCoreMvc.Infrastructure.Plugins;
using Microsoft.Extensions.Hosting.Internal;

namespace MicrokernelAspNetCoreMvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            CommonPath.WebHostEnvironment = builder.Environment.WebRootPath;
            CommonPath.BaseDirectory = builder.Environment.ContentRootPath;
            // Add services to the container.
           var MvcApp= builder.Services.AddControllersWithViews().AddRazorOptions(p =>
           {
               p.ViewLocationExpanders.Add(new ThemeViewExpander());
           }).AddRazorRuntimeCompilation();


           PluginManager.Load(MvcApp, builder.Configuration);

           StartupBaseApp.ConfigureServices(builder.Services, builder.Configuration);
           var app = builder.Build();

           
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            StartupBaseApp.ConfigureRequestPipeline(app,app.Environment);

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}