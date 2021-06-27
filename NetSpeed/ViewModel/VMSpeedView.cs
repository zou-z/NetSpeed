using NetSpeed.Interface;
using NetSpeed.Util;

namespace NetSpeed.ViewModel
{
    internal class VMSpeedView : NotifyBase
    {
        private readonly INetInfo netInfo;
        private ISpeedViewMenu speedViewMenu;
        private string uploadSpeedText;
        private string downloadSpeedText;

        public string UploadSpeedText
        {
            get => uploadSpeedText;
            set => Set(ref uploadSpeedText, value);
        }

        public string DownloadSpeedText
        {
            get => downloadSpeedText;
            set => Set(ref downloadSpeedText, value);
        }

        public VMSpeedView()
        {
            uploadSpeedText = "初始化...";
            downloadSpeedText = "初始化...";
            netInfo = new NetInfo();
        }

        public void Inject(ISpeedViewMenu speedViewMenu)
        {
            this.speedViewMenu = speedViewMenu;
            this.speedViewMenu.UpdateAdapterList(netInfo.GetAdapterList(), netInfo.GetAdapter());
            this.speedViewMenu.SelectedAdapter += netInfo.SetAdapter;
        }






    }
}
