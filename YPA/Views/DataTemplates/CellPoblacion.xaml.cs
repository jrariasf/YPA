using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace YPA.Views.DataTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CellPoblacion : ViewCell
    {
        public CellPoblacion()
        {
            InitializeComponent();
        }

        public void TappedVerAlbergues(object sender, EventArgs e)
        {
            string pob = ((Label)sender).Text;
            string idPob = id.Text;
            Console.WriteLine("DEBUG - TappedVerAlbergues  idPob:{0}", idPob);
            Console.WriteLine("DEBUG - TappedVerAlbergues  pob:{0}", pob);
            ((YPA.App)(App.Current)).IrA("NavigationPage/Ver?listado=albergues&idPoblacion=" + idPob);
        }
    }
}