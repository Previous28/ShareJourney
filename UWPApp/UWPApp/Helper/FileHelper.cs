using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace UWPApp.Helper
{
    class FileHelper
    {
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
    }
}
