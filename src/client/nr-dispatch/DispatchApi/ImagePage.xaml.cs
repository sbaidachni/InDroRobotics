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

            if (App.Authenticator != null)
                authenticated = await App.Authenticator.Authenticate();

            // Refresh items only when authenticated.
            if (authenticated == true)
            {
                Refresh();

                // Hide the Sign-in button.
                this.loginButton.IsVisible = false;

                // Show the Sign-out button.
                this.logoutButton.IsVisible = true;
            }
            else
            {
                // Show the Sign-in button.
                this.loginButton.IsVisible = true;

                // Hide the Sign-out button.
                this.logoutButton.IsVisible = false;
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
            lastRefresh.Text = "Last Refreshed: "+ DateTime.Now.ToString("MMMM, MM dd, yyyy hh: mm:ss");
            this.BindingContext = viewModel;

        }

        async void RefreshClicked(object sender, EventArgs e)
        {
            OnAppearing();
        }

        async void LoginClicked(object sender, EventArgs e)
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

                // Show the Sign-out button.
                this.logoutButton.IsVisible = true;
            }
            else
            {
                // Show the Sign-in button.
                this.loginButton.IsVisible = true;

                // Hide the Sign-out button.
                this.logoutButton.IsVisible = false;
            }
        }

        async void LogoutClicked(object sender, EventArgs e)
        {
            if (DispatchManager.DefaultManager.CurrentClient != null)
            {
                await DispatchManager.DefaultManager.CurrentClient.LogoutAsync();
                authenticated = false;
            }
            if (authenticated == true)
            {
                // Hide the Sign-in button.
                this.loginButton.IsVisible = false;

                // Show the Sign-out button.
                this.logoutButton.IsVisible = true;
            }
            else
            {
                // Show the Sign-in button.
                this.loginButton.IsVisible = true;

                // Hide the Sign-out button.
                this.logoutButton.IsVisible = false;
            }
        }
    }
}
