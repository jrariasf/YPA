using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YPA.Models;


namespace YPA.Views.Formularios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPOBLACIONES : ContentPage
    {
        public EntryPOBLACIONES()
        {
            Console.WriteLine("DEBUG - EntryPOBLACIONES() entrar...");
            InitializeComponent();
            Console.WriteLine("DEBUG - EntryPOBLACIONES() salir...");
        }

        /* Las hemos pasado al ViewModel:
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG - OnSaveButtonClicked()");
            var note = (TablaPOBLACIONES)BindingContext;
            note.fecUltMod = DateTime.UtcNow;
            await App.Database.SavePoblacionesAsync(note);
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG - OnDeleteButtonClicked()");
            var note = (TablaPOBLACIONES)BindingContext;
            await App.Database.DeletePoblacionesAsync(note);
            await Navigation.PopAsync();
        }
        */
    }
}