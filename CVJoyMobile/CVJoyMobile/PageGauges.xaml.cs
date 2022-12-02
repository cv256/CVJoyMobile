using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CVJoyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGauges : ContentPage
    {
        Gauge rpmGauge;
        Gauge speedGauge;

        public PageGauges()
        {
            InitializeComponent();

            rpmGauge = new Gauge(rpmAbsolute);
            speedGauge = new Gauge(speedAbsolute);

            (Application.Current as CVJoyMobile.App).udpReceiver.Updated += UdpReceiver_Updated;
        }

        private void UdpReceiver_Updated(object sender, EventArgs e)
        {
            BaseUdpReceiver udpReceiver = (BaseUdpReceiver)sender;
            Device.BeginInvokeOnMainThread(() =>
            {
                this.BatchBegin();
                slipFL.Color = udpReceiver.Info.slipFL;
                slipFR.Color = udpReceiver.Info.slipFR;
                slipRL.Color = udpReceiver.Info.slipRL;
                slipRR.Color = udpReceiver.Info.slipRR;
                speedText.Text = udpReceiver.Info.speed.ToString();
                speedGauge.needleValue(udpReceiver.Info.speed);
                gear.Text = udpReceiver.Info.gear;
                //rpm.WidthRequest = udpReceiver.RpmPercent() * horizLine1.Width;
                //rpm.Color = udpReceiver.RpmColor();
                rpmGauge.needleValue(udpReceiver.Info.rpm);
                rpmText.Text = udpReceiver.Info.rpm.ToString();
                gearAuto.Text = udpReceiver.Info.gearAuto ? "Auto" : "Manual";
                double pedalsHeight = linePedals.Height;
                clutch.HeightRequest = udpReceiver.Info.clutch * pedalsHeight;
                brake.HeightRequest = udpReceiver.Info.brake * pedalsHeight;
                accel.HeightRequest = udpReceiver.Info.accel * pedalsHeight;
                Distance.Text = ((Single)udpReceiver.InfoExtra.DistanceTraveled).ToString("0.0");
                Lap.Text = (udpReceiver.InfoExtra.CompletedLaps + 1).ToString() + " / " + udpReceiver.InfoExtra.NumberOfLaps.ToString();
                if (udpReceiver.InfoExtra.FuelAvg == 0)
                {
                    FuelKMs.Text = "-";
                    FuelAvg.Text = "-";
                }
                else
                {
                    FuelKMs.Text = ((Single)udpReceiver.InfoExtra.Fuel / udpReceiver.InfoExtra.FuelAvg * 10).ToString("0");
                    FuelAvg.Text = (udpReceiver.InfoExtra.FuelAvg).ToString(udpReceiver.InfoExtra.FuelAvg < 10 ? "0.0" : "0");
                }
                this.BatchCommit();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            (Application.Current as CVJoyMobile.App).udpReceiver.Updated -= UdpReceiver_Updated;
            Application.Current.MainPage = new PageGaugesFast();
        }

        private void rpmAbsolute_SizeChanged(object sender, EventArgs e)
        {
            rpmGauge.Init(0, 5000, 6000,
                Color.FromHsla((double)215 / 360, 1, .30),
                Color.FromHsla((double)215 / 360, 1, .55),
                Color.FromHsla((double)215 / 360, 1, .80),
                Color.OrangeRed,
                7,
                225, 495, Gauge.enumRadiusSize.Fit);
        }
        private void speedAbsolute_SizeChanged(object sender, EventArgs e)
        {
            speedGauge.Init(0, 240, 240,
                Color.FromHsla((double)215 / 360, 1, .30),
                Color.FromHsla((double)215 / 360, 1, .55),
                Color.FromHsla((double)215 / 360, 1, .80), Color.OrangeRed,
                7,
                225, 495, Gauge.enumRadiusSize.Fit);
        }
    }
}