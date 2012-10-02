namespace MobileViewEngines.MVC3
{
    public class BrowserHelpers
    {
        public static readonly object BrowserOverrideKey = new object();
        public const string DesktopUserAgent = "Mozilla/4.0 (compatible; MSIE 6.1; Windows XP)";
        public const string MobileUserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows CE; IEMobile 8.12; MSIEMobile 6.0)";
        public static readonly object UserAgentKey = new object();
        public static readonly CookieBrowserOverrideStore BrowserOverrideStore = new CookieBrowserOverrideStore();
    }
}
