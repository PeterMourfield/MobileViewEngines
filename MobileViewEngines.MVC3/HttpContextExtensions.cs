using MobileViewEngines.MVC3;

namespace System.Web
{
    public enum BrowserOverride
    {
        Desktop = 0,
        Mobile = 1
    }

    public static class HttpContextExtensions
    {
        public static void ClearOverriddenBrowser(this HttpContextBase httpContext)
        {
            httpContext.SetOverriddenBrowser(null);
        }

        private static HttpBrowserCapabilitiesBase CreateOverriddenBrowser(string userAgent)
        {
            return new HttpBrowserCapabilitiesWrapper(new HttpContext(new UserAgentWorkerRequest(userAgent)).Request.Browser);
        }

        public static HttpBrowserCapabilitiesBase GetOverriddenBrowser(this HttpContextBase httpContext)
        {
            return httpContext.GetOverriddenBrowser(CreateOverriddenBrowser);
        }

        static internal HttpBrowserCapabilitiesBase GetOverriddenBrowser(this HttpContextBase httpContext, Func<string, HttpBrowserCapabilitiesBase> createBrowser)
        {
            var browser = (HttpBrowserCapabilitiesBase)httpContext.Items[BrowserHelpers.BrowserOverrideKey];
            if (browser == null)
            {
                var overriddenUserAgent = httpContext.GetOverriddenUserAgent();
                if (string.IsNullOrEmpty(overriddenUserAgent) || string.Equals(overriddenUserAgent, httpContext.Request.UserAgent))
                {
                    browser = httpContext.Request.Browser;
                }
                else
                {
                    browser = createBrowser.Invoke(overriddenUserAgent);
                }
                httpContext.Items[BrowserHelpers.BrowserOverrideKey] = browser;
            }
            return browser;
        }

        public static string GetOverriddenUserAgent(this HttpContextBase httpContext)
        {
            var result = httpContext.Request.Cookies[CookieBrowserOverrideStore.BrowserOverrideCookieName];
            if (result != null)
            {
                return result.Value;
            }
            return null;
        }

        public static void SetOverriddenBrowser(this HttpContextBase httpContext, string userAgent)
        {
            httpContext.Items[BrowserHelpers.UserAgentKey] = userAgent;
            httpContext.Items[BrowserHelpers.BrowserOverrideKey] = null;
            BrowserHelpers.BrowserOverrideStore.SetOverriddenUserAgent(httpContext, userAgent);
        }

        public static void SetOverriddenBrowser(this HttpContextBase httpContext, BrowserOverride browserOverride)
        {
            string userAgent = null;
            switch (browserOverride)
            {
                case BrowserOverride.Desktop:
                    userAgent = "Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)";
                    break;
                case BrowserOverride.Mobile:
                    userAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows CE; IEMobile 8.12; MSIEMobile 6.0)";
                    break;
            }
            httpContext.SetOverriddenBrowser(userAgent);
        }
    }
}
