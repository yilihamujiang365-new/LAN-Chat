using LAN_Chat.Controls;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LAN_Chat.Pages
{
    public partial class Message_List : Page
    {
        private HashSet<string> discoveredIPs = new HashSet<string>();
        private const int listenPort = 11000; // 使用一致的端口

        public Message_List()
        {
            InitializeComponent();
            App.InitializeUdpClient(); // 初始化 UdpClient 实例
            LoadLocalIPAddresses();
            StartListener();
            BroadcastPresence(); // 添加初始化时广播Presence
        }

        // 加载本地IP地址并添加到ComboBox
        private void LoadLocalIPAddresses()
        {
            var localIPs = GetLocalIPAddresses();
            foreach (var ip in localIPs)
            {
                ComboBoxItem item = new ComboBoxItem { Content = ip };
                IPComboBox.Items.Add(item);
            }
        }

        // 开始监听UDP广播消息
        private async void StartListener()
        {
            await Task.Run(() => ListenForBroadcasts());
        }

        // 监听广播消息的任务
        private void ListenForBroadcasts()
        {
            try
            {
                while (true)
                {
                    IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, listenPort);
                    byte[] receivedBytes = App.UdpClientInstance.Receive(ref remoteEP);
                    string message = Encoding.UTF8.GetString(receivedBytes);

                    if (message == "WeChat_LAN_Presence")
                    {
                        Application.Current.Dispatcher.Invoke(() => HandlePresenceMessage(remoteEP));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"监听广播时发生错误: {ex.Message}");
            }
        }

        // 处理接收到的Presence消息
        private void HandlePresenceMessage(IPEndPoint remoteEP)
        {
            if (!discoveredIPs.Contains(remoteEP.Address.ToString()) && !IsLocalIPAddress(remoteEP.Address.ToString()))
            {
                var user = new UserCard
                {
                    UserName = { Text = "用户_" + remoteEP.Address },
                    UserIP = { Text = remoteEP.Address.ToString() },
                    UserState = { Text = "在线" }
                };
                discoveredIPs.Add(remoteEP.Address.ToString());
                Message_List_Listbox.Items.Add(user);

                // 发送回执消息
                Task.Run(() => SendPresenceMessage(remoteEP.Address));
            }
        }

        // 广播Presence消息
        private async void BroadcastPresence()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Broadcast, listenPort);
                byte[] sendBytes = Encoding.UTF8.GetBytes("WeChat_LAN_Presence");
                await App.UdpClientInstance.SendAsync(sendBytes, sendBytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show($"广播消息失败: {ex.Message}"));
            }
        }

        // 发送Presence消息
        private async Task SendPresenceMessage(IPAddress address)
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(address, listenPort);
                byte[] sendBytes = Encoding.UTF8.GetBytes("WeChat_LAN_Presence");
                await App.UdpClientInstance.SendAsync(sendBytes, sendBytes.Length, endPoint);
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() => MessageBox.Show($"发送Presence消息失败: {ex.Message}"));
            }
        }

        // 获取本地IP地址
        private List<string> GetLocalIPAddresses()
        {
            List<string> localIPs = new List<string>();

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus == OperationalStatus.Up && ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    foreach (var ip in ni.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localIPs.Add(ip.Address.ToString());
                        }
                    }
                }
            }

            return localIPs;
        }

        // 判断IP是否为本地IP
        private bool IsLocalIPAddress(string ipAddress)
        {
            List<string> localIPs = GetLocalIPAddresses();
            return localIPs.Contains(ipAddress);
        }

        // 刷新按钮点击事件
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            discoveredIPs.Clear();
            Message_List_Listbox.Items.Clear();
            BroadcastPresence();
        }
    }
}
