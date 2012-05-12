using System;
using System.Web;
using System.Web.Mvc;

namespace Caching.Caching
{
    public static class WebExtensions
    {
        public static string GetETag(this ControllerContext ctx)
        {
            return ctx.RequestContext.HttpContext.Request.Headers["If-None-Match"];
        }

        public static DateTime? GetIfModifiedSince(this ControllerContext ctx)
        {
            var modifiedSince = ctx.RequestContext.HttpContext.Request.Headers["If-Modified-Since"];
            DateTime result;
            if (modifiedSince == null || DateTime.TryParse(modifiedSince, out result) == false)
                return null;

            return result;
        }

        public static void SetETagWithCacheability(this ControllerContext ctx, string etag)
        {
            var response = ctx.RequestContext.HttpContext.Response;
            response.Cache.SetETag(etag);
            response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
        }

        public static void SetLastModifiedWithCacheability(this ControllerContext ctx, DateTime lastModified)
        {
            var response = ctx.RequestContext.HttpContext.Response;
            response.Cache.SetLastModified(lastModified);
            response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
        }


        public static void SetAbsoluteExpiration(this ControllerContext ctx, TimeSpan lifeLength)
        {
            var response = ctx.RequestContext.HttpContext.Response;
            response.ExpiresAbsolute = DateTime.Now + lifeLength;
            response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
        }
    }
}