using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace NetSpeed.Module
{
    public partial class About : MenuItem
    {
        public About()
        {
            InitializeComponent();
        }

        private void TextBlock_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Process.Start("https://github.com/zou-z/NetSpeed");
        }
    }
}
