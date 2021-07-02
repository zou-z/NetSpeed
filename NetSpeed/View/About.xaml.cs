using System.Diagnostics;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;

namespace NetSpeed.View
{
    internal partial class About : MenuItem
    {
        private const string url = "https://github.com/zou-z/NetSpeed/releases";
        private string version;

        public string Version => version ?? (version = Assembly.GetExecutingAssembly().GetName().Version.ToString());

        public string UrlTip => $"前往 {url} 下载最新版本";

        public About()
        {
            InitializeComponent();
        }

        private void OpenUrl_Click(object sender, MouseButtonEventArgs e)
        {
            _ = Process.Start(url);
        }
    }
}
