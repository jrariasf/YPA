using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using YPA.Models;

namespace YPA.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VerCamino : ContentPage
    {       
        public VerCamino()
        {
            Console.WriteLine("DEBUG - VerCamino: Antes de llamar a InitializeComponent()");
            //Acumulado.acumulado = 0;
            //PoblacionVisible.esRutaPrincipal = true;
            InitializeComponent();

            Console.WriteLine("DEBUG - VerCamino: Después de llamar a InitializeComponent()");
        }
        
    }

}
