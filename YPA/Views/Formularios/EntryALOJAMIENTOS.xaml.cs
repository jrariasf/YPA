using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YPA.Models;


namespace YPA.Views.Formularios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryALOJAMIENTOS : ContentPage
    {       
        public EntryALOJAMIENTOS()
        {
            InitializeComponent();

            //Task<TablaPOBLACIONES> pob = App.Database.GetPoblacionesAsync(100);
            //string pob = App.Database.DameNombrePoblacionPorId(100);
            //Console.WriteLine("DEBUG - EntryALOJAMIENTOS  nombrePoblacion: {0}", pob);

            Console.WriteLine("DEBUG - EntryALOJAMIENTOS  Llamamos a ObtenerPoblaciones()");
            ObtenerPoblaciones();

            //listView.ItemsSource = await App.Database.GetCaminosAsync();

        }

        async void ObtenerPoblaciones()
        {
            var listaPoblaciones = await App.Database.GetPoblacionesAsync();

            picker.ItemsSource = listaPoblaciones;

        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = (TablaALOJAMIENTOS)BindingContext;
            note.fecUltMod = DateTime.UtcNow;
            await App.Database.SaveAlojamientosAsync(note);
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (TablaALOJAMIENTOS)BindingContext;
            await App.Database.DeleteAlojamientosAsync(note);
            await Navigation.PopAsync();
        }
    }
}