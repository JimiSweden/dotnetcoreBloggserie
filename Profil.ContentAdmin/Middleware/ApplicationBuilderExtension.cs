using System.IO;
using Microsoft.Extensions.FileProviders;

// adding this class to aspnet core builder as it is common convention to put IApplicationBuilder extensions in this MS namespace (the same as what we are extending)
namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseNodeModules(this IApplicationBuilder app, string projectRoot)
        {
            var path = Path.Combine(projectRoot, "node_modules");
            var fileProvider = new PhysicalFileProvider(path);

            var options = new StaticFileOptions
            {
                RequestPath = "/node_modules", //only listen to path with "node_modules"
                FileProvider = fileProvider
            };

            app.UseStaticFiles(options);

            return app;
        }

        public static IApplicationBuilder UseContentFiles(this IApplicationBuilder app, string projectRoot)
        {
            var path = Path.Combine(projectRoot, "content");
            var fileProvider = new PhysicalFileProvider(path);

            var options = new StaticFileOptions
            {
                RequestPath = "/content", //only listen to path with "content"
                FileProvider = fileProvider
            };

            app.UseStaticFiles(options);

            return app;
        }

    }
}