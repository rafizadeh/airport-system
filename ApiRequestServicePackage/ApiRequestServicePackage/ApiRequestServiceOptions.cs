using System.Security.Cryptography.X509Certificates;

namespace ApiRequestServicePackage
{
    public class ApiRequestServiceOptions
    {
        public ApiRequestServiceOptions()
        {
            Headers = new Dictionary<string, string>();
            X509Certificate2s = new List<X509Certificate2>();
        }

        #region HttpClient options
        public Uri BaseAddress { get; set; }
        public Dictionary<string, string> Headers { get; }
        public void AddHeader(string key, string value)
        {
            if (!Headers.TryAdd(key, value)) Headers[key] = value;
        }
        #endregion

        #region HttpClientHandler options
        public List<X509Certificate2> X509Certificate2s { get; }
        public bool CheckServerCertificate { get; private set; }
        public void AddCertificate(X509Certificate2 x509Certificate2) => X509Certificate2s.Add(x509Certificate2);
        #endregion
    }
}