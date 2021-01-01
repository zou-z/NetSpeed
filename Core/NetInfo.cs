using System.Net.NetworkInformation;
using System.Threading;

namespace NetSpeed.Core
{
    internal sealed class NetInfo
    {
#pragma warning disable IDE0052 // 删除未读的私有成员
        private Timer timer;
#pragma warning restore IDE0052 // 删除未读的私有成员
        private NetworkInterface[] adapters;
        private NetworkInterface selectedAdapter = null;
        private long received;
        private long sent;

        public delegate void UpdateSpeedDelegate(long download_bytes, long upload_bytes);
        public delegate void InitializeCompletedDelegate(NetworkInterface[] adapters, NetworkInterface selectedAdapter);
        public event UpdateSpeedDelegate UpdateSpeed;
        public event InitializeCompletedDelegate InitializeCompleted;

        #region 外部调用的方法
        public void Initialize()
        {
            GetAllAdapter();
            if (SetDefaultAdapter())
            {
                InitializeCompleted(adapters, selectedAdapter);
            }
        }

        public string Start()
        {
            if (adapters.Length == 0)
            {
                return "无网络适配器";
            }
            StartWork();
            return null;
        }

        public void SetSelectedAdapter(string id)
        {
            timer.Change(-1, 0);
            foreach (NetworkInterface ni in adapters)
            {
                if (ni.Id == id)
                {
                    selectedAdapter = ni;
                    break;
                }
            }
            StartWork();
        }

        public void Close()
        {
            timer.Change(-1, 0);
            timer.Dispose();
        }
        #endregion

        private void StartWork()
        {
            UpdateSpeed(0, 0);
            InitReceivedAndSent();
            timer = new Timer(Timer_Tick, null, 1000, 1000);
        }

        /// <summary>
        /// 获取所有的适配器
        /// </summary>
        private void GetAllAdapter()
        {
            adapters = NetworkInterface.GetAllNetworkInterfaces();
        }

        /// <summary>
        /// 设置默认的适配器
        /// </summary>
        /// <returns>
        /// true: adapters.Length > 0 
        /// false: adapters.Length = 0
        /// </returns>
        private bool SetDefaultAdapter()
        {
            if (adapters.Length == 0)
            {
                return false;
            }
            foreach (NetworkInterface ni in adapters)
            {
                if (ni.OperationalStatus == OperationalStatus.Up)
                {
                    if (ni.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                    {
                        selectedAdapter = ni;
                        return true;
                    }
                }
            }
            selectedAdapter = adapters[0];
            return true;
        }

        private void Timer_Tick(object state)
        {
            IPInterfaceStatistics statistics = selectedAdapter.GetIPStatistics();
            UpdateSpeed(statistics.BytesReceived - received, statistics.BytesSent - sent);
            received = statistics.BytesReceived;
            sent = statistics.BytesSent;
        }

        /// <summary>
        /// 初始化接收和发送的字节数
        /// </summary>
        private void InitReceivedAndSent()
        {
            IPInterfaceStatistics statistics = selectedAdapter.GetIPStatistics();
            received = statistics.BytesReceived;
            sent = statistics.BytesSent;
        }
    }
}
