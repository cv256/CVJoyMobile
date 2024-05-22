using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CVJoyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGaugesFordFocus_2015d : ContentPage
    {
        Gauge rpmGauge;
        Gauge speedGauge;

        public PageGaugesFordFocus_2015d()
        {
            InitializeComponent();

            speedGauge = new Gauge(speedAbsolute, 120);
            rpmGauge = new Gauge(rpmAbsolute);

            (Application.Current as CVJoyMobile.App).udpReceiver.Updated += UdpReceiver_Updated;
        }

        private void UdpReceiver_Updated(BaseUdpReceiver udpReceiver, Boolean extra)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.BatchBegin();
                lbTime.Text = DateTime.Now.ToString("H:mm");

                //slipFL.Color = udpReceiver.Info.slipFL;
                //slipFR.Color = udpReceiver.Info.slipFR;
                //slipRL.Color = udpReceiver.Info.slipRL;
                //slipRR.Color = udpReceiver.Info.slipRR;
                //speedText.Text = udpReceiver.Info.speed.ToString();
                speedGauge.needleValue(udpReceiver.Info.speed);
                gear.Text = udpReceiver.Info.gear;
                //rpm.WidthRequest = udpReceiver.RpmPercent() * horizLine1.Width;
                //rpm.Color = udpReceiver.RpmColor();
                rpmGauge.needleValue(udpReceiver.Info.rpm);
                //rpmText.Text = udpReceiver.Info.rpm.ToString();
                lbGearAuto.Text = udpReceiver.Info.gearAuto ? "Auto" : "Manual";
                //double turboWidth = lineTurbo.Width;
                //turbo.WidthRequest = udpReceiver.TurboPercent() * turboWidth;

                if (extra)
                {
                    //turboMax.Text = ((Single)udpReceiver.InfoExtra.turboMax).ToString("0.0");
                    lbDistance.Text = ((Single)udpReceiver.InfoExtra.DistanceTraveled).ToString("0.0 KMs");
                    //Lap.Text = (udpReceiver.InfoExtra.CompletedLaps + 1).ToString() + " / " + udpReceiver.InfoExtra.NumberOfLaps.ToString();
                    if (udpReceiver.InfoExtra.FuelAvg == 0)
                    {
                        //FuelKMs.Text = "-";
                        //lkmGauge.needleValue(0);
                    }
                    else
                    {
                        //FuelKMs.Text = ((Single)udpReceiver.InfoExtra.Fuel / udpReceiver.InfoExtra.FuelAvg * 100).ToString("0");
                        //lkmGauge.needleValue(Math.Min((int)udpReceiver.InfoExtra.FuelAvg,32));
                    }
                }

                this.BatchCommit();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            (Application.Current as CVJoyMobile.App).udpReceiver.Updated -= UdpReceiver_Updated;
            (Application.Current as CVJoyMobile.App).AskForPage();
        }

        private void speedAbsolute_SizeChanged(object sender, EventArgs e)
        {
            speedGauge.Init(0, 9999, 240,
                Color.White,
                Color.White,
                Color.Transparent,
                Color.Cyan,
                8,
                -130, 133, Gauge.enumGaugeRadiusSize.Fit,
                Color.DarkCyan);
        }

        private void rpmAbsolute_SizeChanged(object sender, EventArgs e)
        {
            rpmGauge.Init(0, 4800, 6000,
                Color.White,
                Color.White,
                Color.White,
                Color.Cyan,
                8,
                -134, 132, Gauge.enumGaugeRadiusSize.Fit,
                Color.DarkCyan);
        }

    }
}