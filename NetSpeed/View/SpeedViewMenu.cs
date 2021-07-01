using NetSpeed.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NetSpeed.View
{
    internal class SpeedViewMenu : ContextMenu
    {
        public IEnumerable<Control> AdapterListMenu
        {
            get => (IEnumerable<Control>)GetValue(AdapterListMenuProperty);
            set => SetValue(AdapterListMenuProperty, value);
        }

        public IEnumerable<MenuItem> RefreshIntervalMenu
        {
            get => (IEnumerable<MenuItem>)GetValue(RefreshIntervalMenuProperty);
            set => SetValue(RefreshIntervalMenuProperty, value);
        }


        public static readonly DependencyProperty AdapterListMenuProperty = DependencyProperty.Register("AdapterListMenu", typeof(IEnumerable<Control>), typeof(SpeedViewMenu));
        public static readonly DependencyProperty RefreshIntervalMenuProperty = DependencyProperty.Register("RefreshIntervalMenu", typeof(IEnumerable<MenuItem>), typeof(SpeedViewMenu));


        public SpeedViewMenu()
        {
            Resources = new ResourceDictionary { Source = new Uri("pack://Application:,,,/NetSpeed;component/Style/ContextMenu.xaml") };
            Style = (Style)FindResource("DefaultContextMenuStyle");
            InitMenu();
        }

        private void InitMenu()
        {
            _ = SetBinding(AdapterListMenuProperty, new Binding { Source = VMLocator.VMSpeedViewMenu, Path = new PropertyPath("AdapterListMenu"), Mode = BindingMode.OneWay });
            _ = SetBinding(RefreshIntervalMenuProperty, new Binding { Source = VMLocator.VMSpeedViewMenu, Path = new PropertyPath("RefreshIntervalMenu"), Mode = BindingMode.OneWay });
            ItemsSource = new List<MenuItem>
            {
                new MenuItem { Header = "网络适配器", Icon = "\xEDA3", ItemsSource = AdapterListMenu },
                new MenuItem { Header = "刷新间隔", Icon = "", ItemsSource = RefreshIntervalMenu },
                new MenuItem { Header = "文本颜色", Icon = "" },
                new MenuItem { Header = "关于", Icon = "\xE946" }
            };
        }
    }
}
