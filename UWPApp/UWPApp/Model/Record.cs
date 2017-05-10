using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.Model
{
    public class Record
    {
        public string id;
        public string userId;
        public string title;
        public string content;
        public string image;
        public string audio;
        public string video;
        public string date;
        public string nickname;
        public long favoriteNum;
        public BitmapImage userAvatar;
    }
}
