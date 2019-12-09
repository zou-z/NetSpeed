<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NetSpeed
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 若不带参数"StartDirectly"启动程序则会向用户询问"是否需要开机自启本程序?"
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 1 && e.Args[0] == "StartDirectly")
                return;
            if (MessageBox.Show("是否需要开机自启本程序?\r\n*可在程序设置选项中随时开启和关闭\r\n*设置开机自启需要管理员权限", "设置开机自启", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                FileStream fs = new FileStream(@"D:\tmp.bat", FileMode.Create);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                string cmd = "@echo off\r\n" +
                    "title 设置开机自启\r\n" +
                    "echo 正在写入注册表信息...\r\n" +
                    "reg add \"HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\" /v \"NetSpeed\" /t \"REG_SZ\" /d \"" + Process.GetCurrentProcess().MainModule.FileName + " StartDirectly" + "\" /f\r\n" +
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
                    MessageBox.Show(ex.Message, "设置开机自启失败", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }
    }
}
=======
﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NetSpeed
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
    }
}
>>>>>>> 85550e0aa6c67e8e16422bd4b7157dbbd24605a1
