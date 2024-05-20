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
            string action = await Application.Current.MainPage.DisplayActionSheet("", "Cancel", null, "Generic Digital", "Generic Diesel", "Generic Fast",
                "VW Polo  ( 240 / 5500 )",
                "BMW M2  ( 300 / 8000 )", "BMW M8 2020  ( 330 / 8000 )",
                "Mercedes W124 1990  ( 260 / 7000 )", "Mercedes S 2008  ( 260 / 7000 )", "Mercedes S 2015  ( 260 / 8000 )","Mercedes AMG 2021  ( 360 / 8000 )", 
                "Debug");
            switch (action)
            {
                case "Generic Digital":
                    Application.Current.MainPage = new PageDigital();
                    break;
                case "Generic Diesel":
                    Application.Current.MainPage = new PageGauges();
                    break;
                case "Generic Fast":
                    Application.Current.MainPage = new PageGaugesFast();
                    break;
                case "BMW M2  ( 300 / 8000 )":
                    Application.Current.MainPage = new PageGaugesBmwM2();
                    break;
                case "BMW M8 2020  ( 330 / 8000 )":
                    Application.Current.MainPage = new PageGaugesBmwM8_2020();
                    break;
                case "Mercedes W124 1990  ( 260 / 7000 )":
                    Application.Current.MainPage = new PageGaugesW124();
                    break;
                case "Mercedes S 2008  ( 260 / 7000 )":
                    //Application.Current.MainPage = new PageGaugesMercedes_S_2008();
                    break;
                case "Mercedes S 2015  ( 260 / 8000 )":
                    //Application.Current.MainPage = new PageGaugesMercedes_S_2015();
                    break;
                case "Mercedes AMG 2021  ( 360 / 8000 )":
                    //Application.Current.MainPage = new PageGaugesMercedes_AMG_2021();
                    break;
                case "VW Polo  ( 240 / 5500 )":
                    //Application.Current.MainPage = new PageGaugesVW_Polo();
                    break;
                case "Debug":
                    Application.Current.MainPage = new PageDebug();
                    break;
            }
        }
    }
}
