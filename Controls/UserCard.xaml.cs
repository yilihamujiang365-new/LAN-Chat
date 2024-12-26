using LAN_Chat.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LAN_Chat.Controls
{
    /// <summary>
    /// UserCard.xaml 的交互逻辑
    /// </summary>
    public partial class UserCard : UserControl
    {
        public UserCard()
        {
            InitializeComponent();
        }
        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            MessageInputBox messageInputBox = new MessageInputBox();
            messageInputBox.IPText.Text = UserIP.Text;
            if (mainWindow != null)
            {
                //如果已经打开了聊天窗口，则不再打开
                if (mainWindow.Message_farme.Content != null)
                {
                    return;
                }
                mainWindow.Message_farme.Navigate(messageInputBox);

            }
        }
    }
}
