using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            Store.RecordStore.loadAllRecordsFromServer();
            recordList = Store.RecordStore.allRecords;
            avatarInTopBar.ImageSource = Store.UserStore.avatar;
        }

        private ObservableCollection<Model.Record> recordList = null;

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
