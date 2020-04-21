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
            Console.WriteLine("DEBUG - CONSTR - Poblaciones()");
            InitializeComponent();
        }
        
    }
}