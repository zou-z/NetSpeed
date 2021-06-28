using NetSpeed.Interface;
using NetSpeed.Util;

namespace NetSpeed.ViewModel
{
    internal class VMSpeedView : NotifyBase
    {
        private readonly INetInfo netInfo;
        private ISpeedViewMenu speedViewMenu;
        private string uploadSpeedText;
        private string downloadSpeedText;

        private readonly string[] Units = { "B/S", "KB/S", "MB/S", "GB/S" };

        public string UploadSpeedText
        {
            get => uploadSpeedText;
            set => Set(ref uploadSpeedText, value);
        }

        public string DownloadSpeedText
        {
            get => downloadSpeedText;
            set => Set(ref downloadSpeedText, value);
        }

        public VMSpeedView()
        {
            uploadSpeedText = "初始化...";
            downloadSpeedText = "初始化...";
            netInfo = new NetInfo();
            netInfo.UpdateSpeed += NetInfo_UpdateSpeed;
        }

        public void Inject(ISpeedViewMenu speedViewMenu)
        {
            this.speedViewMenu = speedViewMenu;
            this.speedViewMenu.UpdateAdapterList(netInfo.GetAdapterList(), netInfo.GetAdapter());
            this.speedViewMenu.SelectedAdapter += SpeedViewMenu_SelectedAdapter;
            netInfo.Start();
        }

        private int SpeedViewMenu_SelectedAdapter(string adapterId)
        {
            int result = netInfo.SetAdapter(adapterId);
            netInfo.Start();
            return result;
        }

        private void NetInfo_UpdateSpeed(long upload, long download)
        {
            UploadSpeedText = FormatSpeed(upload);
            DownloadSpeedText = FormatSpeed(download);
        }

        private string FormatSpeed(double speed)
        {
            int index = 0;
            while (speed >= 1024)
            {
                speed /= 1024;
                ++index;
            }
            if (speed < 10)
            {
                return $"{speed:f2} {Units[index]}";
            }
            else if (speed < 100)
            {
                return $"{speed:f1} {Units[index]}";
            }
            else if (speed < 1024)
            {
                return $"{speed:f0} {Units[index]}";
            }
            return $"0.00 {Units[0]}";
        }
    }
}
