using CrossPlataform.Models;
using CrossPlataform.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossPlataform.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Catalog : ContentPage
    {
        public Catalog()
        {
            InitializeComponent();

            PopulateListView(false);
        }

        private async Task<List<ProductVm>> LoadApiData()
        {
            List<ProductVm> products = new List<ProductVm>();

            HttpClient client = new HttpClient();

            try
            {
                var response = await client.GetStringAsync("http://ecommercee.azurewebsites.net/api/products");

                products = JsonConvert.DeserializeObject<List<ProductVm>>(response);
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Wasn't possible to load product list.", "OK");
            }

            return products;
        }

        async void PopulateListView(bool isRefreshing)
        {
            if (isRefreshing)
            {
                catalog.ItemsSource = await LoadApiData();
            }
            else
            {
                LoadingIcon(false);

                catalog.ItemsSource = await LoadApiData();

                LoadingIcon(true);
            }
        }

        private void LoadingIcon(bool isLoaded)
        {
            if (isLoaded)
            {
                catalog.IsVisible = true;
                loadingItems.IsVisible = false;
            }
            else
            {
                catalog.IsVisible = false;
                loadingItems.IsVisible = true;
            }
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Functions functions = new Functions();

            ProductVm catalog = e.Item as ProductVm;

            if (catalog == null)
            {
                return;
            }

            await DisplayAlert("Details", functions.ReplaceCharacteres(catalog.ShortDescription), "OK");
        }

        protected void ListItems_Refreshing(object sender, EventArgs e)
        {
            PopulateListView(true);
            catalog.EndRefresh();
        }
    }
}