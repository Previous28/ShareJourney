using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.View
{
    public sealed partial class UserPage : Page
    {
        public UserPage()
        {
            this.InitializeComponent();
            Store.RecordStore.loadUserRecordsFromServer();
            recordList = Store.RecordStore.userRecords;
            nicknameText.Text = Store.UserStore.nickname;
            usernameText.Text = Store.UserStore.username;
            avatarInTopBar.ImageSource = Store.UserStore.avatar;
            avatarInContent.ImageSource = Store.UserStore.avatar;
        }

        private ObservableCollection<Model.Record> recordList = null;

        // 更换头像
        private async void setAvatar(object sender, RoutedEventArgs e)
        {
            StorageFile file = await Helper.FileHelper.selectImage();
            if (file != null)
            {
                JObject res = await Helper.NetworkHelper.uploadAvatar(file, Store.UserStore.onlineId);
                if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
                {
                    Uri avatarUri = new Uri(Helper.NetworkHelper.SERVER + res["path"].ToString());
                    Store.UserStore.avatar = new BitmapImage(avatarUri);
                    avatarInTopBar.ImageSource = Store.UserStore.avatar;
                    avatarInContent.ImageSource = Store.UserStore.avatar;
                }
                else
                {
                    await (new MessageDialog("上传失败！")).ShowAsync();
                }
            }
        }

        // 更新用户信息
        private bool modify = false;
        private async void setUserInfo(object sender, RoutedEventArgs e)
        {
            if (modify)
            {
                // 昵称不合法
                if (nickname.Text.Length == 0)
                {
                    await (new MessageDialog("请输入昵称！")).ShowAsync();
                }
                // 密码输入不一致
                else if (password.Password != confirm.Password)
                {
                    await (new MessageDialog("密码和确认密码不一致！")).ShowAsync();
                }
                // 需要更新信息
                else if (nickname.Text != Store.UserStore.nickname || password.Password.Length > 0)
                {
                    JObject res = await Helper.NetworkHelper.modify(Store.UserStore.onlineId, nickname.Text, password.Password);
                    if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
                    {
                        Store.UserStore.nickname = nickname.Text;
                        nicknameText.Text = nickname.Text;
                    }
                    else
                    {
                        await (new MessageDialog("修改信息失败！")).ShowAsync();
                    }
                }
                // 操作完毕更新界面
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
                nickname.Text = Store.UserStore.nickname;
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
        }
    }
}
