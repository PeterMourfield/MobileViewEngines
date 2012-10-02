using System.Web.Mvc;
using MobileViewEngines.MVC3;

[assembly: WebActivator.PreApplicationStartMethod(typeof(MobileViewEngines.MVC3.Razor.Sample.App_Start.MobileViewEnginesBootstrapper), "Start")]
namespace MobileViewEngines.MVC3.Razor.Sample.App_Start
{
    public static class MobileViewEnginesBootstrapper
    {
        public static void Start()
        {
            ViewEngines.Engines.Insert(0, new MobileCapableRazorViewEngine());
        }
    }
}
