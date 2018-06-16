using CrossPlataform.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossPlataform.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageTwo : ContentPage
	{
		public PageTwo ()
		{
			InitializeComponent ();

            GetUsersAsync();
        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ProductVm catalog = e.Item as ProductVm;

            if (catalog == null)
            {
                return;
            }

            string newDescription = catalog.ShortDescription;

            var stringsToReplace = new string[]
            {
                "<li>",
                "</li>",
                "<ul>",
                "</ul>"
            };

            foreach (var replacement in stringsToReplace)
            {
                newDescription = newDescription.Replace(replacement, string.Empty);
            }

            await DisplayAlert("Details", newDescription, "OK");
        }

        public async void GetUsersAsync()
        {
            List<ProductVm> products = new List<ProductVm>();

            try
            {
                HttpClient client = new HttpClient();

                var response = await client.GetStringAsync("http://ecommercee.azurewebsites.net/api/products");

                products = JsonConvert.DeserializeObject<List<ProductVm>>(response);
            }
            catch (Exception)
            {
                await DisplayAlert("Error", "Wasn't possible to load product list.", "OK");
            }

            catalog.ItemsSource = products;
        }
    }
}