using CrossPlataform.Models;
using CrossPlataform.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            CacheData(true);
            PopulateListView(false);
        }

        void PopulateListView(bool isRefreshing)
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

        protected async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Functions functions = new Functions();

            ProductVm catalog = e.Item as ProductVm;

            if (catalog == null)
            {
                return;
            }

            await DisplayAlert(DisplayAlertProperties.MenuTitle.DETAILS_TITLE, functions.ReplaceCharacteres(catalog.ShortDescription), DisplayAlertProperties.Button.OK);
        }

        protected async void ListItems_Refreshing(object sender, EventArgs e)
        {
            Functions functions = new Functions();

            var isConnect = await functions.VerifyConnection();
            if (isConnect)
            {
                CacheData(true);
                PopulateListView(true);
            }
            else
            {
                await DisplayAlert(DisplayAlertProperties.MenuTitle.ERROR_TITLE, DisplayAlertProperties.MessageBody.ERROR_CONNECT_TO_INTERNET, DisplayAlertProperties.Button.OK);
            }
            
            catalog.EndRefresh();
        }

        private async void CacheData(bool store)
        {
            Functions functions = new Functions();

            if (store)
            {
                var isConnect = await functions.VerifyConnection();

                if (string.IsNullOrEmpty(Application.Current.Properties[_catalogPropertie].ToString()))
                {
                    if (isConnect)
                    {
                        Application.Current.Properties[_catalogPropertie] = await functions.LoadApiData();
                    }
                    else
                    {
                        await DisplayAlert(DisplayAlertProperties.MenuTitle.ERROR_TITLE, DisplayAlertProperties.MessageBody.ERROR_CONNECT_TO_INTERNET, DisplayAlertProperties.Button.OK);
                    }
                }
                else
                {
                    if (isConnect)
                    {
                        Application.Current.Properties[_catalogPropertie] = await functions.LoadApiData();
                    }
                    else
                    {
                        await DisplayAlert(DisplayAlertProperties.MenuTitle.ERROR_TITLE, DisplayAlertProperties.MessageBody.ERROR_CONNECT_TO_INTERNET, DisplayAlertProperties.Button.OK);
                    }
                }
            }
        }
    }
}