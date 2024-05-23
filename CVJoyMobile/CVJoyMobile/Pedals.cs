using Xamarin.Forms;
using static BaseUdpReceiver;

namespace CVJoyMobile
{
    public class Pedals
    {
        BoxView boxClutch;
        BoxView boxBreak;
        BoxView boxAccel;
        BoxView boxTurbo;
        BoxView boxSep;

        public Pedals(Grid absoluteLayout, bool showTurbo)
        {
            absoluteLayout.Children.Clear();

            boxSep = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Color = Color.White
            };
            absoluteLayout.Children.Add(boxSep, 1, 0);

            boxSep = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                Color = Color.White
            };
            absoluteLayout.Children.Add(boxSep, 3, 0);

            if (showTurbo)
            {
                boxSep = new BoxView
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill,
                    Color = Color.White
                };
                absoluteLayout.Children.Add(boxSep, 5, 0);
            }

            boxClutch = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.End,
                Color = Color.Blue,
                HeightRequest = 30
            };
            absoluteLayout.Children.Add(boxClutch, 0, 0);

            boxBreak = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.End,
                Color = Color.Red,
                HeightRequest = 30
            };
            absoluteLayout.Children.Add(boxBreak, 2, 0);

            boxAccel = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.End,
                Color = Color.Green,
                HeightRequest = 30
            };
            absoluteLayout.Children.Add(boxAccel, 4, 0);

            if (showTurbo)
            {
                boxTurbo= new BoxView
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.End,
                    Color = Color.Yellow,
                    HeightRequest = 30
                };
                absoluteLayout.Children.Add(boxTurbo, 6, 0);
            }
        }

            public void SetValues(BaseUdpReceiver udpReceiver)
        {
            double pedalsHeight = boxSep.Height;

            if (udpReceiver.Info.clutch == 0)
            {
                boxClutch.HeightRequest = 3;
                boxClutch.Color = Color.White;
            }
            else 
            {
                boxClutch.HeightRequest = udpReceiver.Info.clutch * pedalsHeight;
                boxClutch.Color = udpReceiver.Info.clutch == 1 ? Color.AliceBlue : Color.Blue;
            }

            if (udpReceiver.Info.brake == 0)
            {
                boxBreak.HeightRequest = 3;
                boxBreak.Color = Color.White;
            }
            else
            {
                boxBreak.HeightRequest = udpReceiver.Info.brake * pedalsHeight;
                boxBreak.Color = udpReceiver.Info.brake == 1 ? Color.OrangeRed : Color.Red;
            }

            if (udpReceiver.Info.accel == 0)
            {
                boxAccel.HeightRequest = 3;
                boxAccel.Color = Color.White;
            }
            else
            {
                boxAccel.HeightRequest = udpReceiver.Info.accel * pedalsHeight;
                boxAccel.Color = udpReceiver.Info.accel == 1 ? Color.LightGreen : Color.Green;
            }

            if(boxTurbo !=null)
            {
                if (udpReceiver.Info.turbo == 0)
                {
                    boxTurbo.HeightRequest = 3;
                    boxTurbo.Color = Color.White;
                }
                else
                {
                    boxTurbo.HeightRequest = udpReceiver.TurboPercent() * pedalsHeight;
                }
            }
        }

    }
}