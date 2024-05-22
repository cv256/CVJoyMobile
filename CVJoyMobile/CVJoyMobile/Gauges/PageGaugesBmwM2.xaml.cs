using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CVJoyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGaugesBmwM2 : ContentPage
    {
        Gauge rpmGauge;
        Gauge speedGauge;
        Gauge lkmGauge;
        Pedals pedals;

        public PageGaugesBmwM2()
        {
            InitializeComponent();

            speedGauge = new Gauge(speedAbsolute, 120);
            rpmGauge = new Gauge(rpmAbsolute);
            lkmGauge = new Gauge(lkmAbsolute);
            pedals = new Pedals(gridPedals);

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
                pedals.SetValues(udpReceiver.Info);
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
                        lkmGauge.needleValue(0);
                    }
                    else
                    {
                        //FuelKMs.Text = ((Single)udpReceiver.InfoExtra.Fuel / udpReceiver.InfoExtra.FuelAvg * 100).ToString("0");
                        lkmGauge.needleValue(Math.Min((int)udpReceiver.InfoExtra.FuelAvg,32));
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
            speedGauge.Init(0, 9999, 300,
                Color.Transparent,
                Color.Transparent,
                Color.Transparent,
                Color.Red,
                10,
                -123, 121, Gauge.enumGaugeRadiusSize.Fit,
                Color.Red);
        }

        private void rpmAbsolute_SizeChanged(object sender, EventArgs e)
        {
            rpmGauge.Init(0, 9999, 9000,
                Color.Transparent,
                Color.Transparent,
                Color.Transparent,
                Color.Red,
                10,
                -103, 133, Gauge.enumGaugeRadiusSize.Fit,
                Color.Red);
        }

        private void lkmAbsolute_SizeChanged(object sender, EventArgs e)
        {
            lkmGauge.Init(0, 9999, 32,
                Color.Transparent,
                Color.Transparent,
                Color.Transparent,
                Color.Red,
                8,
                210, 113, Gauge.enumGaugeRadiusSize.Fit,
                Color.Red);
        }

    }
}