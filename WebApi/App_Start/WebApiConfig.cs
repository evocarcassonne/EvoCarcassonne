using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/xml") ); 
            
            config.Routes.MapHttpRoute(
                name: "PlayerAPI",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }  
            );
            
            /*config.Routes.MapHttpRoute(
                name: "PlayerAPI2",
                routeTemplate: "api/{controller}/{gameID}/{playerName}",
                defaults: new { gameID = RouteParameter.Optional, playerName = RouteParameter.Optional }  
            );*/
            /*config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );*/


        }
    }
}
