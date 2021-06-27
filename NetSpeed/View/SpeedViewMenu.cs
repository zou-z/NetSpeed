using NetSpeed.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace NetSpeed.View
{
    internal class SpeedViewMenu : ContextMenu
    {
        public SpeedViewMenu()
        {
            Resources = new ResourceDictionary { Source = new Uri("pack://Application:,,,/NetSpeed;component/Style/ContextMenu.xaml") };
            Style = (Style)FindResource("DefaultContextMenuStyle");

            _ = SetBinding(ItemsSourceProperty, new Binding
            {
                Source = VMLocator.VMSpeedViewMenu,
                Path = new PropertyPath("Items")
            });
        }
    }
}
