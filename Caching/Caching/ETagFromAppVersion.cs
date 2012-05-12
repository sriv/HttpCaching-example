using System.Net;
using System.Web.Mvc;

namespace Caching.Caching
{
    public class ETagFromAppVersion: FilterAttribute, IActionFilter
    {
        private static readonly string AppVersion = typeof(ETagFromAppVersion).Assembly.GetName().Version.ToString();

        public void OnActionExecuting(ActionExecutingContext filterContext)
        { }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.GetETag() == AppVersion)
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.NotModified);
            }
            else
            {
                filterContext.SetETagWithCacheability(AppVersion);
            }
        }
    }
}