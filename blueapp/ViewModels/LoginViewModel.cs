using blackapi.Models;
using blueapp.Data;
using blueapp.Resources.Localization;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace blueapp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private bool _isLoggedIn;
        private string _loginEndpoint;
        private string _signupEndpoint;

        public LoginViewModel()
        {
            var (baseUrl, loginEndpoint, signupEndpoint) = ApiConfigManager.LoadApiConfig();
            _loginEndpoint = $"{baseUrl}{loginEndpoint}";
            _signupEndpoint = $"{baseUrl}{signupEndpoint}";
        }

        #region 로그인 기능
        //로그인 유지
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => SetProperty(ref _isLoggedIn, value);
        }

        // 로그인 로직
        public async Task<ApiResponse> LoginAsync(string name, string pw)
        {
            try
            {
                var loginData = new 
                { 
                    username = name,
                    password = pw
                };

                // 타임아웃 5초
                var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_loginEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                // JSON 문자열의 이스케이프 문자 제거
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                var a = apiResponse?.StatusCode;
                var b = apiResponse?.Message;
                if (apiResponse?.StatusCode == 200 && apiResponse !=null)
                {
                    // 로그인 성공
                    IsLoggedIn = true;
                    return apiResponse;
                }
                else
                {
                    // 로그인 실패, 에러 메시지와 코드를 처리합니다.
                    // apiResponse의 데이터가 null인 경우, 오른쪽의 기본값을 반환합니다.
                    if (apiResponse != null)
                    {
                        return apiResponse;
                    }
                    else
                    {
                        return apiResponse ?? new ApiResponse { StatusCode = 0, Message = AppResources.error };
                    }
                }
            }
            catch (Exception ex)
            {
                // await Toast.Make(AppResources.error + " : " + ex.Message, ToastDuration.Long).Show();
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
        }

        // 로그아웃 로직 예시
        public async void Logout()
        {
            IsLoggedIn = false;
            await Toast.Make(AppResources.logout, ToastDuration.Long).Show();
        }
        #endregion
    }
}
