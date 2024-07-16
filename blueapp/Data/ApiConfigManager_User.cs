using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Xml.Linq;

namespace blueapp.Data
{
    public static class ApiConfigManager_User
    {
        public static (string BaseUrl, string LoginEndpoint, string RegisterEndpoint, string DeleteIDEndpoint, string ChangePWEndpoint) LoadApiConfig()
        {
            var assembly = typeof(ApiConfigManager_User).GetTypeInfo().Assembly;
            var resourceName = "blueapp.Resources.Config.ApiConfig.xml";

            using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("ApiConfig.xml 리소스를 찾을 수 없습니다.");
            using var reader = new StreamReader(stream);
            var doc = XDocument.Load(reader);

            var apiSettings = doc.Descendants("ApiSettings").First();
            var user = doc.Descendants("User").First();

            #region baseUrl
            var baseUrl = apiSettings.Element("BaseUrl")?.Value ?? string.Empty;
            #endregion

            #region user
            var LoginEndpoint = user.Element("LoginEndpoint")?.Value ?? string.Empty;
            var RegisterEndpoint = user.Element("RegisterEndpoint")?.Value ?? string.Empty;
            var DeleteIDEndpoint = user.Element("DeleteIDEndpoint")?.Value ?? string.Empty;
            var ChangePWEndpoint = user.Element("ChangePWEndpoint")?.Value ?? string.Empty;
            #endregion

            return (baseUrl, LoginEndpoint, RegisterEndpoint, DeleteIDEndpoint, ChangePWEndpoint);
        }
    }
}
