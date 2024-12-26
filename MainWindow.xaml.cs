using LAN_Chat.Pages;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace LAN_Chat
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //默认显示第一个页面Page_Message
            Contect_farme.Navigate(new Message_List());
        }
        private void LogoButton_Click(object sender, RoutedEventArgs e)
        {
            //选择照片，到Image控件
            Microsoft.Win32.OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "图片文件(*.jpg;*.png)|*.jpg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                Logo.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
    }
}
