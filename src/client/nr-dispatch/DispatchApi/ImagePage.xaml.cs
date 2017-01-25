using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace DispatchApi
{
    public partial class ImagePage : ContentPage
    {

        // Track whether the user has authenticated.
        bool authenticated = false;



        MobileServiceClient client;

        MainPageViewModel viewModel = new MainPageViewModel();

        public ImagePage()
        {
            InitializeComponent();

        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                Refresh();

                // Hide the Sign-in button.
                this.loginButton.IsVisible = false;
            }
        }

        protected async void Refresh()
        {

            client = DispatchManager.DefaultManager.CurrentClient;

            var imageTable = client.GetTable<images>();

            var res = from a in imageTable select a;

            var e = await res.ToEnumerableAsync();

            foreach (var item in e)
            {
                viewModel.Items.Add(new MainPageItem() { DroneName=item.DroneId });
            }

            this.BindingContext = viewModel;

        }



        async void loginButton_Clicked(object sender, EventArgs e)
        {
            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            // Set syncItems to true to synchronize the data on startup when offline is enabled.
            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                Refresh();

                // Hide the Sign-in button.
                this.loginButton.IsVisible = false;
            }
        }
    }
}
