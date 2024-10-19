using System.Text;
using Newtonsoft.Json;
using ApiRequestServicePackage.Extension;
using System.Security.Cryptography.X509Certificates;

namespace ApiRequestServicePackage
{
    public class ApiRequestService : IApiRequestService
    {
        private readonly ApiRequestServiceOptions _options;

        public ApiRequestService(ApiRequestServiceOptions options) => _options = options ?? new ApiRequestServiceOptions();

        public ApiRequestService Configure(Action<ApiRequestServiceOptions> apiOptions)
        {
            apiOptions?.Invoke(_options);

            return this;
        }

        public async Task<TResponse> ApiRequest<TResponse>(Func<HttpClient, Task<TResponse>> func, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            //Log.Information("------------------------------ API Request is started ------------------------------");

            using HttpClientHandler httpClientHandler = new();

            apiOptions?.Invoke(_options);

            if (!_options.CheckServerCertificate)
                httpClientHandler.ServerCertificateCustomValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            foreach (X509Certificate2 x509Certificate2 in _options.X509Certificate2s)
                httpClientHandler.ClientCertificates.Add(x509Certificate2);

            using HttpClient httpClient = new(httpClientHandler);

            httpClient.BaseAddress = _options.BaseAddress;

            foreach (var header in _options.Headers)
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);

            try
            {
                TResponse response = await func(httpClient);

                //Log.Information("------------------------------ API Request has been finished ------------------------------");

                return response;
            }
            catch (Exception exception)
            {
                //exception.LogException();

                //Log.Information("------------------------------ API Request has been failed ------------------------------");

                return default;
            }
        }

        public async Task<TResponse> GetAsync<TResponse>(string url, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                using HttpResponseMessage response = await httpClient.GetAsync(url);
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            }, apiOptions);
        }

        public async Task<TResponse> GetAsync<TResponse>(string url, object body, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Content = new StringContent(JsonConvert.SerializeObject(body))
                };

                using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                return JsonConvert.DeserializeObject<TResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
            }, apiOptions);
        }

        public async Task<TResponse> GetAsync<TResponse>(string url, string body, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                HttpRequestMessage httpRequestMessage = new()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(url),
                    Content = new StringContent(body)
                };

                using HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                return JsonConvert.DeserializeObject<TResponse>(await httpResponseMessage.Content.ReadAsStringAsync());
            }, apiOptions);
        }

        public async Task<TResponse> GetAsync<TRequestParameter, TResponse>(string url, TRequestParameter parameter = default, Action<ApiRequestServiceOptions> apiOptions = default) where TRequestParameter : new()
        {
            return await ApiRequest(async (httpClient) =>
            {
                using HttpResponseMessage response = await httpClient.GetAsync($"{url}{parameter.ToQueryString()}");
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync());
            }, apiOptions);
        }

        public async Task<byte[]> GetByteArrayAsync(string url, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                return await httpClient.GetByteArrayAsync(url);
            }, apiOptions);
        }

        public async Task<TResponse> PostAsync<TResponse>(string url, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                using HttpResponseMessage response = await httpClient.PostAsync(url, null);
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                });
            }, apiOptions);
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                using HttpResponseMessage response = await httpClient.PostAsync(url, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
                });
            }, apiOptions);
        }

        public async Task<Stream> PostAsync(string url, Stream stream, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                using HttpResponseMessage response = await httpClient.PostAsync(url, new StreamContent(stream));
                return await response.Content.ReadAsStreamAsync();
            }, apiOptions);
        }

        public async Task<TResponse> PutAsync<TResponse>(string url, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                using HttpResponseMessage response = await httpClient.PutAsync(url, null);
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                });
            }, apiOptions);
        }

        public async Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest request, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                using HttpResponseMessage response = await httpClient.PutAsync(url, new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                });
            }, apiOptions);
        }

        public async Task<TResponse> DeleteAsync<TResponse>(string url, Action<ApiRequestServiceOptions> apiOptions = default)
        {
            return await ApiRequest(async (httpClient) =>
            {
                using HttpResponseMessage response = await httpClient.DeleteAsync(url);
                return JsonConvert.DeserializeObject<TResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerSettings
                {
                    ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor,
                });
            }, apiOptions);
        }
    }
}