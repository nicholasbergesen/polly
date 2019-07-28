using System.Web;
using System.Web.Mvc;

namespace Polly.Website
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString RenderWhenTrue(this HtmlHelper htmlHelper, bool condition, string positive, string negative = null)
        {
            return new HtmlString(condition ? positive : negative);
        }
    }
}