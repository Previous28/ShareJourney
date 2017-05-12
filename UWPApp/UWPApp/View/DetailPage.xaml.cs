using Newtonsoft.Json.Linq;
using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Media.Core;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace UWPApp.View
{
    public sealed partial class DetailPage : Page
    {
        private Model.Record currentRecord;
        GridView gridView = new GridView();
        private bool isEdit = false;
        private DataTransferManager dataTransferManager;

        public DetailPage()
        {
            this.InitializeComponent();
        }

        private void goBackToLastPage(object sender, BackRequestedEventArgs e)
        {
            if (isEdit)
            {
                (Window.Current.Content as Frame).Navigate(typeof(MainPage));
                isEdit = false;
            }
            else
            {
                if ((Window.Current.Content as Frame).CanGoBack && e.Handled == false)
                {
                    if ((Window.Current.Content as Frame).CurrentSourcePageType != typeof(AuthPage))
                    {
                        e.Handled = true;
                        (Window.Current.Content as Frame).GoBack();
                    }
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            //如果页面可以回退，则显示回退按钮
            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
                SystemNavigationManager.GetForCurrentView().BackRequested += goBackToLastPage;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }

            currentRecord = ((Model.Record)e.Parameter);
            if (currentRecord.favoriteNum == "-1")
            {
                isEdit = true;
                currentRecord.favoriteNum = "0";
            }
            //await (new MessageDialog(currentRecord.audio)).ShowAsync();

            //同步顶栏信息
            avatarInTopBar.ImageSource = currentRecord.userAvatar;
            publisher.Text = currentRecord.nickname;

            //同步宽屏标题、文字信息
            title1.Text = currentRecord.title;
            content1.Text = currentRecord.content;
            //同步窄屏标题、文字信息
            title2.Text = currentRecord.title;
            content2.Text = currentRecord.content;

            //判断是否显示图片、音频、视频
            if (currentRecord.image != "")
            {
                image1.Visibility = Visibility.Visible;//宽屏
                image2.Visibility = Visibility.Visible;//窄屏

                BitmapImage newSource = null;
                string _newSource = Helper.NetworkHelper.SERVER + currentRecord.image;
                newSource = new BitmapImage(new Uri(_newSource));
                image1.Source = image2.Source = newSource;
            }
            if (currentRecord.audio != "")
            {
                //宽屏
                player1.Visibility = Visibility.Visible;

                //窄屏
                player2.Visibility = Visibility.Visible;
                string newSource = Helper.NetworkHelper.SERVER + currentRecord.audio;
                player1.Source = player2.Source = MediaSource.CreateFromUri(new Uri(newSource, UriKind.Absolute));
            }
            if (currentRecord.video != "")
            {
                //宽屏
                player1.Visibility = Visibility.Visible;
                
                //窄屏
                player2.Visibility = Visibility.Visible;
                
                string newSource = Helper.NetworkHelper.SERVER + currentRecord.video;
                player1.Source = player2.Source = MediaSource.CreateFromUri(new Uri(newSource, UriKind.Absolute));
            }
            //同步点赞数
            favoriteNum.Text = currentRecord.favoriteNum;
            //解析点赞头像组
            JObject res = await Helper.NetworkHelper.recordDetail(currentRecord.id);
            if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
            {
                JObject records = JObject.Parse(res["record"].ToString());
                JArray favoriters = JArray.Parse(records["favoriter"].ToString());

                gridView.IsItemClickEnabled = false;
                gridView.Name = "gridView";
                gridView.Margin = new Thickness(30, 0, 0, 0);
                for (int i = 0; i < favoriters.Count; ++i)
                {
                    string _newFavoriter = Helper.NetworkHelper.SERVER + favoriters[i].ToString();
                    Ellipse ellipse = new Ellipse();
                    ImageBrush newFavoriter = new ImageBrush();
                    newFavoriter.ImageSource = new BitmapImage(new Uri(_newFavoriter));
                    ellipse.Fill = newFavoriter;
                    ellipse.Height = ellipse.Width = 60;
                    gridView.Items.Add(ellipse);
                }
                // Add the grid view to a parent container in the visual tree.
                likeList.Children.Add(gridView);
                //获取更详细的发布时间
                publishTime.Text = records["date"].ToString();
            }

            dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += dataRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            dataTransferManager.DataRequested -= dataRequested;
        }

        //把输入的结果转换成int类型
        int toInt(string str)
        {
            int temp = 0, tobe = 0;
            for (int i = 0; i < str.Length; ++i)
            {
                temp = str[i] - '0';
                tobe = tobe * 10 + temp;
            }
            return tobe;
        }

        //实现点赞功能
        private async void favorite(object sender, RoutedEventArgs e)
        {
            JObject res = await Helper.NetworkHelper.favorite(Store.UserStore.onlineId, currentRecord.id);
            if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
            {
                Store.RecordStore.loadAllRecordsFromServer();

                int tmp = toInt(currentRecord.favoriteNum) + 1;
                favoriteNum.Text = tmp.ToString();
                //在下方点赞列表显示头像
                Ellipse ellipse = new Ellipse();
                ImageBrush newFavoriter = new ImageBrush();
                newFavoriter.ImageSource = Store.UserStore.avatar;
                ellipse.Fill = newFavoriter;
                ellipse.Height = ellipse.Width = 60;
                gridView.Items.Add(ellipse);
            }
            else
            {
                await (new MessageDialog("您已经点过赞了！")).ShowAsync();
            }
        }

        // 分享记录
        private void shareRecord(object sender, RoutedEventArgs e)
        {
            DataTransferManager.ShowShareUI();
        }

        private void dataRequested(DataTransferManager sender, DataRequestedEventArgs e)
        {
            DataRequest request = e.Request;
            request.Data.Properties.Title = currentRecord.title;
            request.Data.Properties.Description = currentRecord.content;
            request.Data.SetText(currentRecord.content);
            if (currentRecord.image.Length > 0)
            {
                Uri imageUri = new Uri(Helper.NetworkHelper.SERVER + currentRecord.image);
                RandomAccessStreamReference image = RandomAccessStreamReference.CreateFromUri(imageUri);
                request.Data.SetBitmap(image);
            }
            else if (currentRecord.audio.Length > 0)
            {
                string audioUri = Helper.NetworkHelper.SERVER + currentRecord.audio;
                request.Data.SetText(currentRecord.content + " " + audioUri);
            }
            else if (currentRecord.video.Length > 0)
            {
                string videoUri = Helper.NetworkHelper.SERVER + currentRecord.video;
                request.Data.SetText(currentRecord.content + " " + videoUri);
            }
        }
    }

}
