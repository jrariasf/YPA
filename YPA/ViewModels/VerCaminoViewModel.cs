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
        string caminoAnterior = null;

        List<string> poblacionesConAlbergue = null;

        Dictionary<string, string> bifurcaciones = null; // new Dictionary<string, string>();
        bool primeraVez = true;

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - PoblacionesVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        List<TablaBaseCaminos> miLista = null;

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

        private DelegateCommand<string> _OnCheckedChanged;
        public DelegateCommand<string> OnCheckedChanged =>
            _OnCheckedChanged ?? (_OnCheckedChanged = new DelegateCommand<string>(ExecuteOnCheckedChanged));

        void ExecuteOnCheckedChanged(string poblacion)
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteOnCheckedChanged poblacion: {0}", poblacion);
            //if (listaEtapas != null)
            //    listaEtapas[poblacion] = 
        }

        private DelegateCommand<string> _VerPoblacion;
        public DelegateCommand<string> VerPoblacion =>
            _VerPoblacion ?? (_VerPoblacion = new DelegateCommand<string>(ExecuteVerPoblacion));

        void ExecuteVerPoblacion(string poblacion)
        {
            Console.WriteLine("DEBUG - CaminosVM - ExecuteVerPoblacion({0})", poblacion);
            Console.WriteLine("DEBUG - CaminosVM - ExecuteVerPoblacion  UriPath: {0}", _navigationService.GetNavigationUriPath());
            _navigationService.NavigateAsync("Poblaciones?poblacion=" + poblacion);
        }

        private DelegateCommand<string> _VerAlbergues;
        public DelegateCommand<string> VerAlbergues =>
            _VerAlbergues ?? (_VerAlbergues = new DelegateCommand<string>(ExecuteVerAlbergues));

        void ExecuteVerAlbergues(string poblacion)
        {
            Console.WriteLine("DEBUG - CaminosVM - ExecuteVerAlbergues({0})", poblacion);
            Console.WriteLine("DEBUG - CaminosVM - ExecuteVerAlbergues  UriPath: {0}", _navigationService.GetNavigationUriPath());
            _navigationService.NavigateAsync("Ver?listado=albergues&poblacion=" + poblacion);           
        }


        private DelegateCommand _addCommand;
        public DelegateCommand AddCommand =>
            _addCommand ?? (_addCommand = new DelegateCommand(ExecuteAddCommand, CanExecuteAddCommand));

        void ExecuteAddCommand()
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteAddCommand");
        }

        bool CanExecuteAddCommand()
        {
            return true;
        }


        private DelegateCommand<TablaBaseCaminos> _LabelPulsada;
        public DelegateCommand<TablaBaseCaminos> LabelPulsada =>
            _LabelPulsada ?? (_LabelPulsada = new DelegateCommand<TablaBaseCaminos>(ExecuteLabelPulsada));

        void ExecuteLabelPulsada(TablaBaseCaminos camino)
        {          
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteLabelPulsada()  nombre:{0}  esVisible:{1}  esEtapa:{2}", 
                camino.nombrePoblacion, camino.esVisible, camino.esEtapa);

            if (camino.IniBifurcacion)
                MasajearLista(camino.nombrePoblacion);
            else
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteLabelPulsada()  No hacemos nada porque NO es una bifurcación");

            /*
            if (camino.esEtapa == true)
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand() esEtapa estaba a TRUE. La fonemos a false");
            else
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand() esEtapa estaba a FALSE. La fonemos a true");

            camino.esEtapa = !camino.esEtapa;
            */
        }



        private DelegateCommand<string> _CheckPulsado;
        public DelegateCommand<string> CheckPulsado =>
            _CheckPulsado ?? (_CheckPulsado = new DelegateCommand<string>(ExecuteCheckPulsado));

        void ExecuteCheckPulsado(string id)
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado({0})", id);

            int indice;
            int id2int = int.Parse(id);

            TablaBaseCaminos buscar = new TablaBaseCaminos(id2int);

            indice = listaEtapas.IndexOf(buscar);

            if (indice < 0)
            {
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() retornamos antes de tiempo porque indice es {0}", indice);
                return;
            }

            TablaBaseCaminos item = listaEtapas[indice];
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado()  nombre:{0}  esVisible:{1}  esEtapa:{2}",
                item.nombrePoblacion, item.esVisible, item.esEtapa);

            if (item.esEtapa == false)
            {
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() esEtapa estaba a FALSE. indice:{0}", indice);
                item.esEtapa = true;
                // Ahora calculamos el valor del kilometraje de la etapa.
                for (int i=indice-1; i>=0; i--)
                {
                    if (listaEtapas[i].esEtapa == true)
                    {
                        item.acumuladoEtapa = item.acumulado - listaEtapas[i].acumulado;
                        Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() Localizado inferior i:{0}  acumuladoEtapa:{1}  restamos:{2}",
                                        i, item.acumuladoEtapa, listaEtapas[i].acumulado);
                        break;
                    }
                    if (i == 0)
                        item.acumuladoEtapa = item.acumulado;

                }

                int max = listaEtapas.Count;
                for (int i = indice + 1; i < max; i++)
                {
                    if (listaEtapas[i].esEtapa == true) // || listaEtapas[i].nodosSiguientes == "FIN_CAMINO")
                    {
                        //listaEtapas[i].acumuladoEtapa = listaEtapas[i].acumuladoEtapa - item.acumuladoEtapa;
                        listaEtapas[i].acumuladoEtapa = listaEtapas[i].acumulado - item.acumulado;
                        Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() Localizado siguiente i:{0}-{1}  acumuladoEtapa:{2}  restamos:{3}  max:{4}",
                                        i, listaEtapas[i].nombrePoblacion, listaEtapas[i].acumuladoEtapa, listaEtapas[i].acumulado, max);
                        break;
                    }
                    if (i == max)
                        listaEtapas[max - 1].acumuladoEtapa = listaEtapas[max - 1].acumulado - item.acumulado;

                }

            }
            else
            {
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() esEtapa estaba a TRUE. La ponemos a false");
                item.esEtapa = false;
                // Ahora recalculamos el valor del kilometraje de la siguiente etapa:            

                int max = listaEtapas.Count;
                for (int i = indice + 1; i < max; i++)
                {
                    if (listaEtapas[i].esEtapa == true)
                    {
                        listaEtapas[i].acumuladoEtapa = listaEtapas[i].acumuladoEtapa + item.acumuladoEtapa;
                        Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() Localizado siguiente i:{0}-{1}  acumuladoEtapa:{2}  restamos:{3}  max:{4}",
                                        i, listaEtapas[i].nombrePoblacion, listaEtapas[i].acumuladoEtapa, listaEtapas[i].acumulado, max);
                        break;
                    }
                    if (i == max)
                        listaEtapas[max - 1].acumuladoEtapa = listaEtapas[max - 1].acumulado - item.acumulado;
                }
                item.acumuladoEtapa = 0;
            }


            
        }

        private DelegateCommand<TablaBaseCaminos> _ItemTappedCommand;
        public DelegateCommand<TablaBaseCaminos> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<TablaBaseCaminos>(ExecuteItemTappedCommand));

        void ExecuteItemTappedCommand(TablaBaseCaminos camino)
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand({0})    caminoActual:{1}  entrar...", camino, caminoActual);
            //var navigationParams = new NavigationParameters();
            //navigationParams.Add("camino", camino);
            //_navigationService.NavigateAsync("EntryCAMINOS", navigationParams);

            //listaEtapas.Remove(camino);

            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand()  nombre:{0}  esVisible:{1}  esEtapa:{2}", 
                camino.nombrePoblacion, camino.esVisible, camino.esEtapa);

            //MasajearLista(camino.nombrePoblacion);

            if (camino.esEtapa == true)
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand() esEtapa estaba a TRUE. La fonemos a false");
            else
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand() esEtapa estaba a FALSE. La fonemos a true");

            camino.esEtapa = !camino.esEtapa;


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
            
            Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo(camino:{0})  navigationMode:{1}   caminoAnterior:{2}", 
                caminoActual == null ? "NULL" : caminoActual, navigationMode, caminoAnterior == null ? "NULL" : caminoAnterior);

            if (navigationMode == NavigationMode.Back)
            {
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: Como estamos en BACK, retornamos sin masajear la lista");
                return;
            }

            caminoActual = parameters.GetValue<string>("camino");

            MasajearLista(null);
        }

        public async Task<bool> RellenarLista()
        {
            Console.WriteLine("DEBUG2 - VerCaminoVM - RellenarLista   caminoActual:{0}  caminoAnterior:{1}",
                caminoActual == null ? "NULL" : caminoActual, caminoAnterior == null ? "NULL" : caminoAnterior);

            if (caminoActual == null)
            {
                Console.WriteLine("ERROR - VerCaminoVM - RellenarLista()  caminoActual es null !!!");
                return false;
            }

            if (caminoAnterior == null || caminoAnterior != caminoActual )
            {
                miLista = await App.Database.GetPoblacionesCamino(caminoActual);
                caminoAnterior = caminoActual;
                // Ahora tengo que mirar en qué poblaciones hay albergue:
                poblacionesConAlbergue = App.Database.GetPoblacionesConAlbergue(caminoActual);
                for (int i=0; i<miLista.Count; i++)
                {
                    //Console.WriteLine("DEBUG2 - VerCaminoVM - RellenarLista  esEtapa:{0}", miLista[i].esEtapa);
                    if (poblacionesConAlbergue.Contains(miLista[i].nombrePoblacion))
                        miLista[i].tieneAlbergue = true;
                }
            }

            //List<TablaBaseCaminos> miLista = await App.Database.GetEtapasCamino(caminoActual);
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

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista(): cambiarBifurcacionEn:{0}", 
                    cambiarBifurcacionEn == null ? "NULL" : cambiarBifurcacionEn);
            /*
            if (listaEtapas == null)
            {
                Console.WriteLine("ERROR - VerCaminoVM - MasajearLista(): listaEtapas es null !!");
                return;
            }
            */

            //Primero rellenamos de nuevo el respaldo de listaEtapas con todos los nodos:

            bool resp;
            resp = await RellenarLista();

            if (resp == false)
            { //if (listaEtapas == null)
                Console.WriteLine("ERROR - ###########----------#######    VerCaminoVM - MasajearLista(): RellenarLista devolvió false !!");
                return;
            }
            siguienteNodo = back_listaEtapas[0].nombrePoblacion; // Lo inicializamos con el nombre de la primera población.

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista() entrar  El primer nodo será siguienteNodo:{0}", siguienteNodo);

            // Lista que contendrá los nodos que habrá que eliminar de "listaEtapas" para no mostrarlos en la ListView:
            List<TablaBaseCaminos> borrar = new List<TablaBaseCaminos>();

            //if (primeraVez)           
                if (bifurcaciones == null)
                    bifurcaciones = new Dictionary<string, string>();

            double acumuladoEtapa = 0;

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

                    if (dataItem.esEtapa || dataItem.nodosSiguientes == "FIN_CAMINO")
                    {
                        dataItem.acumuladoEtapa = acumulado - acumuladoEtapa;
                        acumuladoEtapa = acumulado;
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

            int i = 0;
            foreach (var b in back_listaEtapas)
            {
                Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: i: {0}  id: {1}  nombre: {2}  tieneAlbergue: {3}", 
                    i++, b.id, b.nombrePoblacion, b.tieneAlbergue);                
            }

            listaEtapas = back_listaEtapas;
        }


     

        public VerCaminoViewModel(INavigationService navigationService)
        {
            Console.WriteLine("DEBUG - CONSTR - VerCaminoViewModel()  caminoActual:{0}  caminoAnterior:{1}",
                caminoActual == null ? "NULL" : caminoActual, caminoAnterior == null ? "NULL" : caminoAnterior);

            _navigationService = navigationService;
            
        }
    }

    /*
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
    */


    public class IntToColorStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string respuesta = "";
            if ((bool)value == false)
                respuesta = "#F4F7AE";
            else
                respuesta = "Cyan";

            //Console.WriteLine("DEBUG - IntToColorStringConverter:Convert  value: {0}   respuesta:{1}", (bool)value, respuesta);

            return respuesta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class SustituirPorCadenaVaciaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string respuesta = "";

            Console.WriteLine("DEBUG - SustituirPorCadenaVacia:Convert  value: {0}", (double)value);

            if ((double)value == 0)
                respuesta = "";
            else
                respuesta = String.Format("{0:0.0}", (double)value);

            Console.WriteLine("DEBUG - SustituirPorCadenaVacia:Convert  value: {0}   respuesta:{1}", (double)value, respuesta);

            return respuesta;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
