using NetSpeed.Interface;
using NetSpeed.Util;
using System.Windows.Media;

namespace NetSpeed.ViewModel
{
    internal class VMSpeedView : NotifyBase
    {
        private readonly string[] Units = { "B/S", "KB/S", "MB/S", "GB/S" };
        private readonly AppTimer appTimer;
        private ISpeedViewMenu speedViewMenu;
        private string uploadSpeedText;
        private string downloadSpeedText;
        private Brush textColor;

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

        public Brush TextColor
        {
            get => textColor;
            set => Set(ref textColor, value);
        }

        public VMSpeedView()
        {
            uploadSpeedText = "初始化...";
            downloadSpeedText = "初始化...";
            SetTextColor();
            appTimer = new AppTimer();
            appTimer.UpdateSpeed += NetInfo_UpdateSpeed;
            appTimer.Start();
        }

        public void Inject(ISpeedViewMenu speedViewMenu)
        {
            this.speedViewMenu = speedViewMenu;
            this.speedViewMenu.RestartTimer += () => { appTimer.Restart(); };
            this.speedViewMenu.UpdateTextColor += SetTextColor;
        }

        private void SetTextColor()
        {
            Color color = ColorUtil.HexToDecColor(AppSetting.TextColor);
            TextColor = new SolidColorBrush(color);
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

        public void Close()
        {
            appTimer.Close();
        }
    }
}
