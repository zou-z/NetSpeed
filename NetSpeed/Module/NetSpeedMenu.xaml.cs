using System.Net.NetworkInformation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetSpeed.Module
{
    public partial class NetSpeedMenu : ContextMenu
    {
        private readonly ColorSelector colorSelector = new ColorSelector();

        public delegate void SetSelectedAdapterDelegate(string id);
        public delegate void SetTextColorDelegate(Brush color);
        public event SetSelectedAdapterDelegate SetSelectedAdapter;
        public event SetTextColorDelegate SetTextColor;

        public NetSpeedMenu()
        {
            InitializeComponent();
            (Items[1] as MenuItem).Items.Add(colorSelector);
            (Items[2] as MenuItem).Items.Add(new About());
            colorSelector.SetTextColor += (Brush color) =>
            {
                SetTextColor(color);
            };
        }

        public void SetAdapterList(NetworkInterface[] adapters, NetworkInterface selectedAdapter)
        {
            foreach (NetworkInterface ni in adapters)
            {
                MenuItem mi = new MenuItem
                {
                    Header = ni.Description,
                    Icon = ni.Equals(selectedAdapter) ? "\xE001" : "",
                    Tag = ni.Id
                };
                mi.Click += MenuItem_Click;
                AdapterList.Items.Add(mi);
            }
            AdapterList.IsEnabled = true;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            string id = (sender as MenuItem).Tag.ToString();
            SetSelectedAdapter(id);
            foreach (MenuItem mi in AdapterList.Items)
            {
                mi.Icon = mi.Tag.ToString() == id ? "\xE001" : "";
            }
        }
    }
}
