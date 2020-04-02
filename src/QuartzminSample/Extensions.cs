using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace QuartzminSample
{
    public static class Extensions
    {
        public static IApplicationBuilder UseQuartzmin(this IApplicationBuilder app, string schedulerRoute)
        {
            var isEmpty = string.IsNullOrEmpty(schedulerRoute);
            var virtualPathRoot = isEmpty ? string.Empty : $"{schedulerRoute}";

            var fileProvider = new ManifestEmbeddedFileProvider(typeof(Quartzmin.Services).Assembly, "Content");
            FileServerOptions fileServerOptions = new FileServerOptions
            {
                RequestPath = new PathString(!isEmpty ? $"{virtualPathRoot}/Content" : "/Content"),
                EnableDefaultFiles = false,
                EnableDirectoryBrowsing = false,
                FileProvider = fileProvider
            };

            app.UseFileServer(fileServerOptions);

            return app;
        }
    }
}
