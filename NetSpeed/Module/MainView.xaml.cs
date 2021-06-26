using NetSpeed.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace NetSpeed.Module
{
    public partial class MainView : UserControl
    {
        private readonly NetInfo netInfo = new NetInfo();
        private readonly string[] Units = { "B/S", "KB/S", "MB/S", "GB/S" };

        public MainView()
        {
            InitializeComponent();
            DownloadSpeedText.Text = UploadSpeedText.Text = "初始化...";

            NetSpeedMenu menu = ContextMenu as NetSpeedMenu;
            netInfo.UpdateSpeed += UpdateSpeed;
            netInfo.InitializeCompleted += menu.SetAdapterList;
            menu.SetSelectedAdapter += netInfo.SetSelectedAdapter;
            menu.SetTextColor += SetTextColor;

            Loaded += MainView_Loaded;
        }

        private void MainView_Loaded(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                netInfo.Initialize();
                if (netInfo.Start() is string result && result != null)
                {
                    Content = new TextBlock
                    {
                        Text = result,
                        Foreground = Brushes.White,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextAlignment = TextAlignment.Right,
                    };
                }
            }));
        }

        private void UpdateSpeed(long download_bytes, long upload_bytes)
        {
            Dispatcher.BeginInvoke((Action)(() =>
            {
                DownloadSpeedText.Text = FormatSpeed(download_bytes);
                UploadSpeedText.Text = FormatSpeed(upload_bytes);
            }));
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
                return $"{speed:f2} {Units[index]}";
            else if (speed < 100)
                return $"{speed:f1} {Units[index]}";
            else if (speed < 1024)
                return $"{speed:f0} {Units[index]}";
            return $"0.00 {Units[0]}";
        }

        private void SetTextColor(Brush color)
        {
            StackPanel sp = Content as StackPanel;
            foreach (UIElement wrapPanel in sp.Children)
            {
                WrapPanel wp = wrapPanel as WrapPanel;
                foreach (UIElement ui in wp.Children)
                {
                    if (ui is TextBlock tb)
                    {
                        tb.Foreground = color;
                    }
                }
            }
        }

        public void ReleaseResources()
        {
            netInfo.Close();
        }
    }
}
