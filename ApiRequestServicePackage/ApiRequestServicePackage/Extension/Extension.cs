using System.Web;

namespace ApiRequestServicePackage.Extension
{
    internal static class Extension
    {
        public static string ToQueryString<TRequestParameter>(this TRequestParameter parameter) where TRequestParameter : new()
        {
            if (parameter is null)
                return string.Empty;

            IEnumerable<string> parameters = typeof(TRequestParameter).GetProperties().Where(p => p.GetValue(parameter) is not null)
                                                                         .Select(p => $"{p.Name}={HttpUtility.UrlEncode(p.GetValue(parameter)?.ToString())}");

            return $"?{string.Join('&', parameters)}";
        }
    }
}