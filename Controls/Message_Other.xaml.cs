using System;
using System.Windows.Controls;

namespace LAN_Chat.Controls
{
    /// <summary>
    /// Message_Other.xaml 的交互逻辑
    /// </summary>
    public partial class Message_Other : UserControl
    {
        public Message_Other()
        {
            InitializeComponent();
            //TimeBlock.Text是消息收到的时间
            TimeBlock.Text = DateTime.Now.ToString("HH:mm:ss");

        }
    }
}
