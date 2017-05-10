using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace UWPApp.View
{
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();
        }

        // 标记当前是登录还是注册状态，默认为登录
        private bool signUp = false;

        // 更新输入表单状态：登录表单或注册表单
        private void changeState(object sender, RoutedEventArgs e)
        {
            signUp = !signUp;
            if (signUp)
            {
                confirm.Visibility = Visibility.Visible;
                nickname.Visibility = Visibility.Visible;
                mainBtn.Content = "Sign up";
                subBtn.Content = "Back";
            }
            else
            {
                confirm.Visibility = Visibility.Collapsed;
                nickname.Visibility = Visibility.Collapsed;
                mainBtn.Content = "Sign in";
                subBtn.Content = "Sign up";
            }
        }

        // 执行登录或注册操作
        private async void doAction(object sender, RoutedEventArgs e)
        {
            if (signUp)
            {
                // 用户名或密码格式非法
                if (username.Text.Length < 8 || username.Text.Length > 15 ||
                    password.Password.Length < 8 || password.Password.Length > 15)
                {
                    await (new MessageDialog("用户名和密码长度为8-15个字符！")).ShowAsync();
                }
                // 密码和重复密码不一致
                else if (password.Password != confirm.Password)
                {
                    await (new MessageDialog("密码和确认密码不一致！")).ShowAsync();
                }
                // 没有输入昵称
                else if (nickname.Text.Length == 0)
                {
                    await (new MessageDialog("请输入昵称！")).ShowAsync();
                }
                // 数据合法，进行注册
                else
                {
                    JObject res = await Helper.NetworkHelper.signup(username.Text, password.Password, nickname.Text);
                    if (res["result"].ToString() == Helper.NetworkHelper.FAILED)
                    {
                        await (new MessageDialog("注册失败！该用户名已被注册！")).ShowAsync();
                    }
                    else
                    {
                        Store.UserStore.setUserInfo(res);
                        Store.RecordStore.loadAllRecordsFromServer();
                        (Window.Current.Content as Frame).Navigate(typeof(View.MainPage));
                    }
                }
            }
            else
            {
                // 没有输入用户名或密码
                if (username.Text.Length == 0 || password.Password.Length == 0)
                {
                    await (new MessageDialog("请输入用户名密码！")).ShowAsync();
                }
                // 进行登录
                else
                {
                    JObject res = await Helper.NetworkHelper.signin(username.Text, password.Password);
                    if (res["result"].ToString() == Helper.NetworkHelper.FAILED)
                    {
                        await (new MessageDialog("登录失败！请输入正确的用户名密码！")).ShowAsync();
                    }
                    else
                    {
                        Store.UserStore.setUserInfo(res);
                        // 初始化数据库连接
                        Helper.LocalDBHelper.initDB();
                        // 读取本地数据库的缓存数据
                        Helper.LocalDBHelper.loadAllRecordsFromDB();
                        (Window.Current.Content as Frame).Navigate(typeof(View.MainPage));
                    }
                }
            }
        }
    }
}
