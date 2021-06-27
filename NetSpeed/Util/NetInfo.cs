using NetSpeed.DataType;
using NetSpeed.Interface;
using System;
using System.Net.NetworkInformation;
using System.Threading;

namespace NetSpeed.Util
{
    internal class NetInfo : INetInfo
    {
        public event Action<ulong, ulong> UpdateSpeed;
        private NetworkInterface[] adapters;
        private NetworkInterface adapter;
        private Timer timer;
        private bool isRunning = false;

        public NetInfo()
        {
            adapters = NetworkInterface.GetAllNetworkInterfaces();
            if (SetAdapter(AppSetting.AdapterId) > 0)
            {
                SetDefaultAdapter();
            }
        }

        public NetworkInterface GetAdapter()
        {
            return adapter;
        }

        public NetworkInterface[] GetAdapterList(bool isRefreshList = false)
        {
            if (isRefreshList)
            {
                adapters = NetworkInterface.GetAllNetworkInterfaces();

                /*
                 刷新适配器列表
                1.获取适配器列表
                2.如果原先选择的适配器不在了，选择默认的适配器
                 */
            }
            return adapters;
        }

        public bool IsRunning()
        {
            return isRunning;
        }

        public int SetAdapter(string adapterId)
        {
            if (adapterId == null || adapterId == "")
            {
                return 1;
            }
            else if (adapters.Length == 0)
            {
                return 2;
            }
            else
            {
                for (int i = 0; i < adapters.Length; ++i)
                {
                    if (adapters[i].Id == adapterId)
                    {
                        SetAdapter(adapters[i]);
                        return 0;
                    }
                }
            }
            return 3;
        }

        public void SetDefaultAdapter()
        {
            if (adapters.Length == 0)
            {
                return;
            }
            for (int i = 0; i < adapters.Length; ++i)
            {
                if (adapters[i].OperationalStatus == OperationalStatus.Up && adapters[i].NetworkInterfaceType != NetworkInterfaceType.Loopback)
                {
                    SetAdapter(adapters[i]);
                    return;
                }
            }
            SetAdapter(adapters[0]);
        }

        private void SetAdapter(NetworkInterface networkInterface)
        {
            adapter = networkInterface;
            AppSetting.AdapterId = networkInterface.Id;
        }

        public void Start()
        {
            //timer = new Timer(Timer_Tick)

        }

        public void Stop()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }
    }
}
