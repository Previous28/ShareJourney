using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.View
{
    public sealed partial class EditPage : Page
    {
        public EditPage()
        {
            this.InitializeComponent();
            avatarInTopBar.ImageSource = Store.UserStore.avatar;
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
