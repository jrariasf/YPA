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
    public partial class Alojamientos : ContentPage
    {
        public Alojamientos()
        {
            Console.WriteLine("CONSTR - Alojamientos()");
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetAlojamientosAsync();
        }

        async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryALOJAMIENTOS
            {
                BindingContext = new TablaALOJAMIENTOS()
            });
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new EntryALOJAMIENTOS
                {
                    BindingContext = e.SelectedItem as TablaALOJAMIENTOS
                });
            }
        }
    }
}