using CSDeskBand;
using NetSpeed.Util;
using NetSpeed.View;
using NetSpeed.ViewModel;
using System;
using System.Diagnostics;
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
                var size = GetFinalSize(speedView.Width, speedView.Height);
                Options.MinHorizontalSize = new DeskBandSize((int)(size.Width + 0.5), (int)(size.Height + 0.5));
            }
            catch (Exception e)
            {
                _ = MessageBox.Show($"[NetSpeed]\r\n{e.Message}");
            }
        }

        protected override void DeskbandOnClosed()
        {
            VMLocator.VMSpeedView.Close();
        }

        private Size GetFinalSize(double width, double height)
        {
            int[] dpi = ScreenUtil.GetDpi();
            double scaleX = dpi[0] / 96d;
            double scaleY = dpi[1] / 96d;
            Debug.WriteLine($"[NetSpeed] DpiX = {dpi[0]}, DpiY = {dpi[1]}, ScaleX = {scaleX}, ScaleY = {scaleY}");
            return new Size(width * scaleX, height * scaleY);
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
