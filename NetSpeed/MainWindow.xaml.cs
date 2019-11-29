using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.NetworkInformation;
using System.Diagnostics;
using System.Threading;
using System.Management;
using System.Collections;
using System.Timers;
using System.ComponentModel;

namespace NetSpeed
{
    public partial class MainWindow : Window
    {
        private readonly MonitorNetStream mns;
        private readonly MouseButtonEventHandler moveWindow;
        public MainWindow()
        {
            InitializeComponent();
            this.Left = SystemParameters.WorkArea.Width - this.Width;
            this.Top = SystemParameters.WorkArea.Height - this.Height;
            mns = new MonitorNetStream();
            UploadSpeed.DataContext = mns;
            DownloadSpeed.DataContext = mns;
            moveWindow = new MouseButtonEventHandler(MoveWindow);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(() =>
            {
                mns.Start();
            }));
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("QQ : 1575375168\r\n微信 : Guodcx\r\nGithub : https://github.com/zou-z/NetSpeed\r\n邮箱 : zzvr@outlook.com\r\n版本 : 1.0.0.0");
        }

        private void ChangePinState(object sender, RoutedEventArgs e)
        {
            switch (PinState.Header.ToString())
            {
                case "取消固定位置":
                    this.AddHandler(MouseLeftButtonDownEvent, moveWindow);
                    PinState.Header = "固定位置";
                    PinStateIcon.Text = "\xE718";
                    break;
                case "固定位置":
                    this.RemoveHandler(MouseLeftButtonDownEvent, moveWindow);
                    PinState.Header = "取消固定位置";
                    PinStateIcon.Text = "\xE77A";
                    break;
            }
        }

        private void Reposition(object sender, RoutedEventArgs e)
        {
            switch ((sender as MenuItem).Header.ToString())
            {
                case "左上角":
                    this.Left = this.Top = 0;
                    break;
                case "左下角":
                    this.Left = 0;
                    this.Top = SystemParameters.WorkArea.Height - this.Height;
                    break;
                case "右上角":
                    this.Left = SystemParameters.WorkArea.Width - this.Width;
                    this.Top = 0;
                    break;
                case "右下角":
                    this.Left = SystemParameters.WorkArea.Width - this.Width;
                    this.Top = SystemParameters.WorkArea.Height - this.Height;
                    break;
            }
        }
    }

    public class MonitorNetStream : INotifyPropertyChanged
    {
        private string download;
        private string upload;
        public event PropertyChangedEventHandler PropertyChanged;
        public string DownloadSpeed
        {
            get { return download; }
            set
            {
                download = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DownloadSpeed"));
            }
        }
        public string UploadSpeed
        {
            get { return upload; }
            set
            {
                upload = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UploadSpeed"));
            }
        }
        public MonitorNetStream()
        {
            DownloadSpeed = UploadSpeed = "启动中...";
        }
        public void Start()
        {
            List<PerformanceCounter> pcs = new List<PerformanceCounter>();
            List<PerformanceCounter> pcs2 = new List<PerformanceCounter>();
            string[] names = GetAdapter();
            foreach (string name in names)
            {
                try
                {
                    PerformanceCounter pc = new PerformanceCounter("Network Interface", "Bytes Received/sec", name.Replace('(', '[').Replace(')', ']'), ".");
                    PerformanceCounter pc2 = new PerformanceCounter("Network Interface", "Bytes Sent/sec", name.Replace('(', '[').Replace(')', ']'), ".");
                    pc.NextValue();
                    pcs.Add(pc);
                    pcs2.Add(pc2);
                }
                catch
                {
                    continue;
                }
            }
            ParameterizedThreadStart ts = new ParameterizedThreadStart(Run);
            Thread monitor = new Thread(ts);
            List<PerformanceCounter>[] pcss = new List<PerformanceCounter>[2];
            pcss[0] = pcs;
            pcss[1] = pcs2;
            monitor.Start(pcss);
        }
        public void Run(object obj)
        {
            List<PerformanceCounter>[] pcss = (List<PerformanceCounter>[])obj;
            List<PerformanceCounter> pcs = pcss[0];
            List<PerformanceCounter> pcs2 = pcss[1];
            while (true)
            {
                float recv = 0;
                float sent = 0;
                foreach (PerformanceCounter pc in pcs)
                {
                    recv += pc.NextValue();
                }
                foreach (PerformanceCounter pc in pcs2)
                {
                    sent += pc.NextValue();
                }
                DownloadSpeed = FormatUnit(recv);
                UploadSpeed = FormatUnit(sent);
                Thread.Sleep(1000);
            }
        }
        private string FormatUnit(float value)
        {
            string[] units = new string[] { "B/s", "K/s", "M/s", "G/s" };
            int i = 0;
            while (value >= 1024)
            {
                value /= 1024;
                i++;
            }
            return (int)value + " " + units[i];
        }
        public string[] GetAdapter()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            string[] name = new string[adapters.Length];
            int index = 0;
            foreach (NetworkInterface ni in adapters)
            {
                name[index] = ni.Description;
                index++;
            }
            return name;
        }
    }
}
