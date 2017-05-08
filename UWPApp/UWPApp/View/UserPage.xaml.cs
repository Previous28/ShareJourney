using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace UWPApp.View
{
    public sealed partial class UserPage : Page
    {
        public UserPage()
        {
            this.InitializeComponent();
            Data = Store.RecordStore.getInstance();
        }

        private Store.RecordStore Data;
        private string _avatar = Helper.NetworkHelper.SERVER + Store.UserStore.avatar;
        private string _username = Store.UserStore.username;
        private string _nickname = Store.UserStore.nickname;

        // 更新用户信息
        private bool modify = false;
        private void setUserInfo(object sender, RoutedEventArgs e)
        {
            if (modify)
            {
                // TODO: 更新用户信息
                modify = false;
                nicknameText.Visibility = Visibility.Visible;
                usernameText.Visibility = Visibility.Visible;
                nickname.Visibility = Visibility.Collapsed;
                password.Visibility = Visibility.Collapsed;
                confirm.Visibility = Visibility.Collapsed;
                actionBtn.Content = "Modify";
            }
            else
            {
                modify = true;
                nicknameText.Visibility = Visibility.Collapsed;
                usernameText.Visibility = Visibility.Collapsed;
                nickname.Visibility = Visibility.Visible;
                password.Visibility = Visibility.Visible;
                confirm.Visibility = Visibility.Visible;
                actionBtn.Content = "Update";
            }
        }

        // 退出登录
        private async void signout(object sender, RoutedEventArgs e)
        {
            JObject res = await Helper.NetworkHelper.signout(Store.UserStore.onlineId);
            if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
            {
                (Window.Current.Content as Frame).Navigate(typeof(View.AuthPage));
            }
        }

        // 页面跳转
        private void goToOtherPage(object sender, RoutedEventArgs e)
        {
            AppBarButton btn = sender as AppBarButton;
            if (btn.Name == "main")
            {
                (Window.Current.Content as Frame).Navigate(typeof(MainPage));
            }
            else if (btn.Name == "user")
            {
                (Window.Current.Content as Frame).Navigate(typeof(UserPage));
            }
            else if (btn.Name == "edit")
            {
                (Window.Current.Content as Frame).Navigate(typeof(EditPage));
            }
            else if (btn.Name == "detail")
            {
                (Window.Current.Content as Frame).Navigate(typeof(DetailPage));
            }
        }
    }
}
