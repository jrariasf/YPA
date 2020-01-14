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
            InitializeComponent();

        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = (TablaPOBLACIONES)BindingContext;
            note.fecUltMod = DateTime.UtcNow;
            await App.Database.SavePoblacionesAsync(note);
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (TablaPOBLACIONES)BindingContext;
            await App.Database.DeletePoblacionesAsync(note);
            await Navigation.PopAsync();
        }
    }
}