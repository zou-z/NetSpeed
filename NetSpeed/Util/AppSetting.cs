using NetSpeed.Model;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text.RegularExpressions;

namespace NetSpeed.Util
{
    internal sealed class AppSetting : AppSettingBase<AppConfig>
    {
        private static NetworkInterface selectedAdapter;
        public static readonly string DefaultTextColor = "#FFFFFF";

        public struct RefreshIntervals
        {
            public const int Interval_1000 = 1000;
            public const int Interval_500 = 500;
            public const int Interval_200 = 200;
            public const int Interval_100 = 100;

            public const int Default = Interval_1000;

            public static int[] GetValues()
            {
                return new int[]
                {
                    Interval_1000,
                    Interval_500,
                    Interval_200,
                    Interval_100
                };
            }
        }

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

        public static string TextColor
        {
            get => Instance.TextColor;
            set
            {
                Instance.TextColor = value;
                SaveInstance(Instance);
            }
        }

        public static void Init()
        {
            SettingFilePath = $@"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}\NetSpeed.json";
            try
            {
                Instance = LoadInstance();
            }
            catch
#if DEBUG
            (System.Exception ex)
#endif
            {
#if DEBUG
                System.Windows.MessageBox.Show(ex.Message);
#endif
                Instance = new AppConfig();
            }
            InitSetting();
        }

        private static void InitSetting()
        {
            AdapterList = NetworkInterface.GetAllNetworkInterfaces();
            SetSelectedOrDefaultAdapter();
            RefreshInterval = RefreshInterval == 0 ? RefreshIntervals.Default : RefreshInterval;
            SetSelectedOrDefaultTextColor();
        }

        /// <summary>
        /// 设置上次选择的网络设配器，如果没有则选择默认的网络适配器
        /// </summary>
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
        /// 设置上次选择的文本颜色，如果为null或值不正确则选择默认的文本颜色
        /// </summary>
        private static void SetSelectedOrDefaultTextColor()
        {
            if (TextColor == null || TextColor.Length != 7 || TextColor[0] != '#' || !Regex.IsMatch(TextColor.Substring(1), "^[0-9a-fA-F]*$"))
            {
                TextColor = DefaultTextColor;
                return;
            }
        }

        /// <summary>
        /// 更新网络适配器列表（如果已选择的网络适配器不在新的列表中则选择默认的网络适配器）
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
