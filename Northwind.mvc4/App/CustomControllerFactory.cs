using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using ASPNET.Controllers;
using ASPNET.Models;

namespace AppCore
{
    public class CustomControllerFactory : IControllerFactory
    {
        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            //customer implementation of controller
            if (controllerName.ToLower().StartsWith("store"))
            {
                var service = new ProductModel();
                var controller = new StoreController(service);
                return controller;
            }

            //default implementation of controller
            var defaultFactory = new DefaultControllerFactory();
            return defaultFactory.CreateController(requestContext, controllerName);
        }

        public SessionStateBehavior GetControllerSessionBehavior(RequestContext requestContext, string controllerName)
        {
            //default implementation of session
            return SessionStateBehavior.Default;
        }

        public void ReleaseController(IController controller)
        {
            //default implentation of dispose controller
            var disposable = controller as IDisposable;
            if(disposable != null)
                disposable.Dispose();
        }
    }
}