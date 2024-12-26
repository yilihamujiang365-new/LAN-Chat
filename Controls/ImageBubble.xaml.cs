using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace LAN_Chat.Controls
{
    public partial class ImageBubble : UserControl
    {
        public ImageBubble()
        {
            InitializeComponent();
        }

        public void SetImage(byte[] imageBytes)
        {
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.StreamSource = new MemoryStream(imageBytes);
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            ImageControl.Source = bitmap;
        }
    }
}