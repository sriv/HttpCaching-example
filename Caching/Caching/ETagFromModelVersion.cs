using System.Globalization;
using System.Net;
using System.Web.Mvc;
using Caching.Models;

namespace Caching.Caching
{
    public class ETagFromModelVersion : FilterAttribute, IActionFilter
    {
        private const long None = -1;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        { }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var result = filterContext.Result as ViewResultBase;
            if (result == null)
                return;

            var versionable = result.Model as IVersionable;
            if (versionable == null)
                return;

            var versionSent = GetSentVersionOfEntity(filterContext);

            if (versionSent == versionable.Version)
            {
                filterContext.Result = new HttpStatusCodeResult((int)HttpStatusCode.NotModified);
                filterContext.HttpContext.Response.SuppressContent = true;
            }
            else
            {
                filterContext.SetETagWithCacheability(versionable.Version.ToString(CultureInfo.InvariantCulture));
            }
        }

        private static long GetSentVersionOfEntity(ControllerContext filterContext)
        {
            long etag;
            if (long.TryParse(filterContext.GetETag(), out etag))
                return etag;

            return None;
        }
    }
}