using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace UWPApp.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private Model.Record currentRecord;
        GridView gridView = new GridView();

        public DetailPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            //如果页面可以回退，则显示回退按钮
            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }

            currentRecord = ((Model.Record)e.Parameter);
            //await (new MessageDialog(currentRecord.image + currentRecord.audio + currentRecord.video)).ShowAsync();

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
            else if (currentRecord.audio != "")
            {
                //宽屏
                musicBg1.Visibility = Visibility.Visible;
                timelineSlider1.Visibility = Visibility.Visible;
                play1.Visibility = Visibility.Visible;
                //窄屏
                musicBg2.Visibility = Visibility.Visible;
                timelineSlider2.Visibility = Visibility.Visible;
                play2.Visibility = Visibility.Visible;
                string newSource = Helper.NetworkHelper.SERVER + currentRecord.audio;
                mediaElement1.Source = mediaElement2.Source = new Uri(newSource);
            }
            else if (currentRecord.video != "")
            {
                //宽屏
                mediaElement1.Visibility = Visibility.Visible;
                timelineSlider1.Visibility = Visibility.Visible;
                play1.Visibility = Visibility.Visible;
                //窄屏
                mediaElement2.Visibility = Visibility.Visible;
                timelineSlider2.Visibility = Visibility.Visible;
                play2.Visibility = Visibility.Visible;
                string newSource = Helper.NetworkHelper.SERVER + currentRecord.video;
                mediaElement1.Source = mediaElement2.Source = new Uri(newSource);
            }
            //同步点赞数
            favoriteNum.Text = currentRecord.favoriteNum.ToString();
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
        }

        //定义媒体播放器的一些逻辑
        private void ElementMediaOpened(object sender, RoutedEventArgs e)
        {
            var ts = mediaElement1.NaturalDuration.TimeSpan;
            timelineSlider1.Maximum = ts.TotalMilliseconds;
            timelineSlider2.Maximum = ts.TotalMilliseconds;
        }

        private void ElementMediaEnded(object sender, RoutedEventArgs e)
        {
            if (mediaElement1.Visibility == Visibility.Visible)
                mediaElement1.Stop();
            else
                mediaElement2.Stop();
        }

        private void SeekMediaPosition(object sender, RangeBaseValueChangedEventArgs e)
        {
            int SliderValue1 = (int)timelineSlider1.Value;
            int SliderValue2 = (int)timelineSlider2.Value;

            TimeSpan ts1 = new TimeSpan(0, 0, 0, 0, SliderValue1);
            mediaElement1.Position = ts1;
            TimeSpan ts2 = new TimeSpan(0, 0, 0, 0, SliderValue2);
            mediaElement2.Position = ts2;
        }

        private void playClick(object sender, RoutedEventArgs e)
        {
            if (mediaElement1.Visibility == Visibility.Visible)
            {
                mediaElement1.Play();
                play1.Visibility = Visibility.Collapsed;
                stop1.Visibility = Visibility.Visible;
            } else
            {
                mediaElement2.Play();
                play2.Visibility = Visibility.Collapsed;
                stop2.Visibility = Visibility.Visible;
            }
        }

        private void stopClick(object sender, RoutedEventArgs e)
        {
            if (mediaElement1.Visibility == Visibility.Visible)
            {
                mediaElement1.Stop();
                play1.Visibility = Visibility.Visible;
                stop1.Visibility = Visibility.Collapsed;
            }
            else
            {
                mediaElement2.Stop();
                play2.Visibility = Visibility.Visible;
                stop2.Visibility = Visibility.Collapsed;
            }
        }

        private void myMediaElementLoaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += (ss, ee) =>
            {
                //显示当前视频进度
                if (mediaElement1.Visibility == Visibility.Visible)
                {
                    var ts = mediaElement1.Position;
                    timelineSlider1.Value = ts.TotalMilliseconds;
                }
                else
                {
                    var ts = mediaElement2.Position;
                    timelineSlider2.Value = ts.TotalMilliseconds;
                }
            };
            timer.Start();
        }

        //实现点赞功能
        private async void favorite(object sender, RoutedEventArgs e)
        {
            JObject res = await Helper.NetworkHelper.favorite(Store.UserStore.onlineId, currentRecord.id);
            if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
            {
                //Store.RecordStore.loadAllRecordsFromServer();
                //Store.RecordStore.loadUserRecordsFromServer();

                long tmp = (currentRecord.favoriteNum) + 1;
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
                await(new MessageDialog("您已经点过赞了！")).ShowAsync();
            }
        }
    }

}
