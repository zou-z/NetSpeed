using NetSpeed.DataType;
using NetSpeed.Model;
using System.Net.NetworkInformation;

namespace NetSpeed.Util
{
    internal sealed class AppSetting : AppSettingBase<AppConfig>
    {
        private static NetworkInterface selectedAdapter;

        public static NetworkInterface[] AdapterList { get; private set; }

        public static NetworkInterface SelectedAdapter
        {
            get => selectedAdapter;
            set
            {
                Instance.AdapterId = value.Id;
                SaveInstance(Instance);
                selectedAdapter = value;
            }
        }

        public static int RefreshInterval
        {
            get => Instance.RefreshInterval;
            set
            {
                Instance.RefreshInterval = value;
                SaveInstance(Instance);
            }
        }

        public static void Init()
        {
            SettingFilePath = "NetSpeed.json";
            Instance = LoadInstance();
            InitSetting();
        }

        private static void InitSetting()
        {
            // 初始化适配器列表
            AdapterList = NetworkInterface.GetAllNetworkInterfaces();
            // 初始化已选择或默认适配器（已选择的不存在时选默认的）
            SetSelectedOrDefaultAdapter();
            // 刷新间隔
            RefreshInterval = RefreshInterval == 0 ? RefreshIntervals.Default : RefreshInterval;
            // 字体颜色

        }

        private static void SetSelectedOrDefaultAdapter()
        {
            NetworkInterface defaultAdapter = null;
            for (int i = 0; i < AdapterList.Length; ++i)
            {
                if (AdapterList[i].Id == Instance.AdapterId)
                {
                    SelectedAdapter = AdapterList[i];
                    return;
                }
                else if (AdapterList[i].OperationalStatus == OperationalStatus.Up &&
                    AdapterList[i].NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    defaultAdapter == null)
                {
                    defaultAdapter = AdapterList[i];
                }
            }
            SelectedAdapter = defaultAdapter;
        }

        /// <summary>
        /// 更新网络适配器列表
        /// </summary>
        /// <returns>
        ///  true SelectedAdapter改变
        /// false SelectedAdapter不变
        /// </returns>
        public static bool UpdateAdapterList()
        {
            AdapterList = NetworkInterface.GetAllNetworkInterfaces();
            NetworkInterface defaultAdapter = null;
            for (int i = 0; i < AdapterList.Length; ++i)
            {
                if (AdapterList[i].Id == SelectedAdapter.Id)
                {
                    return false;
                }
                else if (AdapterList[i].OperationalStatus == OperationalStatus.Up &&
                  AdapterList[i].NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                  defaultAdapter == null)
                {
                    defaultAdapter = AdapterList[i];
                }
            }
            SelectedAdapter = defaultAdapter;
            return true;
        }
    }
}
