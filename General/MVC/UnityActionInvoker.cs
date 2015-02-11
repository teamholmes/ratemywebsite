using Microsoft.Practices.Unity;
using System.Web.Mvc;

namespace OP.General.MVC
{
    public class UnityActionInvoker : ControllerActionInvoker
    {
        IUnityContainer _container;

        public UnityActionInvoker(IUnityContainer container)
        {
            _container = container;
        }

        protected override ActionExecutedContext InvokeActionMethodWithFilters(ControllerContext controllerContext, System.Collections.Generic.IList<System.Web.Mvc.IActionFilter> filters, System.Web.Mvc.ActionDescriptor actionDescriptor, System.Collections.Generic.IDictionary<string, object> parameters)
        {
            foreach (var filter in filters)
            {
                _container.BuildUp(filter.GetType(), filter);
            }

            return base.InvokeActionMethodWithFilters(controllerContext, filters, actionDescriptor, parameters);
        }
    }
}
