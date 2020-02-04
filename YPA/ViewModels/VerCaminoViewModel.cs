using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using YPA.Models;

namespace YPA.ViewModels
{
    public class VerCaminoViewModel : BindableBase, INavigationAware, INotifyPropertyChanged
    {
        INavigationService _navigationService;

        public static char separador = ';';

        Dictionary<string, string> bifurcaciones = null; // new Dictionary<string, string>();
        bool primeraVez = true;

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - PoblacionesVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<TablaBaseCaminos> _listaEtapas;
        public ObservableCollection<TablaBaseCaminos> listaEtapas
        {
            get { return _listaEtapas; }
            ///set { SetProperty(ref _listaPoblaciones, value); }
            set
            {
                if (_listaEtapas == value)
                    return;
                SetProperty(ref _listaEtapas, value);
                RaisePropertyChanged(nameof(listaEtapas));
            }
        }

        private DelegateCommand _addCommand;
        public DelegateCommand AddCommand =>
            _addCommand ?? (_addCommand = new DelegateCommand(ExecuteAddCommand, CanExecuteAddCommand));

        void ExecuteAddCommand()
        {
            Console.WriteLine("VerCaminoVM - ExecuteAddCommand");
        }

        bool CanExecuteAddCommand()
        {
            return true;
        }


        private DelegateCommand<TablaBaseCaminos> _ItemTappedCommand;
        public DelegateCommand<TablaBaseCaminos> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<TablaBaseCaminos>(ExecuteItemTappedCommand));

        void ExecuteItemTappedCommand(TablaBaseCaminos camino)
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand({0})  entrar...", camino);
            //var navigationParams = new NavigationParameters();
            //navigationParams.Add("camino", camino);
            //_navigationService.NavigateAsync("EntryCAMINOS", navigationParams);

            listaEtapas.Remove(camino);
        }



        bool CanExecuteItemTappedCommand()
        {
            return true;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
            var navigationMode = parameters.GetNavigationMode();
            Console.WriteLine("DEBUG2 - PoblacionesVM - OnNavigatedFrom()  navigationMode:{0}", navigationMode);
        }

        async public void OnNavigatedTo(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
            var navigationMode = parameters.GetNavigationMode();       

            string camino = parameters.GetValue<string>("camino");
            Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo(camino:{0})  navigationMode:{1}", camino, navigationMode);
            //List<TablaBaseCaminos> miLista;
            if (camino == "CaminoDeMadrid")
            {
                Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo()  CaminoDeMadrid");
                List<TablaCaminoDeMadrid> miLista;
                miLista = await App.Database.GetCaminoDeMadridAsync();
                listaEtapas = new ObservableCollection<TablaBaseCaminos>(miLista);
            }
            else if (camino == "SanSalvador")
            {
                Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo()  SanSalvador");
                List<TablaSanSalvador> miLista;
                miLista = await App.Database.GetCaminoSanSalvadorAsync();
                listaEtapas = new ObservableCollection<TablaBaseCaminos>(miLista);
            }
            else
            {
                Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo()  Camino no contemplado");
                return;
            }

            EstablecerVisibilidad();

            Acumulado.acumulado = 0;
            PoblacionVisible.miVerCamino = this;
            PoblacionVisible.esRutaPrincipal = true;
            PoblacionVisible.siguientePoblacion = "";

            //CalculaSiguientePoblacion("");

        }

        void EstablecerVisibilidad()
        {
            bool estamosEnBifurcacion = false;
            double acumulado = 0;
            string siguienteNodo;
            int posicion;
            

            siguienteNodo = listaEtapas[0].nombrePoblacion; // Lo inicializamos con el nombre de la primera población.

            Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad() entrar  siguienteNodo:{0}", siguienteNodo);

            List<TablaBaseCaminos> borrar = new List<TablaBaseCaminos>();

            if (primeraVez)
            {
                if (bifurcaciones == null)
                    bifurcaciones = new Dictionary<string, string>();
                
            }

            foreach (var item in listaEtapas)
            {
                var dataItem = (TablaBaseCaminos)item;

                Console.WriteLine("DEBUG - EstablecerVisibilidad() estamosEnBifurcacion:{0}  siguienteNodo:{1}  nodo actual:{2}",
                            estamosEnBifurcacion, siguienteNodo, dataItem.nombrePoblacion);

                if (!estamosEnBifurcacion)
                {
                    dataItem.esVisible = true;
                    // Si no estamos en bifurcación, el nombre de la población debería coincidir con el que hay en siguienteNodo:
                    if (dataItem.nombrePoblacion != siguienteNodo)
                        Console.WriteLine("ERROR - VerCaminoVM - EstablecerVisibilidad: Se esperaba {0} y estamos en {1}.",
                            siguienteNodo, dataItem.nombrePoblacion);
                }
                else
                {
                    // Estamos en una bifurcación:
                    if (dataItem.nombrePoblacion == siguienteNodo)
                    {
                        Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad: Estamos en bifurcación y el nodo pertenece a la bifurcación");
                        dataItem.esVisible = true;
                    }
                    else
                    {
                        Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad: Estamos en bifurcación y el nodo NO pertenece a la bifurcación");
                        dataItem.esVisible = false;
                        borrar.Add(dataItem);
                    }
                }

                //Lo siguiente sólo es válido para un nodo que pertenezca al camino configurado:
                if (dataItem.esVisible == true)
                {
                    string distanciaSiguiente = dataItem.distanciaNodosSiguientes;
                    string primeraDistancia;
                    dataItem.acumulado = acumulado;

                    posicion = distanciaSiguiente.IndexOf(VerCaminoViewModel.separador);
                    if (posicion == -1)
                        primeraDistancia = distanciaSiguiente;
                    else
                        primeraDistancia = distanciaSiguiente.Substring(0, posicion);
                    acumulado += double.Parse(primeraDistancia);

                    //Primero hay que comprobar si estamos en un fin de bifurcación. Y despues se comprueba si estamos en un inicio de bifurcación (un nodo puede ser las dos cosas):
                    if (dataItem.FinBifurcacion)
                    {
                        estamosEnBifurcacion = false;
                        Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad: Ha finalizado la bifurcación");
                    }

                    if (dataItem.IniBifurcacion)
                    {
                        estamosEnBifurcacion = true;
                        //Calculamos siguiente nodo:
                        posicion = ((string)dataItem.nodosSiguientes).IndexOf(VerCaminoViewModel.separador);
                        if (posicion == -1)
                        {
                            Console.WriteLine("ERROR - VerCaminoVM - EstablecerVisibilidad: Estamos en un Inicio de Bifurcación pero sólo hay un posible nodo siguiente!!");
                            siguienteNodo = dataItem.nodosSiguientes;
                        }
                        else
                            siguienteNodo = (dataItem.nodosSiguientes).Substring(0, posicion);

                        bifurcaciones.Add(dataItem.nombrePoblacion, siguienteNodo);

                    }
                    else
                    {
                        siguienteNodo = dataItem.nodosSiguientes;
                    }

                    Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad: siguienteNodo {0}", siguienteNodo);
                }

                primeraVez = false;

            }

            foreach (KeyValuePair<string, string> kvp in bifurcaciones)
            {
                Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad: bifurcaciones: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

            Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad: Número de nodos en listaEtapas:{0}", listaEtapas.Count());
            if (borrar.Count() > 0)
                foreach (var b in borrar)
                {
                    Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad: Borramos {0}", b.nombrePoblacion);
                    listaEtapas.Remove(b);
                }

            Console.WriteLine("DEBUG - VerCaminoVM - EstablecerVisibilidad: Número de nodos final en listaEtapas:{0}", listaEtapas.Count());

        }

        public string CalculaSiguientePoblacion(string poblacion)
        {
            int posicion;

            
            foreach (var item in listaEtapas)
            {
                // cast the item
                var dataItem = (TablaBaseCaminos)item;
                
                Console.WriteLine("DEBUG - VerCaminoVM - CalculaSiguientePoblacion():  id: {0}   Población: {1}", dataItem.id, dataItem.nombrePoblacion);
                if (poblacion == "")
                {
                    PoblacionVisible.siguientePoblacion = dataItem.nombrePoblacion;
                    break;
                }
                else if (dataItem.nombrePoblacion == poblacion)
                {
                    posicion = ((string)dataItem.nodosSiguientes).IndexOf(VerCaminoViewModel.separador);
                    if (posicion == -1)
                        PoblacionVisible.siguientePoblacion = dataItem.nodosSiguientes;
                    else
                        PoblacionVisible.siguientePoblacion = (dataItem.nodosSiguientes).Substring(0, posicion);
                    break;
                }
            }

            return PoblacionVisible.siguientePoblacion;
        }

        public VerCaminoViewModel(INavigationService navigationService)
        {
            Console.WriteLine("DEBUG - CONSTR - VerCaminoViewModel()");
            _navigationService = navigationService;
            
        }
    }

    public class Acumulado : IValueConverter
    {
        public static double acumulado = 0;
        //public static bool esCaminoAlternativo = false;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Console.WriteLine("DEBUG - VerCaminosVM - Acumulado:Convert  value: {0}", value);
            string primeraDistancia;
            if (PoblacionVisible.esRutaPrincipal == true)
            {
                int posicion = ((string)value).IndexOf(VerCaminoViewModel.separador);
                if (posicion == -1)
                    primeraDistancia = (string)value;
                else
                    primeraDistancia = ((string)value).Substring(0, posicion);
                acumulado += double.Parse((string)primeraDistancia);
            }
            Console.WriteLine("DEBUG - VerCaminosVM - Acumulado:Convert  salir... acumulado: {0}", acumulado);
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
        public static VerCaminoViewModel miVerCamino;
        public static bool esRutaPrincipal = true;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //Console.WriteLine("DEBUG - PoblacionVisible:Convert  value: {0}   parameter: {1}  siguientePoblacion: {2}",
            //                  value, parameter == null ? "null" : parameter, siguientePoblacion);
            string primeraPoblacion;
            int posicion = ((string)value).IndexOf(VerCaminoViewModel.separador);
            if (posicion == -1)
                primeraPoblacion = (string)value;
            else
                primeraPoblacion = ((string)value).Substring(0, posicion);

            if (siguientePoblacion == "")
            {
                Console.WriteLine("DEBUG - VerCaminosVM - PoblacionVisible:Convert Inicializamos siguientePoblacion con {0}", primeraPoblacion);
                siguientePoblacion = primeraPoblacion;
            }

            if (primeraPoblacion == siguientePoblacion)
            {
                //Calculamos la nueva siguientePoblacion:                
                miVerCamino.CalculaSiguientePoblacion(primeraPoblacion);
                esRutaPrincipal = true;
                //return true;
            }
            else
                esRutaPrincipal = false;

            Console.WriteLine("DEBUG2 - VerCaminosVM - PoblacionVisible:Convert  value: {0}   parameter: {1}  siguientePoblacion: {2}   devuelve:{3}",
                              value, parameter == null ? "null" : parameter, siguientePoblacion, esRutaPrincipal);
            //return false;
            return esRutaPrincipal;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class IntToColorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string respuesta = "";
            if ((bool)value == false)
                respuesta = "Yellow";
            else
                respuesta = "Blue";

            Console.WriteLine("DEBUG - IntToColorStringConverter:Convert  value: {0}   respuesta:{1}", (bool)value, respuesta);

            return respuesta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
