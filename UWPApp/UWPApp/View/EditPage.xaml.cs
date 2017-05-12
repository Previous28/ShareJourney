using Newtonsoft.Json.Linq;
using System;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.View
{
    public sealed partial class EditPage : Page
    {
        private StorageFile file;
        private bool isImage = false;
        private bool isAudio = false;
        private bool isVideo = false;

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

        private async void pickPicture(object sender, RoutedEventArgs e)
        {
            file = await Helper.FileHelper.selectImage();
            if (file != null)
            {
                if (isAudio)
                {
                    isAudio = false;
                    audio.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage2.png"));
                }
                if (isVideo)
                {
                    isVideo = false;
                    video.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage3.png"));
                }
                if (!isImage)
                {
                    isImage = true;
                    string _newSource = "ms-appx:///Assets/imagePicked.png";
                    BitmapImage newSource = new BitmapImage(new Uri(_newSource));
                    image.Source = newSource;
                }
            }
        }

        private async void pickMusic(object sender, RoutedEventArgs e)
        {
            file = await Helper.FileHelper.selectAudio();
            if (file != null)
            {
                if (isImage)
                {
                    isImage = false;
                    image.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage1.png"));
                }
                if (isVideo)
                {
                    isVideo = false;
                    video.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage3.png"));
                }
                if (!isAudio)
                {
                    isAudio = true;
                    string _newSource = "ms-appx:///Assets/audioPicked.png";
                    BitmapImage newSource = new BitmapImage(new Uri(_newSource));
                    audio.Source = newSource;
                }
            }
        }

        private async void pickVideo(object sender, RoutedEventArgs e)
        {
            file = await Helper.FileHelper.selectVideo();
            if (file != null)
            {
                if (isImage)
                {
                    isImage = false;
                    image.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage1.png"));
                }
                if (isAudio)
                {
                    isAudio = false;
                    audio.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage2.png"));
                }
                if (!isVideo)
                {
                    isVideo = true;
                    string _newSource = "ms-appx:///Assets/videoPicked.png";
                    BitmapImage newSource = new BitmapImage(new Uri(_newSource));
                    video.Source = newSource;
                }
            }
        }

        private void cancelClick(object sender, RoutedEventArgs e)
        {
            title.Text = content.Text = "";
            isImage = isAudio = isVideo = false;
            image.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage1.png"));
            audio.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage2.png"));
            video.Source = new BitmapImage(new Uri("ms-appx:///Assets/imageForEditPage3.png"));
        }

        private async void publishClick(object sender, RoutedEventArgs e)
        {
            JObject res1 = await Helper.NetworkHelper.postRecord(title.Text, content.Text, Store.UserStore.onlineId);
            if (res1["result"].ToString() == Helper.NetworkHelper.SUCCESS)
            {
                JObject records = JObject.Parse(res1["record"].ToString());
                JObject res2 = null;
                if (isImage)
                {
                    res2 = await Helper.NetworkHelper.uploadImage(file, Store.UserStore.onlineId, records["_id"].ToString());
                }
                else if (isAudio)
                {
                    res2 = await Helper.NetworkHelper.uploadAudio(file, Store.UserStore.onlineId, records["_id"].ToString());
                }
                else if (isVideo)
                {
                    res2 = await Helper.NetworkHelper.uploadVideo(file, Store.UserStore.onlineId, records["_id"].ToString());
                }
                if (res2 != null)
                {
                    if (res2["result"].ToString() == Helper.NetworkHelper.SUCCESS)
                    {
                        await (new MessageDialog("发布成功！")).ShowAsync();
                        Model.Record newRecord1 = new Model.Record();
                        newRecord1.id = records["_id"].ToString();
                        newRecord1.userId = Store.UserStore.userId;
                        newRecord1.title = title.Text;
                        newRecord1.content = content.Text;
                        newRecord1.date = System.DateTime.Now.ToString();
                        newRecord1.nickname = Store.UserStore.nickname;
                        newRecord1.favoriteNum = "-1";
                        newRecord1.userAvatar = Store.UserStore.avatar;
                        newRecord1.image = newRecord1.audio = newRecord1.video = "";
                        if (isImage) newRecord1.image = res2["path"].ToString();
                        else if (isAudio) newRecord1.audio = res2["path"].ToString();
                        else if (isVideo) newRecord1.video = res2["path"].ToString();
                        (Window.Current.Content as Frame).Navigate(typeof(DetailPage), newRecord1);
                    }
                    else
                    {
                        await (new MessageDialog("上传失败！")).ShowAsync();
                        await Helper.NetworkHelper.deleteRecord(Store.UserStore.onlineId, records["_id"].ToString());
                    }
                }

                if (res2 == null)
                {
                    await (new MessageDialog("发布成功！")).ShowAsync();
                    Model.Record newRecord = new Model.Record();
                    newRecord.id = records["_id"].ToString();
                    newRecord.userId = Store.UserStore.userId;
                    newRecord.title = title.Text;
                    newRecord.content = content.Text;
                    newRecord.image = newRecord.video = newRecord.audio = "";
                    newRecord.date = System.DateTime.Now.ToString();
                    newRecord.nickname = Store.UserStore.nickname;
                    newRecord.favoriteNum = "-1";
                    newRecord.userAvatar = Store.UserStore.avatar;
                    (Window.Current.Content as Frame).Navigate(typeof(DetailPage), newRecord);
                }
            }
            else
            {
                await (new MessageDialog("发布失败！")).ShowAsync();
            }
        }
    }
}
