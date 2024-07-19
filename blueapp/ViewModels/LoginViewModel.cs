using blackapi.Models;
using blueapp.Data;
using blueapp.Models;
using blueapp.Resources.Localization;
using blueapp.Service;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
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
        private readonly LoginService _loginService;

        public ICommand LogoutCommand { get; }

        public LoginViewModel()
        {
            _loginService = new LoginService(new HttpClient());
            // api 주소 불러오기
            var (baseUrl, loginEndpoint, signupEndpoint, DeleteIDEndpoint, ChangePWEndpoint) = ApiConfigManager_User.LoadApiConfig();
            _loginEndpoint = $"{baseUrl}{loginEndpoint}";
            _signupEndpoint = $"{baseUrl}{signupEndpoint}";
            _deleteidEndpoint = $"{baseUrl}{DeleteIDEndpoint}";
            _changepwEndpoint = $"{baseUrl}{ChangePWEndpoint}";

            LogoutCommand = new Command(Logout);
            // 비동기로 변경시 LogoutCommand = new Command(async () => await Logout());
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
                var loginData = new User_LoginModel
                {
                    UserName = name,
                    Password = pw
                };

                var apiResponse = await _loginService.LoginAsync(loginData);

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
                var registerdata = new User_RegisterModel
                {
                    UserName = name,
                    Password = pw
                };

                var apiResponse = await _loginService.RegisterAsync(registerdata);

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

        #region 회원탈퇴 기능
        // 회원탈퇴 로직
        public async Task<ApiResponse> DeleteIDAsync(string name, string pw)
        {
            try
            {
                // name, pw 값이 비어있는지 확인
                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pw))
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.error + " : " + AppResources.text_is_empty };
                }
                
                var deleteidData = new User_DeleteID
                {
                    UserName = name,
                    Password = pw
                };

                var apiResponse = await _loginService.DeleteIDAsync(deleteidData);

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
        // 비밀번호 수정 로직
        public async Task<ApiResponse> ChangePWAsync(string oldpw, string newpw, string newpwcheck)
        {
            try
            {
                // id 값이 비어있는지 확인
                string? userid = await SecureStorage.GetAsync("UserName");
                if (string.IsNullOrEmpty(userid))
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.id + AppResources.error };
                }

                //pw 값이 비어있는지 확인
                if (string.IsNullOrEmpty(oldpw) || string.IsNullOrEmpty(newpw) || string.IsNullOrEmpty(newpwcheck))
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.text_is_empty };
                }

                // 새로운 비밀번호를 동일하게 입력했는지 확인
                if (newpw != newpwcheck)
                {
                    return new ApiResponse { StatusCode = 0, Message = AppResources.msg_pw_not_match };
                }

                var changepwData = new User_ChangePW
                {
                    UserName = userid,
                    OldPassword = oldpw,
                    NewPassword = newpw
                };

                var apiResponse = await _loginService.ChangePWAsync(changepwData);

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
        // 로그아웃 로직
        private void Logout()
        {
            IsLoggedIn = false;
            SecureStorage.Remove("UserPW");
            Preferences.Remove("AutoLogin");
        }
        #endregion

    }
}
