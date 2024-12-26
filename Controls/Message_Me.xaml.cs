using System;
using System.Windows.Controls;

namespace LAN_Chat.Controls
{
    /// <summary>
    /// Message_Me.xaml 的交互逻辑
    /// </summary>
    public partial class Message_Me : UserControl
    {
        public Message_Me()
        {
            InitializeComponent();
            //获取现在的时间
            TimeBlock.Text = DateTime.Now.ToString("HH:mm:ss");

        }
    }
}
