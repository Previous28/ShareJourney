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
    public sealed partial class AuthPage : Page
    {
        public AuthPage()
        {
            this.InitializeComponent();
        }

        // 标记当前是登录还是注册状态，默认为登录
        private bool signUp = false;

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
    }
}
