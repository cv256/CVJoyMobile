using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace CVJoyMobile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDebug: ContentPage
    {

        public PageDebug()
        {
            InitializeComponent();
            lbDebugPrints.Text = (Application.Current as CVJoyMobile.App).DebugPrints;
            (Application.Current as CVJoyMobile.App).DebugPrintsUpdated += DebugPrintsUpdated;
        }

        private void DebugPrintsUpdated(string debugPrints)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                this.BatchBegin();
                lbDebugPrints.Text = debugPrints;
                this.BatchCommit();
            });
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            (Application.Current as CVJoyMobile.App).DebugPrintsUpdated -= DebugPrintsUpdated;
            (Application.Current as CVJoyMobile.App).AskForPage();
        }

    }
}