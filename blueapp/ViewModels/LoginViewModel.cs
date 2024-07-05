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
        public async Task<bool> LoginAsync(string username, string password)
        {
            try
            {
                var loginData = new { username, password };
                var client = new HttpClient();
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_loginEndpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    // 로그인 성공
                    IsLoggedIn = true;
                    return true;
                }
                else
                {
                    // 로그인 실패, 에러 메시지와 코드를 처리합니다.                    
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var responseObject = JsonSerializer.Deserialize<ApiResponse>(responseContent);
                    await Toast.Make($"Error {responseObject?.StatusCode}: {responseObject?.Message}", ToastDuration.Long).Show();
                    return false;
                }
            }
            catch (Exception ex)
            {
                await Toast.Make(AppResources.error + " : " + ex.Message, ToastDuration.Long).Show();

                return false;
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
