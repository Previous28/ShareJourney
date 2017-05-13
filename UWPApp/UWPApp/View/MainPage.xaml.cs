using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using Windows.UI.Popups;

namespace UWPApp.View
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            recordList = Store.RecordStore.allRecords;
            avatarInTopBar.ImageSource = Store.UserStore.avatar;
        }

        private ObservableCollection<Model.Record> recordList = null;

        // 查看某条记录的详情
        private void goToDetail(object sender, ItemClickEventArgs e)
        {
            (Window.Current.Content as Frame).Navigate(typeof(DetailPage), e.ClickedItem);
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

        // 刷新全部记录
        private void updateAllRecords(object sender, RoutedEventArgs e)
        {
            Store.RecordStore.loadAllRecordsFromServer();
            Helper.LocalDBHelper.savaAllRecordsToDB();
        }

        // 点赞
        private async void favorite(object sender, RoutedEventArgs e)
        {
            string recordId = (sender as AppBarButton).DataContext.ToString();
            JObject res = await Helper.NetworkHelper.favorite(Store.UserStore.onlineId, recordId);
            if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
            {
                Store.RecordStore.loadAllRecordsFromServer();
            }
            else
            {
                await (new MessageDialog("您已经点过赞了！")).ShowAsync();
            }
        }
    }
}
