using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Xml.Linq;

namespace blueapp.Data
{
    public static class ApiConfigManager
    {
        public static (string BaseUrl, string LoginEndpoint, string RegisterEndpoint) LoadApiConfig()
        {
            var assembly = typeof(ApiConfigManager).GetTypeInfo().Assembly;
            var resourceName = "blueapp.Resources.Config.ApiConfig.xml";

            using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("ApiConfig.xml 리소스를 찾을 수 없습니다.");
            using var reader = new StreamReader(stream);
            var doc = XDocument.Load(reader);
            var apiSettings = doc.Descendants("ApiSettings").First();

            var baseUrl = apiSettings.Element("BaseUrl")?.Value ?? string.Empty;
            var loginEndpoint = apiSettings.Element("LoginEndpoint")?.Value ?? string.Empty;
            var RegisterEndpoint = apiSettings.Element("RegisterEndpoint")?.Value ?? string.Empty;

            return (baseUrl, loginEndpoint, RegisterEndpoint);
        }
    }
}
