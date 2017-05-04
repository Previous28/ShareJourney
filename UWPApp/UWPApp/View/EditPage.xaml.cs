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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace UWPApp.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class EditPage : Page
    {
        public EditPage()
        {
            this.InitializeComponent();
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
            else
            {
                (Window.Current.Content as Frame).Navigate(typeof(AuthPage));
            }
        }

        private void pickPicture(object sender, RoutedEventArgs e)
        {

        }

        private void pickMusic(object sender, RoutedEventArgs e)
        {

        }

        private void pickVideo(object sender, RoutedEventArgs e)
        {

        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {

        }

        private void publishClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
