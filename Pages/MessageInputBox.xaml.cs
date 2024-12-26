using LAN_Chat.Controls;
using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace LAN_Chat.Pages
{
    public partial class MessageInputBox : Page
    {
        private static UdpClient udpClient;
        private static bool isListening = false;
        private const int listenPort = 11001; // 使用一个新的端口

        public MessageInputBox()
        {
            InitializeComponent();
            StartListener();
        }

        // 在 StartListener 方法中替换注释部分
        private async void StartListener()
        {
            if (udpClient == null)
            {
                udpClient = new UdpClient(listenPort);
            }

            if (!isListening)
            {
                isListening = true;
                await Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, listenPort);
                            byte[] receivedBytes = udpClient.Receive(ref remoteEP);
                            string message = Encoding.UTF8.GetString(receivedBytes);


                            // 用右下角弹窗提示消息 message，调用系统消息
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                NotifyIcon notifyIcon = new NotifyIcon

                                {
                                    BalloonTipTitle = remoteEP.Address.ToString(), // 设置消息标题是发消息的人的IP地址
                                    BalloonTipIcon = ToolTipIcon.Info,// 设置消息图标  是软件图标，logo.ico
                                    BalloonTipText = message,
                                    Visible = true,
                                    Icon = SystemIcons.Information
                                };
                                notifyIcon.ShowBalloonTip(3000);
                                notifyIcon.Visible = false; // 隐藏图标
                            });

                            Application.Current.Dispatcher.InvokeAsync(() =>
                            {
                                Message_Other receivedMessage = new Message_Other
                                {
                                    TextBlock = { Text = message }
                                };
                                ChatHistory.Children.Add(receivedMessage);
                            });
                        }
                        catch (Exception ex)
                        {
                            Application.Current.Dispatcher.Invoke(() => MessageBox.Show($"接收消息失败: {ex.Message}"));
                        }
                    }
                });
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string messageText = InputBox.Text;

            if (string.IsNullOrWhiteSpace(messageText))
            {
                System.Windows.Forms.MessageBox.Show("发送内容不能为空");
                return;
            }

            Message_Me message = new Message_Me
            {
                TextBlock = { Text = messageText }
            };
            ChatHistory.Children.Add(message);
            InputBox.Clear();

            SendMessageToHost(IPText.Text, messageText);
        }

        private async void SendMessageToHost(string ipAddress, string message)
        {
            try
            {
                using (UdpClient udpClient = new UdpClient())
                {
                    IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ipAddress), listenPort);
                    byte[] sendBytes = Encoding.UTF8.GetBytes(message);
                    await udpClient.SendAsync(sendBytes, sendBytes.Length, endPoint);

                    // 添加日志记录
                    //MessageBox.Show($"消息已发送到 {ipAddress}:{listenPort} - 内容: {message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"发送消息失败: {ex.Message}");
            }
        }
    }
}
