using Newtonsoft.Json.Linq;
using System;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.Store
{
    class UserStore
    {
        public static string userId = "";
        public static string onlineId = "";
        public static string username = "";
        public static string nickname = "";
        public static BitmapImage avatar = null;

        // 解析 JSON 数据提取用户信息
        public static void setUserInfo(JObject userInfo)
        {
            onlineId = userInfo["onlineId"].ToString();
            username = userInfo["username"].ToString();
            nickname = userInfo["nickname"].ToString();
            userId = userInfo["userId"].ToString();
            string _avatar = Helper.NetworkHelper.SERVER + userInfo["avatar"].ToString();
            avatar = new BitmapImage(new Uri(_avatar));
        }
    }
}
