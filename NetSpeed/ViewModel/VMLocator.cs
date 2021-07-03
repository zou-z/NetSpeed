using NetSpeed.Util;

namespace NetSpeed.ViewModel
{
    internal static class VMLocator
    {
        public static VMSpeedView VMSpeedView { get; private set; }

        public static VMSpeedViewMenu VMSpeedViewMenu { get; private set; }

        /* 此静态类的静态构造函数在主类NetSpeed的构造函数调用之后才调用。
         * 因此不能用静态构造函数去实例化属性，
         * 需要在NetSpeed初始化之前通过调用静态方法实例化属性
         */
        public static void Init()
        {
            AppSetting.Init();
            VMSpeedView = new VMSpeedView();
            VMSpeedViewMenu = new VMSpeedViewMenu();
            VMSpeedView.Inject(VMSpeedViewMenu);
        }
    }
}
