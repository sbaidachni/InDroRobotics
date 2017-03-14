using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPTestApp.Data;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPTestApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.NavigationMode != NavigationMode.Back)
            {
                Utility.data = await Utility.DownloadItems();
               
            }
            gridView1.ItemsSource = Utility.data;
        }

        private void gridView1_ItemClick(object sender, ItemClickEventArgs e)
        {
            this.Frame.Navigate(typeof(ImageDetails),(e.ClickedItem as ImageItem).ImageUri);
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            Utility.data = await Utility.DownloadItems();
            gridView1.ItemsSource = Utility.data;
        }
    }
}
