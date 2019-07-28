using System.Web;

namespace Polly.Website
{
    public static class BrowserHelper
    {
        public static bool IsMobile()
        {
            return HttpContext.Current.Request.Browser.IsMobileDevice;
        }

        public static bool IsDesktop()
        {
            return !IsMobile();
        }
    }
}