using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CVJoyMobile.Gauge;


namespace CVJoyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageGaugeProps : ContentPage
    {
        Gauge gauge;

        public PageGaugeProps(Gauge pGauge)
        {
            InitializeComponent();
            gauge = pGauge;

            lbvalueMin.Text = gauge.valueMin.ToString();
            lbvalueRedLine.Text = gauge.valueRedLine.ToString();
            lbvalueMax.Text = gauge.valueMax.ToString();
            lbangleMin.Text = gauge.angleMin.ToString();
            lbangleMax.Text = gauge.angleMax.ToString();
            lbneedleWidth.Text = gauge.needleWidth.ToString();
        }

        private void OK_Clicked(object sender, EventArgs e)
        {
            gauge.Init(int.Parse(lbvalueMin.Text), int.Parse(lbvalueRedLine.Text), int.Parse(lbvalueMax.Text),
                Color.FromHsla((double)215 / 360, 1, .30),
                Color.FromHsla((double)215 / 360, 1, .55),
                Color.FromHsla((double)215 / 360, 1, .80),
                Color.Red,
                int.Parse(lbneedleWidth.Text), int.Parse(lbangleMin.Text), int.Parse(lbangleMax.Text), gauge.radiusSize,
                Color.Red);
            gauge.needleValue(int.Parse(lbneedleValue.Text));
            gauge.absoluteLayout.BackgroundColor = Color.FromHsla(0, 100, 50, .5);
            Navigation.PopModalAsync();
        }
        private void Cancel_Clicked(object sender, EventArgs e)
        {
            gauge.absoluteLayout.BackgroundColor = Color.Transparent;
            Navigation.PopModalAsync();
        }

    }
}