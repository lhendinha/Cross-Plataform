using CrossPlataform.Models;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CrossPlataform.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMenu : MasterDetailPage
    {
        public List<MainMenuItem> MainMenuItems { get; set; }

        public MainMenu()
        {
            // Set the binding context to this code behind.
            BindingContext = this;

            // Build the Menu
            MainMenuItems = new List<MainMenuItem>()
            {
                new MainMenuItem() { Title = "Home", Icon = "menu_inbox.png", TargetType = typeof(Home) },
                new MainMenuItem() { Title = "Catalog", Icon = "menu_catalog.png", TargetType = typeof(Catalog) }
            };

            // Set the default page, this is the "home" page.
            Detail = new NavigationPage(new Home());

            InitializeComponent();
        }

        // When a MenuItem is selected.
        public void MainMenuItem_Selected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MainMenuItem;
            if (item != null)
            {
                if (item.Title.Equals("Home"))
                {
                    Detail = new NavigationPage(new Home());
                }
                else if (item.Title.Equals("Catalog"))
                {
                    Detail = new NavigationPage(new Catalog());
                }

                MenuListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}