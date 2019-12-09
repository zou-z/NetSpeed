<<<<<<< HEAD
﻿using System;
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
using Microsoft.Win32;
using System.IO;

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
            Environment.Exit(0);
        }

        private void AboutAndHelp(object sender, RoutedEventArgs e)
        {
            string[] about = new string[]
            {
                "QQ : 1575375168",
                "微信 : Guodcx",
                "邮箱 : zzvr@outlook.com",
                "版本 : "+Application.ResourceAssembly.GetName().Version.ToString(),
                "Github : https://github.com/zou-z/NetSpeed",
                "软件位置 : "+Process.GetCurrentProcess().MainModule.FileName,
                "帮助 : (1)若开机自启失效(可能是因为清除注册表或exe文件移动了位置造成的)，则手动运行exe文件选择设置开机自启(2)清除注册表后再删除本程序的exe文件即可完成卸载"
            };
            MessageBox.Show(about[0] + "\r\n" + about[1] + "\r\n" + about[2] + "\r\n" + about[3] + "\r\n" + about[4] + "\r\n" + about[5] + "\r\n" + about[6], "关于和帮助");
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

        private void RemoveRegedit(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("是否确认清除注册表?\r\n*该操作需要管理员权限\r\n*清除注册表将无法开机自启，清除后重启本程序可重获开机自启\r\n*清除注册表后再删除本程序的exe文件即可完成卸载", "清除注册表", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                FileStream fs = new FileStream(@"D:\tmp.bat", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                string cmd = "@echo off\r\n" +
                    "title 清除注册表\r\n" +
                    "echo 正在清除注册表信息...\r\n" +
                    "reg delete \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\" /v \"NetSpeed\" /f\r\n" +
                    "del %0 && pause";
                sw.Write(cmd);
                sw.Flush();
                sw.Close();
                fs.Close();
                try
                {
                    Process process = new Process();
                    process.StartInfo.FileName = @"D:\tmp.bat";
                    process.StartInfo.Verb = "runas";
                    process.Start();
                }
                catch (Exception ex)
                {
                    if (File.Exists(@"D:\tmp.bat"))
                        File.Delete(@"D:\tmp.bat");
                    MessageBox.Show(ex.Message, "清除注册表失败", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
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
=======
﻿using System;
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
            string[] about = new string[]
            {
                "QQ : 1575375168",
                "微信 : Guodcx",
                "邮箱 : zzvr@outlook.com",
                "版本 : "+Application.ResourceAssembly.GetName().Version.ToString(),
                "Github : https://github.com/zou-z/NetSpeed"
            };
            MessageBox.Show(about[0] + "\r\n" + about[1] + "\r\n" + about[2] + "\r\n" +about[3]+"\r\n"+ about[4]);
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
>>>>>>> 85550e0aa6c67e8e16422bd4b7157dbbd24605a1
