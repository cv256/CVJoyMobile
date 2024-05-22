using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CVJoyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGaugesW124 : ContentPage
    {
        Gauge rpmGauge;
        Gauge speedGauge;

        public PageGaugesW124()
        {
            InitializeComponent();

            speedGauge = new Gauge(speedAbsolute);
            rpmGauge = new Gauge(rpmAbsolute);

            (Application.Current as CVJoyMobile.App).udpReceiver.Updated += UdpReceiver_Updated;
        }

        private void UdpReceiver_Updated(BaseUdpReceiver udpReceiver, Boolean extra)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.BatchBegin();
                //slipFL.Color = udpReceiver.Info.slipFL;
                //slipFR.Color = udpReceiver.Info.slipFR;
                //slipRL.Color = udpReceiver.Info.slipRL;
                //slipRR.Color = udpReceiver.Info.slipRR;
                //speedText.Text = udpReceiver.Info.speed.ToString();
                speedGauge.needleValue(udpReceiver.Info.speed);
                //gear.Text = udpReceiver.Info.gear;
                //rpm.WidthRequest = udpReceiver.RpmPercent() * horizLine1.Width;
                //rpm.Color = udpReceiver.RpmColor();
                rpmGauge.needleValue(udpReceiver.Info.rpm);
                //rpmText.Text = udpReceiver.Info.rpm.ToString();
                //gearAuto.Text = udpReceiver.Info.gearAuto ? "Auto" : "Manual";
                //double pedalsHeight = linePedals.Height;
                //clutch.HeightRequest = udpReceiver.Info.clutch * pedalsHeight;
                //brake.HeightRequest = udpReceiver.Info.brake * pedalsHeight;
                //accel.HeightRequest = udpReceiver.Info.accel * pedalsHeight;
                //double turboWidth = lineTurbo.Width;
                //turbo.WidthRequest = udpReceiver.TurboPercent() * turboWidth;

                if (extra)
                {
                    //turboMax.Text = ((Single)udpReceiver.InfoExtra.turboMax).ToString("0.0");
                    lbDistance.Text = ((Single)udpReceiver.InfoExtra.DistanceTraveled).ToString("0 0 0 0 0.0");
                    //Lap.Text = (udpReceiver.InfoExtra.CompletedLaps + 1).ToString() + " / " + udpReceiver.InfoExtra.NumberOfLaps.ToString();
                    //if (udpReceiver.InfoExtra.FuelAvg == 0)
                    //{
                    //    FuelKMs.Text = "-";
                    //    FuelAvg.Text = "-";
                    //}
                    //else
                    //{
                    //    FuelKMs.Text = ((Single)udpReceiver.InfoExtra.Fuel / udpReceiver.InfoExtra.FuelAvg * 100).ToString("0");
                    //    FuelAvg.Text = (udpReceiver.InfoExtra.FuelAvg).ToString(udpReceiver.InfoExtra.FuelAvg < 10 ? "0.0" : "0");
                    //}
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
            speedGauge.Init(20, 9999, 260,
                Color.Transparent,
                Color.Transparent,
                Color.Transparent,
                Color.Orange,
                10,
                -133, 135, Gauge.enumGaugeRadiusSize.ExpandStart,
                Color.Black);
        }

        private void rpmAbsolute_SizeChanged(object sender, EventArgs e)
        {
            rpmGauge.Init(0, 9999, 7000,
                Color.Transparent,
                Color.Transparent,
                Color.Transparent,
                Color.Orange,
                10,
                -125, 125, Gauge.enumGaugeRadiusSize.Fit,
                Color.Black);
        }

    }
}