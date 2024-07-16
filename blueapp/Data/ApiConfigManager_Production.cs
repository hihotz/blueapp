using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace blueapp.Data
{
    public static class ApiConfigManager_Production
    {
        public static (string BaseUrl, string GetListEndpoint, string GetItemEndpoint, string AddItemEndpoint, string UpdateItemEndpoint, string DeleteItemEndpoint) LoadApiConfig()
        {
            var assembly = typeof(ApiConfigManager_Production).GetTypeInfo().Assembly;
            var resourceName = "blueapp.Resources.Config.ApiConfig.xml";

            using var stream = assembly.GetManifestResourceStream(resourceName) ?? throw new InvalidOperationException("ApiConfig.xml 리소스를 찾을 수 없습니다.");
            using var reader = new StreamReader(stream);
            var doc = XDocument.Load(reader);

            var apiSettings = doc.Descendants("ApiSettings").First();
            var production = doc.Descendants("Production").First();

            #region baseUrl
            var baseUrl = apiSettings.Element("BaseUrl")?.Value ?? string.Empty;
            #endregion

            #region production
            var GetListEndpoint = production.Element("GetListEndpoint")?.Value ?? string.Empty;
            var GetItemEndpoint = production.Element("GetItemEndpoint")?.Value ?? string.Empty;
            var AddItemEndpoint = production.Element("AddItemEndpoint")?.Value ?? string.Empty;
            var UpdateItemEndpoint = production.Element("UpdateItemEndpoint")?.Value ?? string.Empty;
            var DeleteItemEndpoint = production.Element("DeleteItemEndpoint")?.Value ?? string.Empty;
            #endregion

            return (baseUrl, GetListEndpoint, GetItemEndpoint, AddItemEndpoint, UpdateItemEndpoint, DeleteItemEndpoint);
        }
    }
}