using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace NetSpeed.Util
{
    internal class AppTimer
    {
        public event Action<long, long> UpdateSpeed;
        private IPInterfaceStatistics statistics;
        private Timer timer;

        private long bytesSent = 0;
        private long bytesReceived = 0;

        public bool IsRunning { get; private set; }

        public AppTimer()
        {
            IsRunning = false;
            timer = new Timer(Timer_Tick, null, -1, 0);
        }

        private void Timer_Tick(object state)
        {
            statistics = AppSetting.SelectedAdapter?.GetIPStatistics();
            UpdateSpeed?.Invoke(statistics.BytesSent - bytesSent, statistics.BytesReceived - bytesReceived);
            bytesSent = statistics.BytesSent;
            bytesReceived = statistics.BytesReceived;
        }

        public void Start()
        {
            UpdateSpeed?.Invoke(0, 0);
            if (AppSetting.SelectedAdapter == null)
            {
                _ = System.Windows.Forms.MessageBox.Show("程序无法运行,未选择网络设配器");
                return;
            }

            statistics = AppSetting.SelectedAdapter.GetIPStatistics();
            bytesSent = statistics.BytesSent;
            bytesReceived = statistics.BytesReceived;

            if (timer?.Change(AppSetting.RefreshInterval, AppSetting.RefreshInterval) == true)
            {
                IsRunning = true;
            }
        }

        public void Stop()
        {
            _ = timer?.Change(-1, 0);
            IsRunning = false;
        }

        public void Close()
        {
            Stop();
            timer?.Dispose();
            timer = null;
            statistics = null;
        }
    }
}
