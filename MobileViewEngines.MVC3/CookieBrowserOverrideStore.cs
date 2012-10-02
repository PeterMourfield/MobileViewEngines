using System;
using System.Linq;
using System.Web;

namespace MobileViewEngines.MVC3
{
    public class CookieBrowserOverrideStore
    {
        static internal readonly string BrowserOverrideCookieName = ".ASPXBrowserOverride";
        private readonly int daysToExpire;

        public CookieBrowserOverrideStore()
            : this(1)
        {
        }

        public CookieBrowserOverrideStore(int daysToExpire)
        {
            this.daysToExpire = daysToExpire;
        }

        public string GetOverriddenUserAgent(HttpContextBase httpContext)
        {
            if (httpContext.Response.Cookies.AllKeys.Contains(BrowserOverrideCookieName))
            {
                var cookie = httpContext.Response.Cookies[BrowserOverrideCookieName];
                return cookie != null ? cookie.Value : null;
            }
            var httpCookie = httpContext.Request.Cookies[BrowserOverrideCookieName];
            return httpCookie != null ? httpCookie.Value : null;
        }

        public void SetOverriddenUserAgent(HttpContextBase httpContext, string userAgent)
        {
            var cookie = new HttpCookie(BrowserOverrideCookieName, HttpUtility.UrlEncode(userAgent));
            if ((userAgent == null))
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
            }
            else if ((this.daysToExpire > 0))
            {
                cookie.Expires = DateTime.Now.AddDays(Convert.ToDouble(this.daysToExpire));
            }
            httpContext.Response.Cookies.Remove(BrowserOverrideCookieName);
            httpContext.Response.Cookies.Add(cookie);
        }
    }
}
