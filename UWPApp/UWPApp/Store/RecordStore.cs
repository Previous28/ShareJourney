using Newtonsoft.Json.Linq;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.Store
{
    public class RecordStore
    {
        // 所有记录集合
        public static ObservableCollection<Model.Record> allRecords = new ObservableCollection<Model.Record>();

        // 用户历史记录集合
        public static ObservableCollection<Model.Record> userRecords = new ObservableCollection<Model.Record>();

        // 从服务器加载所有记录
        public static async void loadAllRecordsFromServer()
        {
            JObject res = await Helper.NetworkHelper.allRecords();
            if (res["result"].ToString() == Helper.NetworkHelper.SUCCESS)
            {
                JArray records = JArray.Parse(res["records"].ToString());
                allRecords.Clear();
                userRecords.Clear();
                getRecordsFromJsonArray(records);
            }
        }

        // 解析 JSON 数组提取记录
        private static void getRecordsFromJsonArray(JArray jsonArray)
        {
            // 解析 JSON 数组
            for (int i = 0; i < jsonArray.Count; ++i)
            {
                JObject record = JObject.Parse(jsonArray[i].ToString());
                Model.Record _record = new Model.Record();
                _record.id = record["_id"].ToString();
                _record.title = record["title"].ToString();
                _record.content = record["content"].ToString();
                _record.image = record["image"].ToString();
                _record.audio = record["audio"].ToString();
                _record.video = record["video"].ToString();
                _record.userId = record["userId"].ToString();
                _record.date = record["date"].ToString().Split(' ')[0];
                _record.favoriteNum = record["favoriteNum"].ToObject<int>().ToString();
                string avatar = Helper.NetworkHelper.SERVER + record["userAvatar"].ToString();
                _record.userAvatar = new BitmapImage(new Uri(avatar));
                _record.nickname = record["nickname"].ToString();
                allRecords.Add(_record);
                if (_record.userId == UserStore.userId)
                {
                    userRecords.Add(_record);
                }
            }
        }
    }
}
