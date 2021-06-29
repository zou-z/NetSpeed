using NetSpeed.DataType;
using NetSpeed.Interface;
using NetSpeed.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace NetSpeed.ViewModel
{
    internal class VMSpeedViewMenu : NotifyBase, ISpeedViewMenu
    {
        public event Func<string, int> SelectedAdapter;
        public event Action RefreshedAdapterList;
        public event Action RestartedTimer;

        private List<MenuItem> items = new List<MenuItem>();
        private readonly ObservableCollection<Control> adapterList = new ObservableCollection<Control>();
        private readonly ObservableCollection<MenuItem> intervalList = new ObservableCollection<MenuItem>();

        public List<MenuItem> Items
        {
            get => items;
            set => Set(ref items, value);
        }

        public VMSpeedViewMenu()
        {
            #region 网络适配器
            Items.Add(new MenuItem { Header = "网络适配器", Icon = "\xEDA3", });
            adapterList.Add(new Separator { Margin = new Thickness(0, 7, 20, 7) });
            adapterList.Add(new MenuItem
            {
                Header = "刷新列表",
                Icon = "\xE149",
                StaysOpenOnClick = true,
                Command = new RelayCommand(RefreshAdapterList)
            });
            Items[0].ItemsSource = adapterList;
            #endregion
            #region 设置
            Items.Add(new MenuItem { Header = "设置", Icon = "\xE115" });
            #region 刷新间隔
            Items[1].Items.Add(new MenuItem { Header = "刷新间隔" });
            int[] intervals = RefreshIntervals.GetValues();
            for (int i = 0; i < intervals.Length; ++i)
            {
                intervalList.Add(new MenuItem
                {
                    Header = $"{intervals[i]} 毫秒(ms)",
                    Icon = AppSetting.RefreshInterval == intervals[i] ? "\xE001" : null,
                    StaysOpenOnClick = true,
                    Command = new RelayCommand<int>(RestartTimer),
                    CommandParameter = intervals[i],
                    ToolTip = intervals[i] == RefreshIntervals.Default ? "默认值" : null
                });
            }
            MenuItem item = (MenuItem)(Items[1].Items[0]);
            item.ItemsSource = intervalList;
            #endregion
            #region 字体颜色
            Items[1].Items.Add(new MenuItem { Header = "字体颜色" });
            item = (MenuItem)(Items[1].Items[1]);
            item.Items.Add(new MenuItem
            {
                Header = "待添加...",
                StaysOpenOnClick = true
            });



            #endregion
            #endregion
            #region 关于
            Items.Add(new MenuItem { Header = "关于", Icon = "\xE946" });
            #endregion
        }

        public void UpdateAdapterList(NetworkInterface[] adapters, NetworkInterface adapter)
        {
            while (adapterList.Count > 2)
            {
                adapterList.RemoveAt(0);
            }
            for (int i = 0; i < adapters.Length; ++i)
            {
                MenuItem item = new MenuItem
                {
                    Header = adapters[i].Description,
                    Icon = adapters[i].Equals(adapter) ? "\xE001" : null,
                    ToolTip = adapters[i].GetDetail(),
                    StaysOpenOnClick = true,
                    Command = new RelayCommand<string>(SelectAdapter),
                    CommandParameter = adapters[i].Id,
                };
                adapterList.Insert(i, item);
            }
        }

        private void SelectAdapter(string adapterId)
        {
            int? result = SelectedAdapter?.Invoke(adapterId);
            if (result == null)
            {
                return;
            }
            if (result == 0)
            {
                for (int i = 0; i < adapterList.Count - 2; ++i)
                {
                    MenuItem item = (MenuItem)adapterList[i];
                    item.Icon = item.CommandParameter.ToString() == adapterId ? "\xE001" : null;
                }
            }
            else
            {
                adapterId = "切换网络适配器失败，";
                switch (result)
                {
                    case 1: adapterId += "目标网络适配器为空"; break;
                    case 2: adapterId += "网络适配器列表为空"; break;
                    case 3: adapterId += "网络适配器列表中未找到目标网络适配器"; break;
                    default: adapterId += "未知错误"; break;
                }
                _ = MessageBox.Show(adapterId);
            }
        }

        private void RefreshAdapterList()
        {
            if (RefreshedAdapterList == null)
            {
                return;
            }
            MenuItem item = (MenuItem)Items[0].Items[Items[0].Items.Count - 1];
            item.Header = "正在刷新列表...";
            item.IsEnabled = false;

            _ = Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() =>
            {
                RefreshedAdapterList();
                item.Header = "已刷新列表";
                item.Icon = "\xE930";
                DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1500) };
                timer.Tick += (sender, e) =>
                {
                    timer.Stop();
                    timer = null;
                    item.Header = "刷新列表";
                    item.Icon = "\xE149";
                    item.IsEnabled = true;
                };
                timer.Start();
            }));
        }

        private void RestartTimer(int interval)
        {
            foreach(MenuItem item in intervalList)
            {
                if(item.Icon == null)
                {
                    if((int)item.CommandParameter == interval)
                    {
                        item.Icon = "\xE001";
                        AppSetting.RefreshInterval = interval;
                        RestartedTimer();
                    }
                }
                else
                {
                    if ((int)item.CommandParameter != interval)
                    {
                        item.Icon = null;
                    }
                }
            }
        }
    }
}
