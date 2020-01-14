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
        ObservableCollection<TablaBaseCaminos> miLista;
        public static char separador = ';';

        public VerCamino()
        {
            Console.WriteLine("DEBUG - VerCamino: Antes de llamar a InitializeComponent()");
            Acumulado.acumulado = 0;
            PoblacionVisible.esRutaPrincipal = true;
            InitializeComponent();

            Console.WriteLine("DEBUG - VerCamino: Después de llamar a InitializeComponent()");
        }

        public int CalcularRuta()
        {
            string sgtePoblacion;
            int posicion;
            int poblacionesEliminadas = 0;

            if (miLista.Count() == 0)
            {
                Console.WriteLine("DEBUG - CalcularRuta():  miLista contiene 0 elementos");
                return 0;
            }

            sgtePoblacion = miLista[0].nombrePoblacion;

            foreach (var dataItem in miLista.ToList())
            {
                //var dataItem = (YoPilgrim.Models.TablaBaseCaminos)item;
                if (sgtePoblacion == dataItem.nombrePoblacion)
                {
                    posicion = ((string)dataItem.nodosSiguientes).IndexOf(VerCamino.separador);
                    sgtePoblacion = (posicion == -1) ? dataItem.nodosSiguientes : dataItem.nodosSiguientes.Substring(0, posicion);
                }
                else
                {
                    miLista.Remove(dataItem);
                    poblacionesEliminadas++;
                }
            }

            return poblacionesEliminadas;
        }

        //public static string CalculaSiguientePoblacion(ListView miLista)
        public string CalculaSiguientePoblacion(string poblacion)
        {
            int posicion;

            var aux1 = listView.SelectedItem;
            var aux2 = listView.FindByName(poblacion);
            foreach (var item in listView.ItemsSource)
            {
                // cast the item
                var dataItem = (YPA.Models.TablaBaseCaminos)item;
                var aux = listView.SelectedItem;
                Console.WriteLine("DEBUG - OnAppearing():  id: {0}   Población: {1}  aux: {2}", dataItem.id, dataItem.nombrePoblacion, aux);
                if (poblacion == "")
                {
                    PoblacionVisible.siguientePoblacion = dataItem.nombrePoblacion;
                    break;
                }
                else if (dataItem.nombrePoblacion == poblacion)
                {
                    posicion = ((string)dataItem.nodosSiguientes).IndexOf(VerCamino.separador);
                    if (posicion == -1)
                        PoblacionVisible.siguientePoblacion = dataItem.nodosSiguientes;
                    else
                        PoblacionVisible.siguientePoblacion = (dataItem.nodosSiguientes).Substring(0, posicion);
                    break;
                }
            }
            return PoblacionVisible.siguientePoblacion;
        }

        private void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Console.WriteLine("DEBUG - OnItemSelected: sender: {0}  tipo_sender: {1}  selectedItem: {2}",
                sender.ToString(), sender.GetType(), e.SelectedItem.ToString());

            miLista.RemoveAt(e.SelectedItemIndex);

            //DisplayAlert("Item Selected", e.SelectedItem.ToString(), "ok");
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            string nodoSgte;
            int posicion;

            Acumulado.acumulado = 0;
            PoblacionVisible.miVerCamino = this;



            //if (((MainPage)Application.Current.MainPage).camino == "CaminoDeMadrid")
            if (((App)Application.Current).camino == "CaminoDeMadrid")
                {
                //_xx_ PoblacionVisible.siguientePoblacion = "Fuencarral";
                //_xx_ listView.ItemsSource = await App.Database.GetCaminoDeMadridAsync();
                //_xx_ listView.ItemsSource = new ObservableCollection<TablaCaminoDeMadrid>( await App.Database.GetCaminoDeMadridAsync() );

                miLista = new ObservableCollection<TablaBaseCaminos>(await App.Database.GetCaminoDeMadridAsync());
                /*
                if (miLista.Count() == 0)
                {
                    Console.WriteLine("DEBUG - OnAppearing  miLista contiene 0 elementos");
                    return;
                }
                Console.WriteLine("DEBUG - OnAppearing  miLista[0].nombrePoblacion: {0}", miLista[0].nombrePoblacion);
                */
                int num = CalcularRuta();
                Console.WriteLine("DEBUG - OnAppearing: Se han eliminado {0} poblaciones", num);
                //miLista.RemoveAt(5);
                listView.ItemsSource = miLista;

                //PoblacionVisible.siguientePoblacion = CalculaSiguientePoblacion(listView);
                //CalculaSiguientePoblacion(listView);
                CalculaSiguientePoblacion("");

            }
            //else if (((MainPage)Application.Current.MainPage).camino == "SanSalvador")
            else if (((App)Application.Current).camino == "SanSalvador")
            {
                //PoblacionVisible.siguientePoblacion = "Carbajal de la Legua";
                //listView.ItemsSource = await App.Database.GetCaminoSanSalvadorAsync();

                /*
                if (miLista.Count() == 0)
                {
                    Console.WriteLine("DEBUG - OnAppearing  miLista contiene 0 elementos");
                    return;
                }
                */

                miLista = new ObservableCollection<TablaBaseCaminos>(await App.Database.GetCaminoSanSalvadorAsync());
                int num = CalcularRuta();
                Console.WriteLine("DEBUG - OnAppearing: Se han eliminado {0} poblaciones", num);
                //miLista.RemoveAt(5);
                listView.ItemsSource = miLista;
                CalculaSiguientePoblacion("");
            }
            else
                Console.WriteLine("DEBUG - Camino no contemplado");

            //  Console.WriteLine("DEBUG - OnAppearing-VerCamino Lista0: {0}", ((List<Models.TablaBaseCaminos>)(listView.ItemsSource)).Count);
            Console.WriteLine("DEBUG - OnAppearing-VerCamino saliendo...");
        }

        /*
        async void OnNoteAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryCAMINOS
            {
                BindingContext = new TablaCAMINOS()
            });
        }

        async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                await Navigation.PushAsync(new EntryCAMINOS
                {
                    BindingContext = e.SelectedItem as TablaCAMINOS
                });
            }
        }
        */
    }

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
                int posicion = ((string)value).IndexOf(VerCamino.separador);
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
            int posicion = ((string)value).IndexOf(VerCamino.separador);
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
                /*
                posicion = ((string)parameter).IndexOf(',');
                siguientePoblacion = ((string)parameter).Substring(0, posicion);
                */
                //App.Current.Views  VerCamino.CalculaSiguientePoblacion(primeraPoblacion);
                //((VerCamino)(((MainPage)Application.Current.MainPage).MenuPages[(int)YoPilgrim.Models.MenuItemType.VerCamino])).
                miVerCamino.CalculaSiguientePoblacion(primeraPoblacion);
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
}
