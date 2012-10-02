using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// in Global.asax.vb Application_Start you can insert these into the ViewEngine chain like so:
//
// ViewEngines.Engines.Insert(0, new MobileCapableRazorViewEngine())
//
// or
//
// ViewEngines.Engines.Insert(0, new MobileCapableRazorViewEngine("iPhone")
// {
//     ContextCondition = (ctx => ctx.Request.UserAgent.IndexOf(
//         "iPhone", StringComparison.OrdinalIgnoreCase) >= 0)
// });

namespace MobileViewEngines.MVC3
{
    public class MobileCapableWebFormViewEngine : WebFormViewEngine
    {
        public string ViewModifier { get; set; }

        public Func<HttpContextBase, bool> ContextCondition { get; set; }

        public MobileCapableWebFormViewEngine()
            : this("Mobile", context => context.Request.Browser.IsMobileDevice)
        {
        }

        public MobileCapableWebFormViewEngine(string viewModifier)
            : this(viewModifier, context => context.Request.Browser.IsMobileDevice)
        {
        }

        public MobileCapableWebFormViewEngine(string viewModifier, Func<HttpContextBase, bool> contextCondition)
        {
            this.ViewModifier = viewModifier;
            this.ContextCondition = contextCondition;
        }

        public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            return NewFindView(controllerContext, viewName, null, useCache, false);
        }

        public override ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache)
        {
            return NewFindView(controllerContext, partialViewName, null, useCache, true);
        }

        private ViewEngineResult NewFindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache, bool isPartialView)
        {
            if (!CheckMobileAndCookie(controllerContext.HttpContext))
            {
                // we found nothing and we pretend we looked nowhere
                return new ViewEngineResult(new string[] { });
            }

            // Get the name of the controller from the path
            var controller = controllerContext.RouteData.Values["controller"].ToString();
            var area = "";
            try
            {
                area = controllerContext.RouteData.DataTokens["area"].ToString();
            }
            catch
            {
            }

            // Apply the view modifier
            var newViewName = string.Format("{0}.{1}", viewName, ViewModifier);

            // Create the key for caching purposes          
            var keyPath = Path.Combine(area, controller, newViewName);

            var cacheLocation = ViewLocationCache.GetViewLocation(controllerContext.HttpContext, keyPath);

            // Try the cache          
            if (useCache)
            {
                //If using the cache, check to see if the location is cached.                              
                if (!string.IsNullOrWhiteSpace(cacheLocation))
                {
                    return isPartialView ? new ViewEngineResult(this.CreatePartialView(controllerContext, cacheLocation), this) : new ViewEngineResult(this.CreateView(controllerContext, cacheLocation, masterName), this);
                }
            }
            var locationFormats = string.IsNullOrEmpty(area) ? ViewLocationFormats : AreaViewLocationFormats;

            // for each of the paths defined, format the string and see if that path exists. When found, cache it.          
            foreach (var currentPath in locationFormats.Select(rootPath => string.IsNullOrEmpty(area) ? string.Format(rootPath, newViewName, controller) : string.Format(rootPath, newViewName, controller, area)).Where(currentPath => this.FileExists(controllerContext, currentPath)))
            {
                this.ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, keyPath, currentPath);

                return isPartialView ? new ViewEngineResult(this.CreatePartialView(controllerContext, currentPath), this) : new ViewEngineResult(this.CreateView(controllerContext, currentPath, masterName), this);
            }
            return new ViewEngineResult(new string[] { });
            // we found nothing and we pretend we looked nowhere
        }

        private bool CheckMobileAndCookie(HttpContextBase context)
        {
            if (ContextCondition(context))
            {
                return context.GetOverriddenBrowser().IsMobileDevice;
            }
            return false;
        }
    }
}