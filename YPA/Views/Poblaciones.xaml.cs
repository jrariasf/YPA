using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using YPA.Models;
using YPA.Views.Formularios;

namespace YPA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Poblaciones : ContentPage
    {
        public Poblaciones()
        {
            Console.WriteLine("CONSTR - Poblaciones()");
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetPoblacionesAsync();
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryPOBLACIONES
            {
                BindingContext = new TablaPOBLACIONES()
            });
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new EntryPOBLACIONES
                {
                    BindingContext = e.SelectedItem as TablaPOBLACIONES
                });
            }
        }
    }
}