using System;

namespace NetSpeed.Interface
{
    internal interface ISpeedViewMenu
    {
        /// <summary>
        /// 重启定时器事件
        /// </summary>
        event Action RestartTimer;

        /// <summary>
        /// 更新文本颜色事件
        /// </summary>
        event Action UpdateTextColor;
    }
}
