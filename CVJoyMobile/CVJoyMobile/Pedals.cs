using Xamarin.Forms;
using static BaseUdpReceiver;

namespace CVJoyMobile
{
    public class Pedals
    {
        BoxView boxClutch;
        BoxView boxBreak;
        BoxView boxAccel;
        BoxView boxSep;

        public Pedals(Grid absoluteLayout)
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
            absoluteLayout.Children.Add(boxSep,3, 0);

            boxClutch = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.End,
                Color = Color.Blue,
                HeightRequest=30
            };
            absoluteLayout.Children.Add(boxClutch, 0, 0);

            boxBreak = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.End,
                Color = Color.Red,
                HeightRequest = 30
            };
            absoluteLayout.Children.Add(boxBreak,2, 0);

            boxAccel = new BoxView
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.End,
                Color = Color.Green,
                HeightRequest = 30
            };
            absoluteLayout.Children.Add(boxAccel,4, 0);
        }

        public void SetValues(structInfo udpInfo)
        {
            double pedalsHeight = boxSep.Height;

            if (udpInfo.clutch == 0)
            {
                boxClutch.HeightRequest = 3;
                boxClutch.Color = Color.White;
            }
            else 
            {
                boxClutch.HeightRequest = udpInfo.clutch * pedalsHeight;
                boxClutch.Color = udpInfo.clutch == 1 ? Color.AliceBlue : Color.Blue;
            }

            if (udpInfo.brake == 0)
            {
                boxBreak.HeightRequest = 3;
                boxBreak.Color = Color.White;
            }
            else
            {
                boxBreak.HeightRequest = udpInfo.brake * pedalsHeight;
                boxBreak.Color = udpInfo.brake == 1 ? Color.OrangeRed : Color.Red;
            }

            if (udpInfo.accel == 0)
            {
                boxAccel.HeightRequest = 3;
                boxAccel.Color = Color.White;
            }
            else
            {
                boxAccel.HeightRequest = udpInfo.accel * pedalsHeight;
                boxAccel.Color = udpInfo.accel == 1 ? Color.LightGreen : Color.Green;
            }
        }

    }
}