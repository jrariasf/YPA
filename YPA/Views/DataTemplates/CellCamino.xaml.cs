using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Xml.Serialization;
using System.IO;
using System.Windows.Input;

namespace YPA.Views.DataTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CellCamino : ViewCell
    {
        public CellCamino()
        {
            InitializeComponent();
        }


        async private void OnVerEtapasCamino(object sender, EventArgs e)
        {
            Console.WriteLine("DEBUG - Estamos en CellCamino.xaml.cs:OnVerEtapasCamino");
            Console.WriteLine("DEBUG - sender: {0}", sender.ToString());
            Console.WriteLine("DEBUG - e: {0}", e.ToString());
            var b = (Button)sender;
            Console.WriteLine("DEBUG - Asignado el botón");
            /*
             * var item = (YoPilgrim.Models.TablaCAMINOS)(b.CommandParameter);
            Console.WriteLine("DEBUG - Largo: {0}   Corto: {1}   Longitud: {2}",
                              item.nombreLargoCamino, item.nombreCortoCamino, item.longitud);
             */
            var cadena = (String)(b.CommandParameter);
            Console.WriteLine("DEBUG - nombreCortoCamino: {0}", cadena);
            Console.WriteLine("DEBUG - CellCamino - OnVerEtapasCamino  Ahora es cuando deberiamos navegar a VerCamino?camino={0}", cadena);
            ((App)Application.Current).camino = cadena;
            ((YPA.App)(App.Current)).IrA("NavigationPage/VerCamino?camino=" + cadena);
            //_xx_ ((MainPage)Application.Current.MainPage).camino = cadena;               
            //_xx_ await ((MainPage)Application.Current.MainPage).NavigateFromMenu((int)YPA.Models.MenuItemType.VerCamino);

            //var newPage = ((MainPage)Application.Current.MainPage).MenuPages[(int)YoPilgrim.Models.MenuItemType.VerCamino];
            //newPage.BindingContext = Models.TablaCAMINOS;



        }


    }

    class MyClass
    {
        public ICommand MyCommand { protected set; get; }
        public MyClass()
        {
            MyCommand = new Command<string>(s => { Console.WriteLine("Recibido: {0}", s); });

        }
    }
}