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

    }

}