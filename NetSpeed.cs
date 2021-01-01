using CSDeskBand;
using NetSpeed.Module;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace NetSpeed
{
    [ComVisible(true)]
    [CSDeskBandRegistration(Name = "实时网速")]
    public class NetSpeed : CSDeskBandWpf
    {
        private readonly MainView mainView;
        protected override UIElement UIElement => mainView;

        public NetSpeed()
        {
            try
            {
                mainView = new MainView();
                Options.MinHorizontalSize = new DeskBandSize(95, 40);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        protected override void DeskbandOnClosed()
        {
            mainView.ReleaseResources();
        }
    }
}
