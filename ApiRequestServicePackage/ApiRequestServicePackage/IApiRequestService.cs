namespace ApiRequestServicePackage
{
    public interface IApiRequestService
    {
        public ApiRequestService Configure(Action<ApiRequestServiceOptions> apiOptions);

        public Task<TResponse> ApiRequest<TResponse>(Func<HttpClient, Task<TResponse>> func, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> GetAsync<TResponse>(string url, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> GetAsync<TResponse>(string url, object body, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> GetAsync<TResponse>(string url, string body, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> GetAsync<TRequestParameter, TResponse>(string url, TRequestParameter parameter = default, Action<ApiRequestServiceOptions> apiOptions = default) where TRequestParameter : new();

        public Task<byte[]> GetByteArrayAsync(string url, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> PostAsync<TResponse>(string url, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest request, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<Stream> PostAsync(string url, Stream stream, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> PutAsync<TResponse>(string url, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> PutAsync<TRequest, TResponse>(string url, TRequest request, Action<ApiRequestServiceOptions> apiOptions = default);

        public Task<TResponse> DeleteAsync<TResponse>(string url, Action<ApiRequestServiceOptions> apiOptions = default);
    }
}