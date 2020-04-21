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
    public partial class Caminos : ContentPage
    {
        public Caminos()
        {
            Console.WriteLine("Caminos.xaml.cs:Caminos()  Llamamos a InitializeComponent()");
            InitializeComponent();
        }

        /*
        protected override async void OnAppearing()
        {
            Console.WriteLine("Caminos.xaml.cs:OnAppearing()");
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetCaminosAsync();
        }
       
        async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            Console.WriteLine("Caminos.xaml.cs:OnNoteAddedClicked");
            await Navigation.PushAsync(new EntryCAMINOS
            {
                BindingContext = new TablaCAMINOS()
            });
        }
        

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Console.WriteLine("Caminos.xaml.cs:OnListViewItemSelected");
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new EntryCAMINOS
                {
                    BindingContext = e.SelectedItem as TablaCAMINOS
                });
            }
        }

        private void OnVerEtapasButtonClicked(object sender, EventArgs e)        
        {
            // Aquí tengo que ver la forma de mostrar en pantalla todas las etapas de ese camino elegido.
            // Tengo que ver qué me llega en "sender" y en "e" !!!
            Console.WriteLine("DEBUG - {0}", "Estamos en OnVerEtapasButtonClicked");
          
        }
        
        async private void OnVerEtapasCamino(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG - Estamos en Caminos:OnVerEtapasCamino");
            Console.WriteLine("DEBUG - sender: {0}", sender.ToString());
            Console.WriteLine("DEBUG - e: {0}", e.ToString());
            var b = (Button)sender;
            Console.WriteLine("DEBUG - Asignado el botón");
            
            // var item = (YoPilgrim.Models.TablaCAMINOS)(b.CommandParameter);
            //Console.WriteLine("DEBUG - Largo: {0}   Corto: {1}   Longitud: {2}",
            //                  item.nombreLargoCamino, item.nombreCortoCamino, item.longitud);
            
            var cadena = (String)(b.CommandParameter);
            Console.WriteLine("DEBUG - nombreCortoCamino: {0}", cadena);
            Console.WriteLine("DEBUG - Caminos:OnVerEtapasCamino  Ahora es cuando deberiamos navegar a VerCamino");
            //_xx_ ((MainPage)Application.Current.MainPage).camino = cadena;               
            //_xx_ await ((MainPage)Application.Current.MainPage).NavigateFromMenu((int)YPA.Models.MenuItemType.VerCamino);

            //var newPage = ((MainPage)Application.Current.MainPage).MenuPages[(int)YoPilgrim.Models.MenuItemType.VerCamino];
            //newPage.BindingContext = Models.TablaCAMINOS;

        }
    */
    }
}