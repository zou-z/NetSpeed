using NetSpeed.Interface;
using NetSpeed.Util;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;

namespace NetSpeed.ViewModel
{
    internal class VMSpeedViewMenu : NotifyBase, ISpeedViewMenu
    {
        public event Func<string, int> SelectedAdapter;

        private List<MenuItem> items = new List<MenuItem>();

        public List<MenuItem> Items
        {
            get => items;
            set => Set(ref items, value);
        }

        public VMSpeedViewMenu()
        {
            Items.Add(new MenuItem
            {
                Header = "网络适配器",
                Icon = "\xEDA3",
            });
            Items.Add(new MenuItem
            {
                Header = "设置",
            });
            Items.Add(new MenuItem
            {
                Header = "关于",
                Icon = "\xE946"
            });
        }

        public void UpdateAdapterList(NetworkInterface[] adapters, NetworkInterface adapter)
        {
            List<FrameworkElement> list = new List<FrameworkElement>();
            for (int i = 0; i < adapters.Length; ++i)
            {
                MenuItem mi = new MenuItem
                {
                    Header = adapters[i].Description,
                    Icon = adapters[i].Equals(adapter) ? "\xE001" : "",
                    ToolTip = adapters[i].GetDetail(),
                    StaysOpenOnClick = true,
                    Command = new RelayCommand<string>(SelectAdapter),
                    CommandParameter = adapters[i].Id,
                };
                list.Add(mi);
            }
            list.Add(new Separator { Margin = new Thickness(0, 7, 20, 7) });
            list.Add(new MenuItem
            {
                Header = "刷新列表",
                Icon = "\xE149",
                StaysOpenOnClick = true,
                Command = new RelayCommand(() =>
                {
                    System.Diagnostics.Debug.WriteLine("ttt");
                })
            });
            Items[0].ItemsSource = list;
        }

        private void SelectAdapter(string adapterId)
        {
            int result = SelectedAdapter(adapterId);
            if (result == 0)
            {
                for (int i = 0; i < Items[0].Items.Count - 2; ++i)
                {
                    MenuItem mi = (MenuItem)Items[0].Items[i];
                    mi.Icon = mi.CommandParameter.ToString() == adapterId ? "\xE001" : "";
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

    }

}
