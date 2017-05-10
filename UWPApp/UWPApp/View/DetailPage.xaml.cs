using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace UWPApp.View
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class DetailPage : Page
    {
        private Model.Record currentRecord;
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
            publishTime.Text = currentRecord.date;

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
            JObject res = await Helper.NetworkHelper.recordDetail(currentRecord.id);
            if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
            {
                JArray records = JArray.Parse(res["records"].ToString());
                
            }
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

        }

        private void stopClick(object sender, RoutedEventArgs e)
        {

        }
    }

}
