namespace Dianper.FeatureToggle.Extensions
{
    using Microsoft.AspNetCore.Http;

    public static class HttpContextAccessorExtensions
    {
        public static bool GetFromCookies(this IHttpContextAccessor httpContextAccessor, string featureName)
        {
            var cookies = httpContextAccessor.HttpContext?.Request?.Cookies;

            if (cookies.TryGetValue(featureName, out var cookieValue) && !string.IsNullOrWhiteSpace(cookieValue) && bool.TryParse(cookieValue, out var result))
            {
                return result;
            }

            return false;
        }

        public static bool GetFromHeaders(this IHttpContextAccessor httpContextAccessor, string featureName)
        {
            var headers = httpContextAccessor.HttpContext?.Request?.Headers;

            if (headers.TryGetValue(featureName, out var headersValue) && !string.IsNullOrWhiteSpace(headersValue) && bool.TryParse(headersValue, out var result))
            {
                return result;
            }

            return false;
        }

        public static bool GetFromQueryString(this IHttpContextAccessor httpContextAccessor, string featureName)
        {
            var query = httpContextAccessor.HttpContext?.Request?.Query;

            if (query.TryGetValue(featureName, out var queryStringValue) && !string.IsNullOrWhiteSpace(queryStringValue) && bool.TryParse(queryStringValue, out var result))
            {
                return result;
            }

            return false;
        }
    }
}
