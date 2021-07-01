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
        public event Action RestartTimer;

        public ObservableCollection<Control> AdapterListMenu { get; set; }

        public ObservableCollection<MenuItem> RefreshIntervalMenu { get; set; }

        public VMSpeedViewMenu()
        {
            AdapterListMenu = new ObservableCollection<Control>();
            RefreshIntervalMenu = new ObservableCollection<MenuItem>();
            InitAdapterListMenu();
            InitRefreshIntervalMenu();
        }

        private void InitAdapterListMenu()
        {
            SetAdapterList();
            AdapterListMenu.Add(new Separator { Margin = new Thickness(0, 7, 20, 7) });
            AdapterListMenu.Add(new MenuItem
            {
                Header = "刷新列表",
                Icon = "\xE149",
                StaysOpenOnClick = true,
                Command = new RelayCommand(RefreshAdapterList)
            });
        }

        private void InitRefreshIntervalMenu()
        {
            int[] intervals = AppSetting.RefreshIntervals.GetValues();
            for (int i = 0; i < intervals.Length; ++i)
            {
                RefreshIntervalMenu.Add(new MenuItem
                {
                    Header = $"{intervals[i]} 毫秒(ms)",
                    Icon = AppSetting.RefreshInterval == intervals[i] ? "\xE001" : null,
                    ToolTip = intervals[i] == AppSetting.RefreshIntervals.Default ? "默认值" : null,
                    StaysOpenOnClick = true,
                    //Command = new RelayCommand<int>(RestartTimer),
                    //CommandParameter = intervals[i],
                });
            }
        }

        private void SetAdapterList()
        {
            while (AdapterListMenu.Count > 2)
            {
                AdapterListMenu.RemoveAt(0);
            }
            for (int i = 0; i < AppSetting.AdapterList.Length; ++i)
            {
                MenuItem item = new MenuItem
                {
                    Header = AppSetting.AdapterList[i].Description,
                    Icon = AppSetting.AdapterList[i].Id == AppSetting.SelectedAdapter.Id ? "\xE001" : null,
                    ToolTip = AppSetting.AdapterList[i].GetDetail(),
                    StaysOpenOnClick = true,
                    Command = new RelayCommand<string>(SelectAdapter),
                    CommandParameter = AppSetting.AdapterList[i].Id
                };
                AdapterListMenu.Insert(i, item);
            }
        }

        private void RefreshAdapterList()
        {
            if (AppSetting.UpdateAdapterList())
            {
                RestartTimer?.Invoke();
            }
            SetAdapterList();
            ShowRefreshAdapterListTip();
        }

        private void ShowRefreshAdapterListTip()
        {
            MenuItem item = (MenuItem)AdapterListMenu[AdapterListMenu.Count - 1];
            item.Header = "已刷新列表";
            item.Icon = "\xE930";
            item.IsEnabled = false;
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
        }

        private void SelectAdapter(string adapterId)
        {
            for (int i = 0; i < AdapterListMenu.Count - 2; ++i)
            {
                MenuItem item = (MenuItem)AdapterListMenu[i];
                item.Icon = item.CommandParameter.ToString() == adapterId ? "\xE001" : null;
                if (AppSetting.AdapterList[i].Id == adapterId)
                {
                    AppSetting.SelectedAdapter = AppSetting.AdapterList[i];
                    RestartTimer?.Invoke();
                }
            }
        }





        /*
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
            //if (RefreshedAdapterList == null)
            //{
            //    return;
            //}
            //MenuItem item = (MenuItem)Items[0].Items[Items[0].Items.Count - 1];
            //item.Header = "正在刷新列表...";
            //item.IsEnabled = false;

            //_ = Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() =>
            //{
            //    RefreshedAdapterList();
            //    item.Header = "已刷新列表";
            //    item.Icon = "\xE930";
            //    DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1500) };
            //    timer.Tick += (sender, e) =>
            //    {
            //        timer.Stop();
            //        timer = null;
            //        item.Header = "刷新列表";
            //        item.Icon = "\xE149";
            //        item.IsEnabled = true;
            //    };
            //    timer.Start();
            //}));
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
        */



    }
}
