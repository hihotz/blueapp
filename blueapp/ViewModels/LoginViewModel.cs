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
using System.Xml.Linq;

namespace blueapp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private bool _isLoggedIn;
        private readonly string _loginEndpoint;
        private readonly string _signupEndpoint;
        private readonly string _deleteidEndpoint;
        private readonly string _changepwEndpoint;

        public LoginViewModel()
        {
            // api 주소 불러오기
            var (baseUrl, loginEndpoint, signupEndpoint, DeleteIDEndpoint, ChangePWEndpoint) = ApiConfigManager_User.LoadApiConfig();
            _loginEndpoint = $"{baseUrl}{loginEndpoint}";
            _signupEndpoint = $"{baseUrl}{signupEndpoint}";
            _deleteidEndpoint = $"{baseUrl}{DeleteIDEndpoint}";
            _changepwEndpoint = $"{baseUrl}{ChangePWEndpoint}";
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
                // name, pw 값이 비어있는지 확인
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pw))
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + AppResources.text_is_empty };
                }
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
                if (apiResponse?.StatusCode == 200 && apiResponse != null)
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
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
        }
        #endregion

        #region 회원가입 기능
        // 회원가입 로직
        public async Task<ApiResponse> RegisterAsync(string name, string pw)
        {
            try
            {
                // name, pw 값이 비어있는지 확인
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pw))
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + AppResources.text_is_empty };
                }
                var loginData = new
                {
                    username = name,
                    password = pw
                };

                // 타임아웃 5초
                var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_signupEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                // JSON 문자열의 이스케이프 문자 제거
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                var a = apiResponse?.StatusCode;
                var b = apiResponse?.Message;
                if (apiResponse?.StatusCode == 200 && apiResponse != null)
                {
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
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
        }
        #endregion

        #region 회원탈퇴 기능
        // 회원탈퇴 로직
        public async Task<ApiResponse> DeleteIDAsync(string name, string pw)
        {
            try
            {
                // name, pw 값이 비어있는지 확인
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pw))
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.text_is_empty };
                }
                var loginData = new
                {
                    username = name,
                    password = pw
                };

                // 타임아웃 5초
                var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_deleteidEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                // JSON 문자열의 이스케이프 문자 제거
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                if (apiResponse?.StatusCode == 200 && apiResponse != null)
                {
                    // 로그아웃 및 저장된 아아디/비번 삭제
                    Logout();
                    // 비밀번호는 Logout 동작에 포함
                    SecureStorage.Remove("UserName");
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
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
        }
        #endregion

        #region 비밀번호 수정
        // 회원탈퇴 로직
        public async Task<ApiResponse> ChangePWAsync(string oldpw, string newpw, string newpwcheck)
        {
            try
            {
                // name, pw 값이 비어있는지 확인
                if (string.IsNullOrEmpty(oldpw) || string.IsNullOrEmpty(newpw) || string.IsNullOrEmpty(newpwcheck))
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.text_is_empty };
                }
                // 새로운 비밀번호를 동일하게 입력했는지 확인
                if (newpw != newpwcheck)
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.pw_not_match };
                }

                var loginData = new
                {
                    username = await SecureStorage.GetAsync("UserName"),
                    oldpassword = oldpw,
                    newpassword = newpw
                };

                // 타임아웃 5초
                var client = new HttpClient { Timeout = TimeSpan.FromSeconds(5) };
                var json = JsonSerializer.Serialize(loginData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_changepwEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                // JSON 문자열의 이스케이프 문자 제거
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(responseContent, options);

                if (apiResponse?.StatusCode == 200 && apiResponse != null)
                {
                    // 로그아웃 및 저장된 아아디/비번 삭제
                    Logout();
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
                return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + ex.Message };
            }
        }
        #endregion

        #region 로그아웃 기능
        // 로그아웃 로직 예시
        public void Logout()
        {
            IsLoggedIn = false;
            SecureStorage.Remove("UserPW");
            Preferences.Remove("AutoLogin");
        }
        #endregion

    }
}
