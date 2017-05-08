using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace UWPApp.Helper
{
    class FileHelper
    {
        // 选择图片
        public static async Task<StorageFile> selectImage()
        {
            // 文件选择器，默认打开目录为图片目录，// 过滤图片文件
            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".gif");
            // 选择一个文件
            return await filePicker.PickSingleFileAsync();
        }

        // 选择音频
        public static async Task<StorageFile> selectAudio()
        {
            // 文件选择器，默认打开目录为音乐目录，// 过滤音频文件
            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.MusicLibrary;
            filePicker.FileTypeFilter.Add(".mp3");
            // 选择一个文件
            return await filePicker.PickSingleFileAsync();
        }

        // 选择视频
        public static async Task<StorageFile> selectVideo()
        {
            // 文件选择器，默认打开目录为视频目录，// 过滤视频文件
            var filePicker = new FileOpenPicker();
            filePicker.SuggestedStartLocation = PickerLocationId.VideosLibrary;
            filePicker.FileTypeFilter.Add(".mp4");
            filePicker.FileTypeFilter.Add(".avi");
            // 选择一个文件
            return await filePicker.PickSingleFileAsync();
        }
    }
}
