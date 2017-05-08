using System;
using System.Collections.Generic;
using System.IO;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.View
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Data = Store.RecordStore.getInstance();
            Uri avatarUri = new Uri(Helper.NetworkHelper.SERVER + Store.UserStore.avatar);
            avatarInTopBar.ImageSource = new BitmapImage(avatarUri);
        }

        private Store.RecordStore Data;

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
