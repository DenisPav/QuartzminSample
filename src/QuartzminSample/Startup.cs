using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartzmin;

namespace QuartzminSample
{
    public class Startup
    {
        IScheduler _scheduler = new StdSchedulerFactory().GetScheduler().Result;
        public const string _routePrefix = "/something";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<Quartzmin.Services>(provider =>
            {
                var isEmpty = string.IsNullOrEmpty(_routePrefix);

                var options = new QuartzminOptions
                {
                    Scheduler = _scheduler,
                    VirtualPathRoot = isEmpty ? string.Empty : _routePrefix
                };

                return Quartzmin.Services.Create(options);
            });
            services.AddScoped<QuartzminFilter>();
            services.AddControllers(opts => opts.Conventions.Add(new QuartzminConvention()))
                .AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseQuartzmin(_routePrefix);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
