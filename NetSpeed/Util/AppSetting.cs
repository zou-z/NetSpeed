using NetSpeed.DataType;
using NetSpeed.Model;

namespace NetSpeed.Util
{
    internal class AppSetting : AppSettingBase<AppConfig>
    {
        public static string AdapterId
        {
            get => Instance.AdapterId;
            set
            {
                Instance.AdapterId = value;
                SaveInstance(Instance);
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
            if(RefreshInterval == 0)
            {
                RefreshInterval = RefreshIntervals.Interval_1000;
            }
        }
    }
}
