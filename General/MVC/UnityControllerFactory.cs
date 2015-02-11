using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OP.General.MVC
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private IUnityContainer _container;

        public UnityControllerFactory(IUnityContainer container)
        {
            _container = container;
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {

            if (controllerType == null) return null;

            if (controllerType == null) throw new ArgumentNullException();

            var controller1 = (Controller)_container.Resolve(controllerType, null);
            controller1.ActionInvoker = new UnityActionInvoker(_container);

            return controller1;

        }
    }
}