namespace Dianper.FeatureToggle.Extensions
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Primitives;

    public static class HttpContextAccessorExtensions
    {
        public static bool GetFromCookies(this IHttpContextAccessor httpContextAccessor, string featureName)
        {
            var cookies = httpContextAccessor?.HttpContext?.Request?.Cookies;

            if (cookies != null && cookies.TryGetValue(featureName, out var cookieValue) && !string.IsNullOrEmpty(cookieValue) && bool.TryParse(cookieValue, out var result))
            {
                return result;
            }

            return false;
        }

        public static bool GetFromHeaders(this IHttpContextAccessor httpContextAccessor, string featureName)
        {
            var headers = httpContextAccessor?.HttpContext?.Request?.Headers;

            if (headers != null && headers.TryGetValue(featureName, out var headersValue) && !StringValues.IsNullOrEmpty(headersValue) && bool.TryParse(headersValue[0], out var result))
            {
                return result;
            }

            return false;
        }

        public static bool GetFromQueryString(this IHttpContextAccessor httpContextAccessor, string featureName)
        {
            var query = httpContextAccessor?.HttpContext?.Request?.Query;

            if (query != null && query.TryGetValue(featureName, out var queryStringValue) && !StringValues.IsNullOrEmpty(queryStringValue) && bool.TryParse(queryStringValue[0], out var result))
            {
                return result;
            }

            return false;
        }
    }
}
