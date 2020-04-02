using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace QuartzminSample
{
    public class QuartzminFilter : IAsyncActionFilter
    {
        readonly Quartzmin.Services _services;

        public QuartzminFilter(Quartzmin.Services services) => _services = services;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Items[typeof(Quartzmin.Services)] = _services;
            await next();
        }
    }
}
