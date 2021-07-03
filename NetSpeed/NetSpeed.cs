using CSDeskBand;
using NetSpeed.View;
using NetSpeed.ViewModel;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace NetSpeed
{
    [ComVisible(true)]
    [CSDeskBandRegistration(Name = "实时网速")]
    public class NetSpeed : CSDeskBandWpf
    {
        private readonly SpeedView speedView;
        protected override UIElement UIElement => speedView;

        public NetSpeed()
        {
            try
            {
                VMLocator.Init();
                speedView = new SpeedView();
                Options.MinHorizontalSize = new DeskBandSize(75, 40);
            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.Message);
            }
        }

        protected override void DeskbandOnClosed()
        {
            VMLocator.VMSpeedView.Close();
        }
#if DEBUG
        public static FrameworkElement GetSpeedView()
        {
            VMLocator.Init();
            return new SpeedView();
        }
#endif
    }
}
