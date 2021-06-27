using System.Net.NetworkInformation;

namespace NetSpeed.Util
{
    internal static class NetworkInterfaceExtend
    {
        /// <summary>
        /// 获取适配器的详细信息
        /// </summary>
        /// <param name="networkInterface">
        /// 适配器
        /// </param>
        /// <returns>
        /// 适配器的详细信息
        /// </returns>
        public static string GetDetail(this NetworkInterface ni)
        {
            return $"名称：{ni.Name}\r\n" +
                $"描述：{ni.Description}\r\n" +
                $"类型：{GetNetworkInterfaceType(ni.NetworkInterfaceType)}\r\n" +
                $"状态：{GetOperationalStatus(ni.OperationalStatus)}";
        }

        private static string GetNetworkInterfaceType(NetworkInterfaceType type)
        {
            switch (type)
            {
                case NetworkInterfaceType.Unknown: return "未知";
                case NetworkInterfaceType.Ethernet: return "以太网";
                case NetworkInterfaceType.TokenRing: return "令牌环网";
                case NetworkInterfaceType.Fddi: return "光纤分布式数据接口 (FDDI) ";
                case NetworkInterfaceType.BasicIsdn: return "基本速率接口综合业务数字网 (ISDN) ";
                case NetworkInterfaceType.PrimaryIsdn: return "主速率接口综合业务数字网 (ISDN) ";
                case NetworkInterfaceType.Ppp: return "点对点协议 (PPP) ";
                case NetworkInterfaceType.Loopback: return "环回适配器";
                case NetworkInterfaceType.Ethernet3Megabit: return "以太网 3 兆位/秒";
                case NetworkInterfaceType.Slip: return "串行线路 Internet 协议 (SLIP) ";
                case NetworkInterfaceType.Atm: return "异步传输模式 (ATM) ";
                case NetworkInterfaceType.GenericModem: return "调制解调器";
                case NetworkInterfaceType.FastEthernetT: return "快速以太网 (双绞线) ";
                case NetworkInterfaceType.Isdn: return "ISDN 和 X.25 协议配置的连接";
                case NetworkInterfaceType.FastEthernetFx: return "快速以太网 (光纤) ";
                case NetworkInterfaceType.Wireless80211: return "无线 LAN 连接";
                case NetworkInterfaceType.AsymmetricDsl: return "非对称数字用户线 (ADSL) ";
                case NetworkInterfaceType.RateAdaptDsl: return "速率自适应数字用户线 (RADSL) ";
                case NetworkInterfaceType.SymmetricDsl: return "对称数字用户线 (SDSL) ";
                case NetworkInterfaceType.VeryHighSpeedDsl: return "超高速数字用户线 (VDSL) ";
                case NetworkInterfaceType.IPOverAtm: return "Internet 协议 (IP) 与异步传输模式 (ATM) 结合使用";
                case NetworkInterfaceType.GigabitEthernet: return "千兆以太网";
                case NetworkInterfaceType.Tunnel: return "隧道连接";
                case NetworkInterfaceType.MultiRateSymmetricDsl: return "多速率数字用户线";
                case NetworkInterfaceType.HighPerformanceSerialBus: return "高性能串行总线";
                case NetworkInterfaceType.Wman: return "WiMax 设备使用移动宽带接口";
                case NetworkInterfaceType.Wwanpp: return "基于 GSM 的设备使用移动宽带接口";
                case NetworkInterfaceType.Wwanpp2: return "基于 CDMA 的设备使用移动宽带接口";
                default: return "未知";
            }
        }

        private static string GetOperationalStatus(OperationalStatus status)
        {
            switch (status)
            {
                case OperationalStatus.Up: return "已运行，可以传输数据包";
                case OperationalStatus.Down: return "无法传输数据包";
                case OperationalStatus.Testing: return "正在运行测试";
                case OperationalStatus.Unknown: return "未知";
                case OperationalStatus.Dormant: return "不处于传输数据包的状态；正等待外部事件";
                case OperationalStatus.NotPresent: return "由于缺少组件（通常为硬件组件），无法传输数据包";
                case OperationalStatus.LowerLayerDown: return "无法传输数据包，因为它运行在一个或多个其他接口之上，而这些“低层”接口中至少有一个已关闭";
                default: return "未知";
            }
        }
    }
}
