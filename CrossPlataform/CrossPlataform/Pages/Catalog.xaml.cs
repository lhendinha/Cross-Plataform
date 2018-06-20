using CrossPlataform.Models;
using CrossPlataform.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossPlataform.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Catalog : ContentPage
    {
        private readonly string _catalogPropertie = "Catalog";

        public Catalog()
        {
            InitializeComponent();

            VerifyPendencies();
        }

        protected async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Functions functions = new Functions();

            ProductVm catalog = e.Item as ProductVm;

            if (catalog == null)
            {
                return;
            }

            await Application.Current.MainPage.DisplayAlert(DisplayAlertProperties.MenuTitle.ERROR_TITLE, DisplayAlertProperties.MessageBody.ERROR_CONNECT_TO_INTERNET, DisplayAlertProperties.Button.OK);
        }

        protected async void ListItems_Refreshing(object sender, EventArgs e)
        {
            Functions functions = new Functions();

            var isConnected = await functions.IsConnect();
            if (isConnected)
            {
                await VerifyCache();
                PopulateListView(true);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert(DisplayAlertProperties.MenuTitle.ERROR_TITLE, DisplayAlertProperties.MessageBody.ERROR_CONNECT_TO_INTERNET, DisplayAlertProperties.Button.OK);
            }

            catalog.EndRefresh();
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

        private async void VerifyPendencies()
        {
            await VerifyCache();

            PopulateListView(false);
        }

        private async Task VerifyCache()
        {
            Functions functions = new Functions();

            var isConnected = await functions.IsConnect();

            if (!App.Current.Properties.ContainsKey(_catalogPropertie))
            {
                if (isConnected)
                {
                    App.Current.Properties[_catalogPropertie] = await functions.LoadApiData();
                }
                else
                {
                    App.Current.Properties[_catalogPropertie] = null;
                }
            }
            else
            {
                if (isConnected)
                {
                    var cachedData = App.Current.Properties[_catalogPropertie].ToString();
                    var apiData = await functions.LoadApiData();

                    if (string.IsNullOrEmpty(cachedData) || cachedData != apiData)
                    {
                        App.Current.Properties[_catalogPropertie] = apiData;
                    }
                }
                else
                {
                    var cachedData = App.Current.Properties[_catalogPropertie].ToString();
                    if (string.IsNullOrEmpty(cachedData))
                    {
                        await Application.Current.MainPage.DisplayAlert(DisplayAlertProperties.MenuTitle.ERROR_TITLE, DisplayAlertProperties.MessageBody.ERROR_CONNECT_TO_INTERNET, DisplayAlertProperties.Button.OK);
                    }
                }
            }

            await App.Current.SavePropertiesAsync();
        }

        private void PopulateListView(bool isRefreshing)
        {
            var data = JsonConvert.DeserializeObject<List<ProductVm>>(Application.Current.Properties[_catalogPropertie].ToString());

            if (isRefreshing)
            {
                catalog.ItemsSource = data;
            }
            else
            {
                LoadingIcon(false);

                catalog.ItemsSource = data;

                LoadingIcon(true);
            }
        }
    }
}