using System;
using Xamarin.Forms;

namespace CVJoyMobile
{
    public partial class PageDigitalPhone : ContentPage
    {
        public PageDigitalPhone() // just for the designer preview
        {
            InitializeComponent();
            (Application.Current as CVJoyMobile.App).udpReceiver.Updated += UdpReceiver_Updated;
        }

        private void UdpReceiver_Updated(object sender, EventArgs e)
        {
            BaseUdpReceiver udpReceiver = (BaseUdpReceiver)sender;
            Device.BeginInvokeOnMainThread(() =>
            {
                this.BatchBegin();
                speed.Text = udpReceiver.Info.speed.ToString();
                gear.Text = udpReceiver.Info.gear;
                slipFL.Color = udpReceiver.Info.slipFL;
                slipFR.Color = udpReceiver.Info.slipFR;
                slipRL.Color = udpReceiver.Info.slipRL;
                slipRR.Color = udpReceiver.Info.slipRR;
                dirtFL.Color = udpReceiver.Info.dirtFL;
                dirtFR.Color = udpReceiver.Info.dirtFR;
                dirtRL.Color = udpReceiver.Info.dirtRL;
                dirtRR.Color = udpReceiver.Info.dirtRR;
                rpm.WidthRequest = udpReceiver.RpmPercent() * lineWidth.Width;
                rpm.Color = udpReceiver.RpmColor();
                rpmText.Text = udpReceiver.Info.rpm.ToString();
                gearAuto.Text = udpReceiver.Info.gearAuto ? "Gear Auto" : "Gear Manual";
                double pedalsHeight = linePedals.Height;
                clutch.HeightRequest = udpReceiver.Info.clutch * pedalsHeight;
                brake.HeightRequest = udpReceiver.Info.brake * pedalsHeight;
                accel.HeightRequest = udpReceiver.Info.accel * pedalsHeight;
                Distance.Text = ((Single)udpReceiver.InfoExtra.DistanceTraveled ).ToString("0.0");
                Lap.Text = (udpReceiver.InfoExtra.CompletedLaps + 1).ToString() + " / " + udpReceiver.InfoExtra.NumberOfLaps.ToString();
                if (udpReceiver.InfoExtra.FuelAvg == 0)
                {
                    FuelKMs.Text = "-";
                    FuelAvg.Text = "-";
                }
                else
                {
                    FuelKMs.Text = ((Single)udpReceiver.InfoExtra.Fuel / udpReceiver.InfoExtra.FuelAvg * 100).ToString("0");
                    FuelAvg.Text = (udpReceiver.InfoExtra.FuelAvg).ToString(udpReceiver.InfoExtra.FuelAvg < 10 ? "0.0" : "0");
                }

                this.BatchCommit();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            (Application.Current as CVJoyMobile.App).udpReceiver.Updated -= UdpReceiver_Updated;
            Application.Current.MainPage = new PageDigital();
        }
    }
}
