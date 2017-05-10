using SQLitePCL;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media.Imaging;

namespace UWPApp.Helper
{
    class LocalDBHelper
    {
        // 数据库连接
        public static SQLiteConnection connection;

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
                                Image CHAR(100) NOT NULL,
                                Audio CHAR(100) NOT NULL,
                                Video CHAR(100) NOT NULL,
                                Date CHAR(20) NOT NULL,
                                Nickname VARCHAR(100) NOT NULL,
                                FavoriteNum INTEGER NOT NULL,
                                UserAvatar CHAR(120) NOT NULL
                            );";
            using (var statement = connection.Prepare(sql))
            {
                statement.Step();
            }
        }

        // 清空数据库
        public static void clearDB()
        {
            using (var statement = connection.Prepare("TRUNCATE TABLE Records"))
            {
                statement.Step();
            }
        }

        // 从本地数据库加载所有记录
        public static void loadAllRecordsFromDB(
            ObservableCollection<Model.Record> allRecords,
            ObservableCollection<Model.Record> userRecords)
        {
            using (var statement = connection.Prepare("SELECT * FROM Records"))
            {
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
                    record.favoriteNum = (int)statement[9];
                    Uri avatar = new Uri((string)statement[10]);
                    record.userAvatar = new BitmapImage(avatar);
                    allRecords.Add(record);
                    if (record.userId == Store.UserStore.userId)
                    {
                        userRecords.Add(record);
                    }
                }
            }
        }

        // 将所有记录存储到本地数据库
        public static void savaAllRecordsToDB(ObservableCollection<Model.Record> allRecords)
        {
            for (int i = 0; i < allRecords.Count; ++i)
            {
                using (var statement = connection.Prepare(@"INSERT INTO Records
                      (Id, UserId, Title, Content, Image, Audio, Video, Date, Nickname, FavoriteNum, UserAvatar)
                       VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"))
                {
                    statement.Bind(0, allRecords[i].id);
                    statement.Bind(1, allRecords[i].userId);
                    statement.Bind(2, allRecords[i].title);
                    statement.Bind(3, allRecords[i].content);
                    statement.Bind(4, allRecords[i].image);
                    statement.Bind(5, allRecords[i].audio);
                    statement.Bind(6, allRecords[i].video);
                    statement.Bind(7, allRecords[i].date);
                    statement.Bind(8, allRecords[i].nickname);
                    statement.Bind(9, allRecords[i].favoriteNum);
                    statement.Bind(10, allRecords[i].userAvatar.UriSource.ToString());
                    statement.Step();
                }
            }
        }
    }
}
