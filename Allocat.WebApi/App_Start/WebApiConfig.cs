using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Allocat.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
            name: "ApiById",
            routeTemplate: "api/{controller}/{id}",
            defaults: new { id = RouteParameter.Optional },
            constraints: new { id = @"^[0-9]+$" }
        );

            config.Routes.MapHttpRoute(
           name: "ApiByName",
           routeTemplate: "api/{controller}/{action}",
           defaults: null
       );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );



            // config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "TissueBank/api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }
}
