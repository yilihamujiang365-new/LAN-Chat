using System.Net.Sockets;
using System.Windows;

namespace LAN_Chat
{
    public partial class App : Application
    {
        public static UdpClient UdpClientInstance { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            InitializeUdpClient();
        }

        public static void InitializeUdpClient()
        {
            if (UdpClientInstance == null)
            {
                UdpClientInstance = new UdpClient();
                UdpClientInstance.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                UdpClientInstance.Client.Bind(new System.Net.IPEndPoint(System.Net.IPAddress.Any, 11000));
            }
        }
    }
}