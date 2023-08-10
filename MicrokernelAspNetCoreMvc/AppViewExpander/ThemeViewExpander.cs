using Microsoft.AspNetCore.Mvc.Razor;

namespace MicrokernelAspNetCoreMvc.AppViewExpander
{
   
    public class ThemeViewExpander : IViewLocationExpander
    {
        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context, IEnumerable<string> viewLocations)
        {
            viewLocations = new[] {
                        $"/Themes/MyTheme/Views/{{1}}/{{0}}.cshtml",
                        $"/Themes/MyTheme/Views/Shared/{{0}}.cshtml"
                    }
                   .Concat(viewLocations);

            return viewLocations;
        }

        public void PopulateValues(ViewLocationExpanderContext context)
        {

        }
    }
}
