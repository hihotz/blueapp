using blackapi.Models;
using blueapp.Data;
using blueapp.Models;
using blueapp.Resources.Localization;
using System.Net.Http.Json;
using System.Net.Sockets;
using System.Text.Json;

namespace blueapp.Service
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;
        private readonly string _loginEndpoint;
        private readonly string _registerEndpoint;
        private readonly string _deleteidEndpoint;
        private readonly string _changepwEndpoint;
        private readonly JsonSerializerOptions options;

        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // 타임아웃 5초
            _httpClient.Timeout = TimeSpan.FromSeconds(5);

            var (baseUrl, loginEndpoint, registerEndpoint, DeleteIDEndpoint, ChangePWEndpoint) = ApiConfigManager_User.LoadApiConfig();
            _loginEndpoint = $"{baseUrl}{loginEndpoint}";
            _registerEndpoint = $"{baseUrl}{registerEndpoint}";
            _deleteidEndpoint = $"{baseUrl}{DeleteIDEndpoint}";
            _changepwEndpoint = $"{baseUrl}{ChangePWEndpoint}";

            // JSON 문자열의 이스케이프 문자 제거를 위한 코드
            options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        // 로그인 로직
        public async Task<ApiResponse> LoginAsync(User_LoginModel loginmodel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_loginEndpoint, loginmodel);
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                return apiResponse ?? new ApiResponse { StatusCode = 0, Message = AppResources.error }; // null인 경우 빈 리스트 반환
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

        // 회원가입 로직
        public async Task<ApiResponse> RegisterAsync(User_RegisterModel registermodel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_registerEndpoint, registermodel);
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                return apiResponse ?? new ApiResponse { StatusCode = 0, Message = AppResources.error }; // null인 경우 빈 리스트 반환
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

        // 회원탈퇴 로직
        public async Task<ApiResponse> DeleteIDAsync(User_DeleteID deleteidmodel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_deleteidEndpoint, deleteidmodel);
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                return apiResponse ?? new ApiResponse { StatusCode = 0, Message = AppResources.error }; // null인 경우 빈 리스트 반환
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
        
        // 비밀번호 변경 로직
        public async Task<ApiResponse> ChangePWAsync(User_ChangePW changepwmodel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_changepwEndpoint, changepwmodel);
                var responseContent = await response.Content.ReadAsStringAsync();
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                return apiResponse ?? new ApiResponse { StatusCode = 0, Message = AppResources.error }; // null인 경우 빈 리스트 반환
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
