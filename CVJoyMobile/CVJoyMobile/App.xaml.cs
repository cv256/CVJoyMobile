using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CVJoyMobile
{

    public delegate void DebugPrintsHandler(string debugPrints);

    public partial class App : Application
    {

        public event DebugPrintsHandler DebugPrintsUpdated;

        public BaseUdpReceiver udpReceiver;
        public string DebugPrints = "";

        public App()
        {
            InitializeComponent();
            udpReceiver = new BaseUdpReceiver();
            udpReceiver.Start(); //            udpReceiver.StartDebug();

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

        public void DebugPrint(string textToAdd)
        {
            DebugPrints = DateTime.Now.ToString("mm:ss:ff") + "  " + textToAdd + Environment.NewLine + DebugPrints;
            if (DebugPrints.Length > 5000) DebugPrints = DebugPrints.Substring(4000);

            DebugPrintsUpdated?.Invoke(DebugPrints);
        }

        public async void AskForPage()
        {
            string action = await Application.Current.MainPage.DisplayActionSheet("", "Cancel", null, "Digital", "Diesel", "Fast", "Debug");
            switch (action)
            {
                case "Digital":
                    Application.Current.MainPage = new PageDigital();
                    break;
                case "Diesel":
                    Application.Current.MainPage = new PageGauges();
                    break;
                case "Fast":
                    Application.Current.MainPage = new PageGaugesFast();
                    break;
                case "Debug":
                    Application.Current.MainPage = new PageDebug();
                    break;
            }
        }
    }
}
