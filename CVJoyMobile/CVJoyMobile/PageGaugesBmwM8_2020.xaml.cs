using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CVJoyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGaugesBmwM8_2020 : ContentPage
    {
        Gauge rpmGauge;
        Gauge speedGauge;

        public PageGaugesBmwM8_2020()
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
                double pedalsHeight = linePedals.Height;
                clutch.HeightRequest = udpReceiver.Info.clutch * pedalsHeight;
                brake.HeightRequest = udpReceiver.Info.brake * pedalsHeight;
                accel.HeightRequest = udpReceiver.Info.accel * pedalsHeight;
                //double turboWidth = lineTurbo.Width;
                //turbo.WidthRequest = udpReceiver.TurboPercent() * turboWidth;

                if (extra)
                {
                    //turboMax.Text = ((Single)udpReceiver.InfoExtra.turboMax).ToString("0.0");
                    lbDistance.Text = ((Single)udpReceiver.InfoExtra.DistanceTraveled).ToString("0.0");
                    //Lap.Text = (udpReceiver.InfoExtra.CompletedLaps + 1).ToString() + " / " + udpReceiver.InfoExtra.NumberOfLaps.ToString();
                    if (udpReceiver.InfoExtra.FuelAvg == 0)
                    {
                        lbFuelKMs.Text = "- KMs";
                        //lbFuelAvg.Text = "-";
                    }
                    else
                    {
                        lbFuelKMs.Text = ((Single)udpReceiver.InfoExtra.Fuel / udpReceiver.InfoExtra.FuelAvg * 100).ToString("0") +" KMs";
                        //lbFuelAvg.Text = (udpReceiver.InfoExtra.FuelAvg).ToString(udpReceiver.InfoExtra.FuelAvg < 10 ? "0.0" : "0");
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
                Color.White,
                7,
                -143, 0, Gauge.enumGaugeRadiusSize.Fit,
                Color.White);
        }

        private void rpmAbsolute_SizeChanged(object sender, EventArgs e)
        {
            rpmGauge.Init(0, 9999, 8000,
                Color.Transparent,
                Color.Transparent,
                Color.Transparent,
                Color.White,
                7,
                135, 0, Gauge.enumGaugeRadiusSize.Fit,
                Color.White);
        }

    }
}