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

            LoadingIcon(false);

            CacheData(true);
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
                CacheData(false);
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
                if (!Application.Current.Properties.ContainsKey(_catalogPropertie))
                {
                    await CreateCache(_catalogPropertie);
                }

                var isConnect = await functions.VerifyConnection();

                if (string.IsNullOrEmpty(Application.Current.Properties[_catalogPropertie].ToString()))
                {
                    if (isConnect)
                    {
                        var apiData = await functions.LoadApiData();
                        if (Application.Current.Properties[_catalogPropertie].ToString() != apiData)
                        {
                            Application.Current.Properties[_catalogPropertie] = apiData;
                        }
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
            else
            {
                var apiData = await functions.LoadApiData();
                if (Application.Current.Properties[_catalogPropertie].ToString() != apiData)
                {
                    Application.Current.Properties[_catalogPropertie] = apiData;
                }
            }

            await Application.Current.SavePropertiesAsync();

            IsRefreshing(store);
        }

        private void IsRefreshing(bool whichAction)
        {
            if (whichAction)
            {
                PopulateListView(false);
            }
            else
            {
                PopulateListView(true);
            }
        }

        private async Task CreateCache(string cacheKey)
        {
            Application.Current.Properties.TryAdd(cacheKey, string.Empty);
            await Application.Current.SavePropertiesAsync();
        }
    }
}