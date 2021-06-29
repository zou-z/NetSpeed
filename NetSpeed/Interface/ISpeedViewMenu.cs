using System;
using System.Net.NetworkInformation;

namespace NetSpeed.Interface
{
    internal interface ISpeedViewMenu
    {
        /// <summary>
        /// 选择网络适配器事件
        /// </summary>
        event Func<string, int> SelectedAdapter;

        /// <summary>
        /// 刷新网络适配器列表事件
        /// </summary>
        event Action RefreshedAdapterList;

        /// <summary>
        /// 重启定时器事件
        /// </summary>
        event Action RestartedTimer;

        /// <summary>
        /// 更新网络适配器列表
        /// </summary>
        /// <param name="adapters">
        /// 网络适配器列表
        /// </param>
        /// <param name="adapter">
        /// 已选择的网络适配器
        /// </param>
        void UpdateAdapterList(NetworkInterface[] adapters, NetworkInterface adapter);
    }
}
