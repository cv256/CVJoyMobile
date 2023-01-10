using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CVJoyMobile
{
    public partial class App : Application
    {

        public BaseUdpReceiver udpReceiver;

        public App()
        {
            InitializeComponent();
            udpReceiver = new BaseUdpReceiver();
            //udpReceiver.Start(); 
            udpReceiver.StartDebug();

            MainPage = new PageDigital();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
