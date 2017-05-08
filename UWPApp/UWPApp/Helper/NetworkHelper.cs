using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace UWPApp.Helper
{
    class NetworkHelper
    {
        // 服务器域名配置
        public static string SERVER = "http://118.89.35.155:8080";

        // 操作结果成功与失败的标志
        public static string SUCCESS = "ok";
        public static string FAILED = "error";

        // 发送普通表单 POST 请求
        private static async Task<JObject> POST(string api, FormUrlEncodedContent form)
        {
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(SERVER + api, form);
            Byte[] res = await response.Content.ReadAsByteArrayAsync();
            Encoding code = Encoding.GetEncoding("UTF-8");
            string resStr = code.GetString(res, 0, res.Length);
            return (JObject)JsonConvert.DeserializeObject(resStr);
        }

        // 发送普通 GET 请求
        private static async Task<JObject> GET(string api)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(SERVER + api);
            Byte[] res = await response.Content.ReadAsByteArrayAsync();
            Encoding code = Encoding.GetEncoding("UTF-8");
            string resStr = code.GetString(res, 0, res.Length);
            return (JObject)JsonConvert.DeserializeObject(resStr);
        }

        // 发送文件 POST 请求
        private static async Task<JObject> POST_FILE(string api, string key, StorageFile file)
        {
            // 读取文件流
            var streamData = await file.OpenReadAsync();
            var bytes = new byte[streamData.Size];
            using (var dataReader = new DataReader(streamData))
            {
                await dataReader.LoadAsync((uint)streamData.Size);
                dataReader.ReadBytes(bytes);
            }
            var streamContent = new ByteArrayContent(bytes);

            // 上传文件
            var content = new MultipartFormDataContent();
            content.Add(streamContent, key);
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(SERVER + api, content);
            Byte[] res = await response.Content.ReadAsByteArrayAsync();
            Encoding code = Encoding.GetEncoding("UTF-8");
            string resStr = code.GetString(res, 0, res.Length);
            return (JObject)JsonConvert.DeserializeObject(resStr);
        }

        // 用户登录接口
        public static async Task<JObject> signin(string username, string password)
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "username", username },
                { "password", password }
            });
            return await POST("/api/auth/signin", form);
        }

        // 用户注册接口
        public static async Task<JObject> signup(string username, string password, string nickname)
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "username", username },
                { "password", password },
                { "nickname", nickname }
            });
            return await POST("/api/auth/signup", form);
        }

        // 退出登录接口
        public static async Task<JObject> signout(string onlineId)
        {
            string api = "/api/auth/signout?onlineId=" + onlineId;
            return await GET(api);
        }

        // 用户改名改密码接口
        public static async Task<JObject> modify(string onlineId, string nickname, string password)
        {
            var dict = new Dictionary<string, string>()
            {
                { "onlineId", onlineId },
                { "nickname", nickname }
            };
            if (password.Length > 0)
            {
                dict["password"] = password;
            }
            return await POST("/api/auth/modify", new FormUrlEncodedContent(dict));
        }

        // 发布记录接口
        public static async Task<JObject> postRecord(string title, string content, string onlineId)
        {
            var form = new FormUrlEncodedContent(new Dictionary<string, string>()
            {
                { "title", title },
                { "content", content },
                { "onlineId", onlineId }
            });
            return await POST("/api/record/post", form);
        }

        // 删除记录接口
        public static async Task<JObject> deleteRecord(string onlineId, string recordId)
        {
            string api = "/api/record/delete?onlineId=" + onlineId + "&recordId=" + recordId;
            return await GET(api);
        }

        // 查看所有记录接口
        public static async Task<JObject> allRecords()
        {
            return await GET("/api/record/all");
        }

        // 查询记录详情接口
        public static async Task<JObject> recordDetail(string recordId)
        {
            string api = "/api/record/detail?recordId=" + recordId;
            return await GET(api);
        }

        // 点赞接口
        public static async Task<JObject> favorite(string onlineId, string recordId)
        {
            string api = "/api/record/favorite?onlineId=" + onlineId + "&recordId=" + recordId;
            return await GET(api);
        }

        // 查询用户记录接口
        public static async Task<JObject> recordsOfUser(string userId)
        {
            string api = "/api/record/records-of-user?userId=" + userId;
            return await GET(api);
        }

        // 上传图片接口
        public static async Task<JObject> uploadImage(StorageFile file, string onlineId, string recordId)
        {
            string api = "/api/upload/image?onlineId=" + onlineId + "&recordId=" + recordId;
            return await POST_FILE(api, "image", file);
        }

        // 上传音频接口
        public static async Task<JObject> uploadAudio(StorageFile file, string onlineId, string recordId)
        {
            string api = "/api/upload/audio?onlineId=" + onlineId + "&recordId=" + recordId;
            return await POST_FILE(api, "audio", file);
        }

        // 上传视频接口
        public static async Task<JObject> uploadVideo(StorageFile file, string onlineId, string recordId)
        {
            string api = "/api/upload/video?onlineId=" + onlineId + "&recordId=" + recordId;
            return await POST_FILE(api, "video", file);
        }

        // 上传头像接口
        public static async Task<JObject> uploadAvatar(StorageFile file, string onlineId)
        {
            string api = "/api/upload/avatar?onlineId=" + onlineId;
            return await POST_FILE(api, "avatar", file);
        }
    }
}
