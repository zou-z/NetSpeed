using NetSpeed.Interface;
using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace NetSpeed.Util
{
    internal class NetInfo : INetInfo
    {
        public event Action<long, long> UpdateSpeed;

        private NetworkInterface[] adapters;
        private NetworkInterface adapter;
        private IPInterfaceStatistics statistics;
        private Timer timer;
        private bool isRunning = false;

        private long bytesSent = 0;
        private long bytesReceived = 0;

        public NetInfo()
        {
            adapters = NetworkInterface.GetAllNetworkInterfaces();
            if (SetAdapter(AppSetting.AdapterId) > 0)
            {
                SetDefaultAdapter();
            }
            timer = new Timer(Timer_Tick, null, -1, 0);
        }

        public NetworkInterface GetAdapter()
        {
            return adapter;
        }

        public NetworkInterface[] GetAdapterList(bool isRefreshList = false)
        {
            if (isRefreshList)
            {
                adapters = NetworkInterface.GetAllNetworkInterfaces();
                if (adapters.Length == 0)
                {
                    _ = System.Windows.MessageBox.Show("无网络适配器");
                    return adapters;
                }
                for (int i = 0; i < adapters.Length; ++i)
                {
                    if (adapter.Id == adapters[i].Id)
                    {
                        SetAdapter(adapters[i]);
                        return adapters;
                    }
                }
                SetDefaultAdapter();
            }
            return adapters;
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public int SetAdapter(string adapterId)
        {
            if (adapterId == null || adapterId == "")
            {
                return 1;
            }
            else if (adapters.Length == 0)
            {
                return 2;
            }
            else
            {
                for (int i = 0; i < adapters.Length; ++i)
                {
                    if (adapters[i].Id == adapterId)
                    {
                        Stop();
                        SetAdapter(adapters[i]);
                        return 0;
                    }
                }
            }
            return 3;
        }

        public void SetDefaultAdapter()
        {
            if (adapters.Length == 0)
            {
                return;
            }
            for (int i = 0; i < adapters.Length; ++i)
            {
                if (adapters[i].OperationalStatus == OperationalStatus.Up && adapters[i].NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    SetAdapter(adapters[i]);
                    return;
                }
            }
            SetAdapter(adapters[0]);
        }

        private void SetAdapter(NetworkInterface networkInterface)
        {
            adapter = networkInterface;
            AppSetting.AdapterId = networkInterface.Id;
        }

        public void Start()
        {
            UpdateSpeed?.Invoke(0, 0);
            if (adapter == null)
            {
                _ = System.Windows.Forms.MessageBox.Show("程序无法运行,未选择网络设配器");
                return;
            }

            statistics = adapter?.GetIPStatistics();
            bytesSent = statistics.BytesSent;
            bytesReceived = statistics.BytesReceived;

            if (timer?.Change(1000, 1000) == true)
            {
                isRunning = true;
            }
        }

        public void Stop()
        {
            _ = timer?.Change(-1, 0);
        }

        public void Close()
        {
            Stop();
            timer?.Dispose();
            timer = null;
            adapter = null;
            adapters = null;
            statistics = null;
        }

        private void Timer_Tick(object state)
        {
            statistics = adapter.GetIPStatistics();
            UpdateSpeed?.Invoke(statistics.BytesSent - bytesSent, statistics.BytesReceived - bytesReceived);
            bytesSent = statistics.BytesSent;
            bytesReceived = statistics.BytesReceived;
        }
    }
}
