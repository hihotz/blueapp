using blueapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace blueapp.Data
{
    public class ProductionService
    {
        private readonly HttpClient _httpClient;
        private readonly string _getListEndpoint;
        private readonly string _getItemEndpoint;
        private readonly string _addItemEndpoint;
        private readonly string _updateItemEndpoint;
        private readonly string _deleteItemEndpoint;

        public ProductionService(HttpClient httpClient)
        {
            _httpClient = httpClient;

            var (baseUrl, getListEndpoint, getItemEndpoint, addItemEndpoint, updateItemEndpoint, deleteItemEndpoint) = ApiConfigManager_Production.LoadApiConfig();

            _getListEndpoint = $"{baseUrl}{getListEndpoint}";
            _getItemEndpoint = $"{baseUrl}{getItemEndpoint}/";
            _addItemEndpoint = $"{baseUrl}{addItemEndpoint}";
            _updateItemEndpoint = $"{baseUrl}{updateItemEndpoint}/";
            _deleteItemEndpoint = $"{baseUrl}{deleteItemEndpoint}/";
        }

        public async Task<List<ProductionModel>> GetListAsync()
        {
            var response = await _httpClient.GetAsync(_getListEndpoint);
            response.EnsureSuccessStatusCode();
            var productions = await response.Content.ReadFromJsonAsync<List<ProductionModel>>();
            return productions ?? new List<ProductionModel>(); // null인 경우 빈 리스트 반환
        }

        public async Task<ProductionModel> GetItemAsync(int id)
        {
            var response = await _httpClient.GetAsync(_getItemEndpoint + id);
            response.EnsureSuccessStatusCode();
            var productions = await response.Content.ReadFromJsonAsync<ProductionModel>();
            return productions ?? new ProductionModel(); // null인 경우 빈 리스트 반환
        }

        public async Task<bool> AddItemAsync(ProductionModel production)
        {
            var response = await _httpClient.PostAsJsonAsync(_addItemEndpoint, production);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateItemAsync(ProductionModel production)
        {
            var response = await _httpClient.PutAsJsonAsync(_updateItemEndpoint + production.Id, production);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var response = await _httpClient.DeleteAsync(_deleteItemEndpoint + id);
            return response.IsSuccessStatusCode;
        }
    }
}
