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
    public class SplashViewModel : BaseViewModel
    {
        // db 검증 등 앱 로딩 시 필요한 작업을 수행합니다.
        public DatabaseService _databaseService;
        public SplashViewModel()
        {
            _databaseService = new DatabaseService();
        }
        
        #region 앱 로딩에 필요한 작업 예시
        // 앱 로딩
        public async Task Loading()
        {
            // 작업 수행 시간 임의 설정됨
            await Task.Delay(1000);

            bool SaveUserName = Preferences.Get("SaveUserName", false);
            bool AutoLogin = Preferences.Get("AutoLogin", false);

            // 자동로그인/아이디 저장 모두 꺼진경우 아이디/비번 삭제
            if (SaveUserName!=true && AutoLogin != true)
            {
                SecureStorage.Remove("UserName");
                SecureStorage.Remove("UserPW");
            }
            // 자동로그인 꺼짐 / 아이디 저장 켜진경우 비번 삭제
            else if (SaveUserName == true && AutoLogin !=true)
            {
                SecureStorage.Remove("UserPW");
            }
        }
        
        // 서버 체크
        public async Task<bool> ServerCheck()
        {
            // 로딩에 필요한 작업을 수행하도록 수정해야합니다.
            bool service = true;
            if (service)
            {
                // 작업 수행 시간 임의 설정됨
                await Task.Delay(1000);
                return true;
            }
            else
                return false;
        }

        // db 체크
        public async Task<bool> DBCheck()
        {
            // 로딩에 필요한 작업을 수행하도록 수정해야합니다.
            bool service = true;
            if (service)
            {
                // 작업 수행 시간 임의 설정됨
                await Task.Delay(1000);
                return true;
            }
            else
                return false;
        }

        // 업데이트 체크
        public async Task<bool> UpdateCheck()
        {
            // 로딩에 필요한 작업을 수행하도록 수정해야합니다.
            bool service = true;
            if (service)
            {
                // 작업 수행 시간 임의 설정됨
                await Task.Delay(1000);
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}
