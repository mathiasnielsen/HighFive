using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace HighFive.Server.Api.Filters.Data.Cache
{
    public class CacheFilter : ActionFilterAttribute
    {
        private const string ContentTypeKeyParameter = "application/xml";
        private const string CharSet = "utf-8";

        private readonly ICacheImplementation cache;
        private readonly TimeSpan duration;

        public CacheFilter(int durationInMs)
        {
            duration = new TimeSpan(0, 0, 0, 0, durationInMs);

            cache = new MemoryCacheImplementation();
        }

        public int Count { get { return cache.TotalEntries; } }

        /// <summary>
        /// Occurs before the action method is invoked.
        /// If cached key exist, the actionContext.Response is assigned with the cached data.
        /// When actionContext.Response is not null, the OnActionExecuted will not run.
        /// </summary>
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var mainKey = actionContext.Request.RequestUri.ToString();
            var authHeader = actionContext.Request.Headers.Authorization;
            var key = ConstructKeyBasedOnAuthorizationHeader(mainKey, authHeader);

            if (cache.DoesKeyExist(key))
            {
                var value = cache.Get<string>(key);
                var content = new StringContent(value);

                content.Headers.ContentType = GetContentType(key);
                actionContext.Response = actionContext.Request.CreateResponse();
                actionContext.Response.Content = content;
            }
        }

        /// <summary>
        /// Occures after the action method is invoked.
        /// Saves the actionExecutedContext.Response.
        /// </summary>
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception == null)
            {
                var mainKey = actionExecutedContext.Request.RequestUri.ToString();
                var authHeader = actionExecutedContext.Request.Headers.Authorization;
                var key = ConstructKeyBasedOnAuthorizationHeader(mainKey, authHeader);

                var data = string.Empty;
                if (actionExecutedContext.Response != null)
                {
                    var content = actionExecutedContext.Response.Content;
                    data = content.ReadAsStringAsync().Result;
                    cache.Add(ConstructKey(key, ContentTypeKeyParameter), content.Headers.ContentType, duration);
                }

                cache.Add(key, data, duration);
            }
        }

        private MediaTypeHeaderValue GetContentType(string key)
        {
            var contentType = cache.Get<MediaTypeHeaderValue>(ConstructKey(key, ContentTypeKeyParameter));
            if (contentType == null)
            {
                contentType = new MediaTypeHeaderValue(ContentTypeKeyParameter);
                contentType.CharSet = CharSet;
            }

            return contentType;
        }

        private string ConstructKeyBasedOnAuthorizationHeader(string mainKey, AuthenticationHeaderValue authHeader)
        {
            const string WithCredentials = nameof(WithCredentials);
            const string NoCredentials = nameof(NoCredentials);

            if (authHeader != null && string.IsNullOrWhiteSpace(authHeader.ToString()) == false)
            {
                return ConstructKey(mainKey, WithCredentials);
            }

            return ConstructKey(mainKey, NoCredentials);
        }

        private string ConstructKey(string mainKey, params object[] args)
        {
            return mainKey += string.Concat(args);
        }
    }
}
