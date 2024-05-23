using System;
using Xamarin.Forms;

namespace CVJoyMobile
{
    public partial class PageDigital : ContentPage
    {
        Pedals pedals;

        public PageDigital() // just for the designer preview
        {
            InitializeComponent();

            pedals = new Pedals(gridPedals, true);

            (Application.Current as CVJoyMobile.App).udpReceiver.Updated += UdpReceiver_Updated;
        }

        private void UdpReceiver_Updated(BaseUdpReceiver udpReceiver, Boolean extra)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.BatchBegin();
                lbTime.Text = DateTime.Now.ToString("H:mm");

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
                rpm.WidthRequest = udpReceiver.RpmPercent() * GridTires.Width;
                rpm.Color = udpReceiver.RpmColor();
                rpmText.Text = udpReceiver.Info.rpm.ToString("#,###");
                gearAuto.Text = udpReceiver.Info.gearAuto ? "Gear Auto" : "Gear Manual";
                pedals.SetValues(udpReceiver);

                if (extra)
                {
                    Distance.Text = ((Single)udpReceiver.InfoExtra.DistanceTraveled).ToString("0.0");
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
                }

                this.BatchCommit();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            (Application.Current as CVJoyMobile.App).udpReceiver.Updated -= UdpReceiver_Updated;
            (Application.Current as CVJoyMobile.App).AskForPage();
        }
    }
}
