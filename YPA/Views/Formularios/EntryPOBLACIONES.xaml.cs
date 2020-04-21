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
       
    }
}