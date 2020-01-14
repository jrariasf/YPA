using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using YPA.Models;

namespace YPA.Views.Formularios
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryCAMINOS : ContentPage
    {
        public EntryCAMINOS()
        {
            InitializeComponent();

        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var note = (TablaCAMINOS)BindingContext;
            note.fecUltMod = DateTime.UtcNow;
            await App.Database.SaveCaminosAsync(note);
            await Navigation.PopAsync();
        }

        async void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            var note = (TablaCAMINOS)BindingContext;
            await App.Database.DeleteCaminosAsync(note);
            await Navigation.PopAsync();
        }
    }
}