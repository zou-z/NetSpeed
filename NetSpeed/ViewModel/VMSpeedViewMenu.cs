using NetSpeed.Interface;
using NetSpeed.Util;
using NetSpeed.View;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace NetSpeed.ViewModel
{
    internal class VMSpeedViewMenu : NotifyBase, ISpeedViewMenu
    {
        public event Action RestartTimer;
        public event Action UpdateTextColor;

        public ObservableCollection<Control> AdapterListMenu { get; set; }

        public ObservableCollection<MenuItem> RefreshIntervalMenu { get; set; }

        public ObservableCollection<MenuItem> TextColorMenu { get; set; }

        public VMSpeedViewMenu()
        {
            AdapterListMenu = new ObservableCollection<Control>();
            RefreshIntervalMenu = new ObservableCollection<MenuItem>();
            TextColorMenu = new ObservableCollection<MenuItem>();
            InitAdapterListMenu();
            InitRefreshIntervalMenu();
            InitTextColorMenu();
        }

        private void InitAdapterListMenu()
        {
            SetAdapterList();
            AdapterListMenu.Add(new Separator { Margin = new Thickness(-30, 8, 0, 8), Background = new SolidColorBrush(Color.FromRgb(232, 232, 232)) });
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
                    Command = new RelayCommand<int>(SelectRefreshInterval),
                    CommandParameter = intervals[i],
                });
            }
        }

        private void InitTextColorMenu()
        {
            ColorPicker colorPicker = new ColorPicker();
            colorPicker.UpdateTextColor += () => { UpdateTextColor?.Invoke(); };
            TextColorMenu.Add(colorPicker);
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
            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(1000) };
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
                item.Icon = (string)item.CommandParameter == adapterId ? "\xE001" : null;
                if (AppSetting.AdapterList[i].Id == adapterId)
                {
                    AppSetting.SelectedAdapter = AppSetting.AdapterList[i];
                    RestartTimer?.Invoke();
                }
            }
        }

        private void SelectRefreshInterval(int interval)
        {
            for (int i = 0; i < RefreshIntervalMenu.Count; ++i)
            {
                MenuItem item = RefreshIntervalMenu[i];
                item.Icon = (int)item.CommandParameter == interval? "\xE001" : null;
            }
            AppSetting.RefreshInterval = interval;
            RestartTimer?.Invoke();
        }
    }
}
