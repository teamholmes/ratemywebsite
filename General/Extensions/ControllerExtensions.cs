using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OP.General.Extensions
{
    public static class ControllerExtensions
    {
        //note: this is a work in progress method to replace mvccontrib strongly typed redirecttoaction


        //public static RedirectToRouteResult RedirectToAction<TController>(this TController controller, Expression<Action<TController>> actionMethodName) where TController : Controller
        //{
        //    Match match = Regex.Match(actionMethodName.Body.ToString(), @"\.(?<MethodName>[^\.]+)\((?<Parameters>.*?)\){1}$");

        //    string controllerName = Regex.Replace(controller.GetType().Name, "Controller$", "", RegexOptions.IgnoreCase);
        //    string actionName = match.Groups["MethodName"].Value;
        //    string methodParams = match.Groups["Parameters"].Value;


        //    return new RedirectToRouteResult(
        //        new RouteValueDictionary(
        //            new
        //            {
        //                controller = controllerName,
        //                action = actionName,
        //            }));
        //}
    }
}
