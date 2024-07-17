using blueapp.Data;
using blueapp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace blueapp.Service
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
            // 타임아웃 5초
            _httpClient.Timeout = TimeSpan.FromSeconds(5);

            var (baseUrl, getListEndpoint, getItemEndpoint, addItemEndpoint, updateItemEndpoint, deleteItemEndpoint) = ApiConfigManager_Production.LoadApiConfig();

            _getListEndpoint = $"{baseUrl}{getListEndpoint}";
            _getItemEndpoint = $"{baseUrl}{getItemEndpoint}/";
            _addItemEndpoint = $"{baseUrl}{addItemEndpoint}";
            _updateItemEndpoint = $"{baseUrl}{updateItemEndpoint}/";
            _deleteItemEndpoint = $"{baseUrl}{deleteItemEndpoint}/";
        }

        // 제품 리스트 불러오기
        public async Task<List<Product_ProductionModel>> GetListAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(_getListEndpoint);
                response.EnsureSuccessStatusCode();
                var productions = await response.Content.ReadFromJsonAsync<List<Product_ProductionModel>>();
                return productions ?? new List<Product_ProductionModel>(); // null인 경우 빈 리스트 반환
            }
            catch (TaskCanceledException ex)
            {
                // 타임아웃 처리
                Console.WriteLine("Request timed out." + ex.Message);
                return new List<Product_ProductionModel>(); // 타임아웃 발생 시 빈 리스트 반환
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                // 인터넷 연결 문제 처리
                Console.WriteLine("No internet connection.");
                return new List<Product_ProductionModel>(); // 인터넷 연결 문제 발생 시 빈 리스트 반환
            }
            catch (HttpRequestException ex)
            {
                // 다른 HTTP 요청 관련 예외 처리
                Console.WriteLine("An error occurred: " + ex.Message);
                return new List<Product_ProductionModel>(); // 오류 발생 시 빈 리스트 반환
            }

        }

        // 제품 리스트 불러오기 - id 값 일치하는 제품
        public async Task<Product_ProductionModel> GetItemAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync(_getItemEndpoint + id);
                response.EnsureSuccessStatusCode();
                var productions = await response.Content.ReadFromJsonAsync<Product_ProductionModel>();
                return productions ?? new Product_ProductionModel(); // null인 경우 빈 리스트 반환
            }
            catch (TaskCanceledException ex)
            {
                // 타임아웃 처리
                Console.WriteLine("Request timed out." + ex.Message);
                return new Product_ProductionModel(); // 타임아웃 발생 시 빈 모델 반환
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                // 인터넷 연결 문제 처리
                Console.WriteLine("No internet connection.");
                return new Product_ProductionModel(); // 인터넷 연결 문제 발생 시 빈 모델 반환
            }
            catch (HttpRequestException ex)
            {
                // 다른 HTTP 요청 관련 예외 처리
                Console.WriteLine("An error occurred: " + ex.Message);
                return new Product_ProductionModel(); // 오류 발생 시 빈 모델 반환
            }

        }

        // 제품 추가
        public async Task<bool> AddItemAsync(Product_ProductionModel production)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_addItemEndpoint, production);
                return response.IsSuccessStatusCode;
            }
            catch (TaskCanceledException ex)
            {
                // 타임아웃 처리
                Console.WriteLine("Request timed out." + ex.Message);
                return false; // 타임아웃 발생 시 false 반환
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                // 인터넷 연결 문제 처리
                Console.WriteLine("No internet connection.");
                return false; // 인터넷 연결 문제 발생 시 false 반환
            }
            catch (HttpRequestException ex)
            {
                // 다른 HTTP 요청 관련 예외 처리
                Console.WriteLine("An error occurred: " + ex.Message);
                return false; // 오류 발생 시 false 반환
            }
        }

        // 제품 수정
        public async Task<bool> UpdateItemAsync(Product_ProductionModel production)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync(_updateItemEndpoint + production.Id, production);
                return response.IsSuccessStatusCode;
            }
            catch (TaskCanceledException ex)
            {
                // 타임아웃 처리
                Console.WriteLine("Request timed out." + ex.Message);
                return false; // 타임아웃 발생 시 false 반환
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                // 인터넷 연결 문제 처리
                Console.WriteLine("No internet connection.");
                return false; // 인터넷 연결 문제 발생 시 false 반환
            }
            catch (HttpRequestException ex)
            {
                // 다른 HTTP 요청 관련 예외 처리
                Console.WriteLine("An error occurred: " + ex.Message);
                return false; // 오류 발생 시 false 반환
            }

        }

        // 제품 삭제
        public async Task<bool> DeleteItemAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(_deleteItemEndpoint + id);
                return response.IsSuccessStatusCode;
            }
            catch (TaskCanceledException ex)
            {
                // 타임아웃 처리
                Console.WriteLine("Request timed out." + ex.Message);
                return false; // 타임아웃 발생 시 false 반환
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                // 인터넷 연결 문제 처리
                Console.WriteLine("No internet connection.");
                return false; // 인터넷 연결 문제 발생 시 false 반환
            }
            catch (HttpRequestException ex)
            {
                // 다른 HTTP 요청 관련 예외 처리
                Console.WriteLine("An error occurred: " + ex.Message);
                return false; // 오류 발생 시 false 반환
            }
        }
    }
}
