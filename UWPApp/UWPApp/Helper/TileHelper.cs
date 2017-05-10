using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace UWPApp.Helper
{
    class TileHelper
    {
        public static void setTile()
        {
            // 创建磁贴内容
            TileContent tile = new TileContent()
            {
                Visual = new TileVisual()
                {
                    TileSmall = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage
                            {
                                Source = "ms-appx:///Assets/tile-bg-small.jpg"
                            }
                        }
                    },
                    TileMedium = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage
                            {
                                Source = "ms-appx:///Assets/tile-bg-mid.jpg"
                            }
                        }
                    },
                    TileWide = new TileBinding()
                    {
                        Content = new TileBindingContentAdaptive()
                        {
                            BackgroundImage = new TileBackgroundImage
                            {
                                Source = "ms-appx:///Assets/tile-bg-large.jpg"
                            }
                        }
                    }
                }
            };

            // 创建发送磁贴提醒
            var notification = new TileNotification(tile.GetXml());
            TileUpdateManager.CreateTileUpdaterForApplication().Update(notification);
        }
    }
}
