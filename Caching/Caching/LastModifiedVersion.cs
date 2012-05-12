using System;
using System.Globalization;
using System.Net;
using System.Web.Mvc;
using Caching.Models;

namespace Caching.Caching
{
    public class LastModifiedVersion : FilterAttribute, IActionFilter
    {
        private const long None = -1;

        public void OnActionExecuting(ActionExecutingContext filterContext)
        { }

        public void OnActionExecuted(ActionExecutedContext ctx)
        {
            var result = ctx.Result as ViewResultBase;
            if (result == null)
                return;

            var versionable = result.Model as ITimeVersionable;
            if (versionable == null)
                return;

            var lastModifiedSent = GetSentVersionOfEntity(ctx);

            if (lastModifiedSent != null &&  lastModifiedSent.Value == versionable.LastModified)
            {
                ctx.Result = new HttpStatusCodeResult((int)HttpStatusCode.NotModified);
            }
            else
            {
                ctx.SetLastModifiedWithCacheability(versionable.LastModified);
            }
        }

        private static DateTime? GetSentVersionOfEntity(ControllerContext filterContext)
        {
            return filterContext.GetIfModifiedSince();
        }
    }
}