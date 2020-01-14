using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Globalization;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace YPA.Views.DataTemplates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CellEtapaCamino : ViewCell
    {
        public CellEtapaCamino()
        {
            InitializeComponent();
        }
    }

    /*
    public class IntToColorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("DEBUG - IntToColorStringConverter:Convert  value: {0}", (bool)value);
            if ((bool)value == false)
                return "Yellow";
            else
                return "Blue";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class Acumulado : IValueConverter
    {
        public static double acumulado = 0;
        //public static bool esCaminoAlternativo = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("DEBUG - Acumulado:Convert  value: {0}", value);
            string primeraDistancia;
            if (PoblacionVisible.esRutaPrincipal == true)
            {
                int posicion = ((string)value).IndexOf(',');
                if (posicion == -1)
                    primeraDistancia = (string)value;
                else
                    primeraDistancia = ((string)value).Substring(0, posicion);
                acumulado += double.Parse((string)primeraDistancia);
            }
            Console.WriteLine("DEBUG - Acumulado:Convert  salir... acumulado: {0}", acumulado);
            return acumulado;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PoblacionVisible : IValueConverter
    {
        public static string siguientePoblacion = "";
        public static VerCamino miVerCamino;
        public static bool esRutaPrincipal = true;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("DEBUG - PoblacionVisible:Convert  value: {0}   parameter: {1}  siguientePoblacion: {2}",
                              value, parameter == null ? "null" : parameter, siguientePoblacion);
            string primeraPoblacion;
            int posicion = ((string)value).IndexOf(',');
            if (posicion == -1)
                primeraPoblacion = (string)value;
            else
                primeraPoblacion = ((string)value).Substring(0, posicion);

            if (siguientePoblacion == "")
            {
                Console.WriteLine("DEBUG - PoblacionVisible:Convert Inicializamos siguientePoblacion con {0}", primeraPoblacion);
                siguientePoblacion = primeraPoblacion;
            }

            if (primeraPoblacion == siguientePoblacion)
            {
                //Calculamos la nueva siguientePoblacion:                
                //posicion = ((string)parameter).IndexOf(',');
                //siguientePoblacion = ((string)parameter).Substring(0, posicion);
                
                //App.Current.Views  VerCamino.CalculaSiguientePoblacion(primeraPoblacion);
                //((VerCamino)(((MainPage)Application.Current.MainPage).MenuPages[(int)YoPilgrim.Models.MenuItemType.VerCamino])).
                miVerCamino.    CalculaSiguientePoblacion(primeraPoblacion);
                esRutaPrincipal = true;
                return true;
            }

            esRutaPrincipal = false;
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
*/
}