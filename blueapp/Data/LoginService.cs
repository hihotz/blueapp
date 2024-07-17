using blackapi.Models;
using blueapp.Models;
using blueapp.Resources.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace blueapp.Data
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly string _loginEndpoint;
        private readonly string _signupEndpoint;
        private readonly string _deleteidEndpoint;
        private readonly string _changepwEndpoint;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // 타임아웃 5초
            _httpClient.Timeout = TimeSpan.FromSeconds(5);

            var (baseUrl, loginEndpoint, signupEndpoint, DeleteIDEndpoint, ChangePWEndpoint) = ApiConfigManager_User.LoadApiConfig();
            _loginEndpoint = $"{baseUrl}{loginEndpoint}";
            _signupEndpoint = $"{baseUrl}{signupEndpoint}";
            _deleteidEndpoint = $"{baseUrl}{DeleteIDEndpoint}";
            _changepwEndpoint = $"{baseUrl}{ChangePWEndpoint}";
        }

        // 로그인 로직
        public async Task<ApiResponse> LoginAsync(LoginModel loginmodel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_loginEndpoint, loginmodel);

                var responseContent = await response.Content.ReadAsStringAsync();
                // JSON 문자열의 이스케이프 문자 제거
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                return apiResponse ?? new ApiResponse { StatusCode = 0, Message = AppResources.error}; // null인 경우 빈 리스트 반환
            }
            catch (TaskCanceledException ex)
            {
                // 타임아웃 처리
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
            catch (HttpRequestException ex) when (ex.InnerException is SocketException)
            {
                // 인터넷 연결 문제 처리
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
            catch (HttpRequestException ex)
            {
                // 다른 HTTP 요청 관련 예외 처리
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
            catch (Exception ex)
            {
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
        }
    }
}
