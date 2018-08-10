using PilotSmithApp.UserInterface.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace PilotSmithApp.UserInterface
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            MappingConfig.RegisterMaps();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Enabling Bundling and Minification
            BundleTable.EnableOptimizations = false;
            var sessionStateModuleType = typeof(SessionStateModule);
            var pollingIntervalFieldInfo = sessionStateModuleType.GetField("LOCKED_ITEM_POLLING_INTERVAL", BindingFlags.NonPublic | BindingFlags.Static);
            pollingIntervalFieldInfo.SetValue(null, 30); // default 500ms
            var pollingDeltaFieldInfo = sessionStateModuleType.GetField("LOCKED_ITEM_POLLING_DELTA", BindingFlags.NonPublic | BindingFlags.Static);
            pollingDeltaFieldInfo.SetValue(null, TimeSpan.FromMilliseconds(15.0)); // default 250ms
        }
    }
}
