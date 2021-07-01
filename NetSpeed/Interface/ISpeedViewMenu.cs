using System;
using System.Net.NetworkInformation;

namespace NetSpeed.Interface
{
    internal interface ISpeedViewMenu
    {
        /// <summary>
        /// 重启定时器事件
        /// </summary>
        event Action RestartTimer;
    }
}
