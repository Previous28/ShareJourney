namespace UWPApp.Model
{
    public class Record
    {
        public Record (string id = "", string userId = "", string title = "",
            string content = "", string image = "", string audio = "",
            string video = "", long date = 0, int favoriteNum = 0)
        {
            this.id = id;
            this.userId = userId;
            this.title = title;
            this.content = content;
            this.image = image;
            this.audio = audio;
            this.video = video;
            this.date = date;
            this.favoriteNum = favoriteNum;
        }

        public string id;
        public string userId;
        public string title;
        public string content;
        public string image;
        public string audio;
        public string video;
        public long date;
        public int favoriteNum;
    }
}
