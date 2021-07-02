using CSDeskBand;
using NetSpeed.View;
using System;
using System.Runtime.InteropServices;
using System.Windows;

namespace NetSpeed
{
    [ComVisible(true)]
    [CSDeskBandRegistration(Name = "实时网速")]
    public class NetSpeed : CSDeskBandWpf
    {
        //private readonly MainView mainView;
        private readonly SpeedView speedView;
        protected override UIElement UIElement => speedView;

        public NetSpeed()
        {
            try
            {
                speedView = new SpeedView();
                Options.MinHorizontalSize = new DeskBandSize(75, 40);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            Window window = new Window
            {
                Content = new System.Windows.Controls.TextBlock
                {
                    Text = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                }
            };
            window.Show();
        }

        protected override void DeskbandOnClosed()
        {
            //speedView.ReleaseResources();


        }

        public static readonly object SpeedView = new SpeedView();
    }
}
