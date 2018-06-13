using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using DotNetOpenAuth.OpenId.Extensions.AttributeExchange;
using GlobalWatchSystem.Filters;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace GlobalWatchSystem.Config
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.Routes.MapHttpRoute(name:"AreaAlarms",
                routeTemplate:"api/alarms/area/{areaId}", 
                defaults:new {controller="Alarm", action="GetAlarmsInArea"});
            config.Routes.MapHttpRoute(name: "DeviceStatus",
                routeTemplate: "api/device/status/{ids}",
                defaults: new { controller = "DeviceData", action = "GetDeviceStatus" });
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.Filters.Add(new AuthorizeAttribute());
        }
    }
}