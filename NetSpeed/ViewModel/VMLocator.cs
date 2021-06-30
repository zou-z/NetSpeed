using NetSpeed.Util;

namespace NetSpeed.ViewModel
{
    internal static class VMLocator
    {
        public static VMSpeedView VMSpeedView { get; private set; }

        public static VMSpeedViewMenu VMSpeedViewMenu { get; private set; }

        static VMLocator()
        {
            AppSetting.Init();
            VMSpeedView = new VMSpeedView();
            VMSpeedViewMenu = new VMSpeedViewMenu();
        }
    }
}
