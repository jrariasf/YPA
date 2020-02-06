using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using YPA.Models;

namespace YPA.ViewModels
{
    public class VerCaminoViewModel : BindableBase, INavigationAware, INotifyPropertyChanged
    {
        INavigationService _navigationService;

        public static char separador = ';';

        string caminoActual = null;

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

        ObservableCollection<TablaBaseCaminos> back_listaEtapas;

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

            //listaEtapas.Remove(camino);

            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand()  nombre:{0}  esVisible:{1}", camino.nombrePoblacion, camino.esVisible);

            MasajearLista(camino.nombrePoblacion);
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

            caminoActual = parameters.GetValue<string>("camino");
            Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo(camino:{0})  navigationMode:{1}", caminoActual, navigationMode);

            //RellenarLista(); 
            
            MasajearLista(null);

            Acumulado.acumulado = 0;
            PoblacionVisible.miVerCamino = this;
            PoblacionVisible.esRutaPrincipal = true;
            PoblacionVisible.siguientePoblacion = "";

            //CalculaSiguientePoblacion("");

        }

        public async Task<bool> RellenarLista()
        {
            if (caminoActual == null)
            {
                Console.WriteLine("ERROR - VerCaminoVM - RellenarLista()  caminoActual es null !!!");
                return false;
            }

            List<TablaBaseCaminos> miLista = await App.Database.GetEtapasCamino(caminoActual);
            back_listaEtapas = new ObservableCollection<TablaBaseCaminos>(miLista);

            /*
            if (caminoActual == "CaminoDeMadrid")
            {
                Console.WriteLine("DEBUG - VerCaminoVM - RellenarLista()  CaminoDeMadrid");
                List<TablaCaminoDeMadrid> miLista = null;
                miLista = await App.Database.GetCaminoDeMadridAsync();
                if (miLista == null)
                    Console.WriteLine("ERROR - VerCaminoVM - RellenarLista()  miLista es null !!");
                else
                    Console.WriteLine("DEBUG3 - VerCaminoVM - RellenarLista()  miLista NOO es null !!");
                back_listaEtapas = new ObservableCollection<TablaBaseCaminos>(miLista);
                if (back_listaEtapas == null)
                    Console.WriteLine("ERROR - VerCaminoVM - RellenarLista()  back_listaEtapas es null !!");
                else
                    Console.WriteLine("DEBUG3 - VerCaminoVM - RellenarLista()  back_listaEtapas NOO es null !!");
            }
            else if (caminoActual == "SanSalvador")
            {
                Console.WriteLine("DEBUG - VerCaminoVM - RellenarLista()  SanSalvador");
                List<TablaSanSalvador> miLista = null;
                miLista = await App.Database.GetCaminoSanSalvadorAsync();
                if (miLista == null)
                    Console.WriteLine("ERROR - VerCaminoVM - RellenarLista()  miLista es null !!");
                else
                    Console.WriteLine("DEBUG3 - VerCaminoVM - RellenarLista()  miLista NOO es null !!");
                back_listaEtapas = new ObservableCollection<TablaBaseCaminos>(miLista);
                if (back_listaEtapas == null)
                    Console.WriteLine("ERROR - VerCaminoVM - RellenarLista()  back_listaEtapas es null !!");
                else
                    Console.WriteLine("DEBUG3 - VerCaminoVM - RellenarLista()  back_listaEtapas NOO es null !!");

            }
            else
            {
                Console.WriteLine("DEBUG - VerCaminoVM - RellenarLista()  Camino no contemplado");
                return false;
            }
            */

            return true;
        }


        async void MasajearLista(string cambiarBifurcacionEn=null)
        {
            bool estamosEnBifurcacion = false;
            double acumulado = 0;
            double distanciaTotal;
            string siguienteNodo;

            Console.WriteLine("ERROR - VerCaminoVM - MasajearLista(): cambiarBifurcacionEn:{0}", 
                    cambiarBifurcacionEn == null ? "NULL" : cambiarBifurcacionEn);
            /*
            if (listaEtapas == null)
            {
                Console.WriteLine("ERROR - VerCaminoVM - MasajearLista(): listaEtapas es null !!");
                return;
            }
            */

            //Primero rellenamos de nuevo listaEtapas con todos los nodos:

            bool resp;
            resp = await RellenarLista();

            if (resp == false)
            { //if (listaEtapas == null)
                Console.WriteLine("ERROR - ###########----------#######    VerCaminoVM - MasajearLista(): listaEtapas es null !!");
                return;
            }
            siguienteNodo = back_listaEtapas[0].nombrePoblacion; // Lo inicializamos con el nombre de la primera población.

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista() entrar  El primer nodo será siguienteNodo:{0}", siguienteNodo);

            // Lista que contendrá los nodos que habrá que eliminar de "listaEtapas" para no mostrarlos en la ListView:
            List<TablaBaseCaminos> borrar = new List<TablaBaseCaminos>();

            //if (primeraVez)           
                if (bifurcaciones == null)
                    bifurcaciones = new Dictionary<string, string>();


            foreach (var item in back_listaEtapas)
            {
                var dataItem = (TablaBaseCaminos)item;

                Console.WriteLine("DEBUG - MasajearLista() estamosEnBifurcacion:{0}  siguienteNodo:{1}  nodo actual:{2}",
                            estamosEnBifurcacion, siguienteNodo, dataItem.nombrePoblacion);

                if (!estamosEnBifurcacion)
                {
                    dataItem.esVisible = true;
                    // Si no estamos en bifurcación, el nombre de la población debería coincidir con el que hay en siguienteNodo:
                    if (dataItem.nombrePoblacion != siguienteNodo)
                        Console.WriteLine("ERROR - VerCaminoVM - MasajearLista: Se esperaba {0} y estamos en {1}",
                            siguienteNodo, dataItem.nombrePoblacion);
                }
                else
                {
                    // Estamos en una bifurcación:                    
                    if (dataItem.nombrePoblacion == siguienteNodo)
                    {
                        Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Estamos en bifurcación y el nodo pertenece a la bifurcación");
                        dataItem.esVisible = true;
                    }
                    else
                    {
                        Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Estamos en bifurcación y el nodo NO pertenece a la bifurcación");
                        dataItem.esVisible = false;
                        borrar.Add(dataItem);
                    }
                }
            

                //Lo siguiente sólo es válido para un nodo que pertenezca al camino configurado:
                if (dataItem.esVisible == true)
                {
                    int indice = 0;
                    string[] distancias = dataItem.distanciaNodosSiguientes.Split(VerCaminoViewModel.separador);
                    string[] nodosSiguientes = dataItem.nodosSiguientes.Split(VerCaminoViewModel.separador);

                    Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista - #distancias:{0}  #nodosSiguientes:{1}",
                                distancias.Count(), nodosSiguientes.Count());

                    //Primero hay que comprobar si estamos en un fin de bifurcación. 
                    //Y más adelante se comprobará si estamos en un inicio de bifurcación (un nodo puede ser las dos cosas):
                    if (dataItem.FinBifurcacion)
                    {
                        estamosEnBifurcacion = false;
                        Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Aquí finaliza una bifurcación");                        
                    }

                    if (dataItem.IniBifurcacion)
                    {
                        Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: Estamos en INI bifurcacion en {0}", dataItem.nombrePoblacion);
                        estamosEnBifurcacion = true;

                        string bifConf = "";
                        if (bifurcaciones.TryGetValue(dataItem.nombrePoblacion, out bifConf))
                        {
                            int len = nodosSiguientes.Length;
                            indice = Array.IndexOf(nodosSiguientes, bifConf);
                            Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: bifConf: {0}  len:{1}  indice:{2}", bifConf, len, indice);
                            if (cambiarBifurcacionEn != null && cambiarBifurcacionEn == dataItem.nombrePoblacion)
                            {
                                // Este es el nodo o población donde tenemos que cambiar la bifurcación a tomar.
                                // Hay que buscar la siguiente bifurcación posible en este nodo,
                                // teniendo en cuenta que la bifurcación que había configurada está ahora en "bifConf":
                                indice = indice == len - 1 ? 0 : ++indice;
                                bifurcaciones[dataItem.nombrePoblacion] = nodosSiguientes[indice];
                                Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: indice:{0}  bif CAMBIADA en {1} hacia {2}",
                                    indice, dataItem.nombrePoblacion, bifurcaciones[dataItem.nombrePoblacion]);
                            }
                        }
                        else // No tenemos configurado ninguna bifurcación en el diccionario para este nodo.
                        {
                            // Si nos pasan algo en cambiarBifurcacionEn habrá que añadirla al diccionario:
                            if (cambiarBifurcacionEn != null && cambiarBifurcacionEn == dataItem.nombrePoblacion)
                            {
                                Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: ****SE AÑADIRA la bif de {0} hacia {1}",
                                    dataItem.nombrePoblacion, nodosSiguientes[1]);
                                bifurcaciones.Add(dataItem.nombrePoblacion, nodosSiguientes[1]);
                                indice = 1;
                            }

                        }

                    }

                    dataItem.acumulado = acumulado;
                    acumulado += double.Parse(distancias[indice]);
                    siguienteNodo = nodosSiguientes[indice];

                    Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: siguienteNodo {0}   acumulado:{1}", 
                                siguienteNodo, acumulado);
                }

                primeraVez = false;

            }

            foreach (KeyValuePair<string, string> kvp in bifurcaciones)
            {
                Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: bifurcaciones: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

            //Ahora relleno el campo "distanciaAlFinal" que es el que dice los kms que restan hasta el final:
            distanciaTotal = acumulado;
            foreach (var item in back_listaEtapas)
            {
                var dataItem = (TablaBaseCaminos)item;
                if (dataItem.esVisible == true)
                {
                    dataItem.distanciaAlFinal = distanciaTotal - dataItem.acumulado;

                }
            }


            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Número de nodos en back_listaEtapas:{0}", back_listaEtapas.Count());
            if (borrar.Count() > 0)
                foreach (var b in borrar)
                {
                    Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Borramos {0}", b.nombrePoblacion);
                    back_listaEtapas.Remove(b);
                }

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Número de nodos final en back_listaEtapas:{0}", back_listaEtapas.Count());
            
            listaEtapas = back_listaEtapas;
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

            //Console.WriteLine("DEBUG - IntToColorStringConverter:Convert  value: {0}   respuesta:{1}", (bool)value, respuesta);

            return respuesta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
