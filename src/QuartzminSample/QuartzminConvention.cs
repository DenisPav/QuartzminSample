using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Linq;

namespace QuartzminSample
{
    public class QuartzminConvention : IApplicationModelConvention
    {
        public void Apply(ApplicationModel application)
        {
            var quartzMinControllers = application.Controllers
                .Where(model => model.ControllerType.FullName.Contains(nameof(Quartzmin)))
                .ToList();

            quartzMinControllers.ForEach(model => application.Controllers.Remove(model));

            var newControllerModels = quartzMinControllers.Select(CreateFrom)
                .ToList();

            newControllerModels.ForEach(application.Controllers.Add);
        }

        private ControllerModel CreateFrom(ControllerModel controllerModel)
        {
            var newModel = new ControllerModel(controllerModel.ControllerType, Array.Empty<object>());
            var isRouteEmpty = string.IsNullOrEmpty(Startup._routePrefix);

            controllerModel.Actions.ToList().ForEach(newModel.Actions.Add);
            newModel.ApiExplorer = controllerModel.ApiExplorer;
            newModel.Application = controllerModel.Application;
            newModel.ControllerName = controllerModel.ControllerName;
            controllerModel.ControllerProperties.ToList().ForEach(newModel.ControllerProperties.Add);
            controllerModel.Selectors.ToList().ForEach(newModel.Selectors.Add);

            if (!isRouteEmpty)
            {
                newModel.Selectors.ToList().ForEach(selector =>
                {
                    selector.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute($"{Startup._routePrefix}/{{controller=Scheduler}}/{{action=Index}}"));
                });
            }

            newModel.Filters.Add(new ServiceFilterAttribute(typeof(QuartzminFilter)));
            return newModel;
        }
    }
}
