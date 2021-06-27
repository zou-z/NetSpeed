using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace NetSpeed.Interface
{
    internal interface INetInfo
    {
        /// <summary>
        /// 更新上下行网速
        /// </summary>
        event Action<ulong, ulong> UpdateSpeed;

        /// <summary>
        /// 获取适配器列表
        /// </summary>
        /// <param name="isRefreshList">
        /// 在获取前是否刷新列表
        /// </param>
        /// <returns>
        /// 适配器列表
        /// </returns>
        NetworkInterface[] GetAdapterList(bool isRefreshList = false);

        /// <summary>
        /// 选择适配器
        /// </summary>
        /// <param name="adapterId">
        /// 适配器ID (GUID)
        /// </param>
        /// <returns>
        /// 0 成功
        /// 1 目标网络适配器为空
        /// 2 网络适配器列表为空
        /// 3 网络适配器列表中未找到目标网络适配器
        /// </returns>
        int SetAdapter(string adapterId);

        /// <summary>
        /// 选择默认的适配器
        /// </summary>
        void SetDefaultAdapter();

        /// <summary>
        /// 获取已选择的适配器
        /// </summary>
        /// <returns>
        /// 已选择的适配器
        /// </returns>
        NetworkInterface GetAdapter();

        /// <summary>
        /// 是否正在运行
        /// </summary>
        /// <returns>
        /// true  正在运行
        /// false 已停止
        /// </returns>
        bool IsRunning();

        /// <summary>
        /// 开始获取上下行网速
        /// </summary>
        void Start();

        /// <summary>
        /// 停止获取上下行网速
        /// </summary>
        void Stop();

        /// <summary>
        /// 结束运行，释放资源
        /// </summary>
        void Close();
    }
}
