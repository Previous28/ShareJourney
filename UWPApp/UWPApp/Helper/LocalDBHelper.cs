using SQLitePCL;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.Helper
{
    class LocalDBHelper
    {
        // 数据库连接
        public static SQLiteConnection connection = null;

        // 初始化数据库
        public static void initDB()
        {
            connection = new SQLiteConnection("Records.db");
            string sql = @"CREATE TABLE IF NOT EXISTS
                            Records(
                                Id CHAR(30) PRIMARY KEY NOT NULL,
                                UserId CHAR(30) NOT NULL,
                                Title VARCHAR(100) NOT NULL,
                                Content VARCHAR(1000) NOT NULL,
                                Image VARCHAR(100) NOT NULL,
                                Audio VARCHAR(100) NOT NULL,
                                Video VARCHAR(100) NOT NULL,
                                Date VARCHAR(20) NOT NULL,
                                Nickname VARCHAR(100) NOT NULL,
                                FavoriteNum CHAR(10) NOT NULL,
                                UserAvatar VARCHAR(120) NOT NULL
                            );";
            using (var statement = connection.Prepare(sql))
            {
                statement.Step();
            }
        }

        // 从本地数据库加载所有记录
        public static void loadAllRecordsFromDB()
        {
            using (var statement = connection.Prepare("SELECT * FROM Records"))
            {
                Store.RecordStore.allRecords.Clear();
                Store.RecordStore.userRecords.Clear();
                while (SQLiteResult.ROW == statement.Step())
                {
                    Model.Record record = new Model.Record();
                    record.id = (string)statement[0];
                    record.userId = (string)statement[1];
                    record.title = (string)statement[2];
                    record.content = (string)statement[3];
                    record.image = (string)statement[4];
                    record.audio = (string)statement[5];
                    record.video = (string)statement[6];
                    record.date = (string)statement[7];
                    record.nickname = (string)statement[8];
                    record.favoriteNum = (string)statement[9];
                    Uri avatar = new Uri((string)statement[10]);
                    record.userAvatar = new BitmapImage(avatar);
                    Store.RecordStore.allRecords.Add(record);
                    if (record.userId == Store.UserStore.userId)
                    {
                        Store.RecordStore.userRecords.Add(record);
                    }
                }
            }
        }

        // 将所有记录存储到本地数据库
        public static void savaAllRecordsToDB()
        {
            // 将所有记录存储到本地数据库
            for (int i = 0; i < Store.RecordStore.allRecords.Count; ++i)
            {
                using (var statement = connection.Prepare(@"INSERT INTO Records
                      (Id, UserId, Title, Content, Image, Audio, Video, Date, Nickname, FavoriteNum, UserAvatar)
                       VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"))
                {
                    statement.Bind(1, Store.RecordStore.allRecords[i].id);
                    statement.Bind(2, Store.RecordStore.allRecords[i].userId);
                    statement.Bind(3, Store.RecordStore.allRecords[i].title);
                    statement.Bind(4, Store.RecordStore.allRecords[i].content);
                    statement.Bind(5, Store.RecordStore.allRecords[i].image);
                    statement.Bind(6, Store.RecordStore.allRecords[i].audio);
                    statement.Bind(7, Store.RecordStore.allRecords[i].video);
                    statement.Bind(8, Store.RecordStore.allRecords[i].date);
                    statement.Bind(9, Store.RecordStore.allRecords[i].nickname);
                    statement.Bind(10, Store.RecordStore.allRecords[i].favoriteNum);
                    statement.Bind(11, Store.RecordStore.allRecords[i].userAvatar.UriSource.ToString());
                    statement.Step();
                }
            }
        }

        // 删除某条记录
        public static void deleteRecord(string recordId)
        {
            Debug.WriteLine("DELETE FROM Records WHERE Id='" + recordId + "'");
            using (var statement = connection.Prepare("DELETE FROM Records WHERE Id='" + recordId + "'"))
            {
                statement.Step();
            }
        }
    }
}
