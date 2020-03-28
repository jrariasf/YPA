using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
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

        public static CultureInfo culture;
        private IDialogService _dialogService { get; }

        //_xx_ public static char[] separador = { ';' };
        //_xx_ public static char[] separadorDePares = { '#' };

        string caminoActual = null;
        string caminoAnterior = null;

        //List<string> poblacionesConAlbergue = null;

        //_xx_ int numEtapas = 0;

        Dictionary<string, string> bifurcaciones = null; // new Dictionary<string, string>();
        

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - PoblacionesVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<TablaBaseCaminos> miLista;
        /*_xx_
        private List<TablaBaseCaminos> _miLista;
        public List<TablaBaseCaminos> miLista {
            get { return _miLista; }            
            set
            {
                Console.WriteLine("DEBUG3 - VerCaminoVM - Cambio en miLista !!!");
                if (_miLista == value)
                    return;
                SetProperty(ref _miLista, value);
                RaisePropertyChanged(nameof(miLista));
            }
        }
        */

        private ObservableCollection<TablaBaseCaminos> _listaPuntosDePaso;
        public ObservableCollection<TablaBaseCaminos> listaPuntosDePaso
        {
            get { return _listaPuntosDePaso; }
            ///set { SetProperty(ref _listaPoblaciones, value); }
            set
            {
                if (_listaPuntosDePaso == value)
                    return;
                SetProperty(ref _listaPuntosDePaso, value);
                RaisePropertyChanged(nameof(listaPuntosDePaso));
            }
        }

        //_xx_ ObservableCollection<TablaBaseCaminos> back_listaPuntosDePaso;


        private string _fechaInicio;
        public string fechaInicio
        {
            get { return _fechaInicio; }
            set
            { 
                SetProperty(ref _fechaInicio, value);
                RaisePropertyChanged(nameof(fechaInicio));
            }
        }

        private string _miNombreCamino;
        public string miNombreCamino
        {
            get { return _miNombreCamino; }
            set
            { 
                SetProperty(ref _miNombreCamino, value);
                RaisePropertyChanged(nameof(miNombreCamino));
            }
        }

        private string _descripcion;
        public string descripcion
        {
            get { return _descripcion; }
            set
            {
                SetProperty(ref _descripcion, value);
                RaisePropertyChanged(nameof(descripcion));
            }
        }

        private string _resumen;
        public string resumen
        {
            get { return _resumen; }
            set
            {
                SetProperty(ref _resumen, value);
                RaisePropertyChanged(nameof(resumen));
            }
        }


        private DelegateCommand _GuardarCamino;
        public DelegateCommand GuardarCamino =>
            _GuardarCamino ?? (_GuardarCamino = new DelegateCommand(ExecuteGuardarCamino));

        async void ExecuteGuardarCamino()
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino()");
            /*
            _dialogService.ShowDialog("DialogoMiCamino", new DialogParameters
            {
                { "message", "Hello from hell !!" }
            });
            */

            string listadoEtapas = Global.DameListaEtapas(ref miLista);
            string listadoBifurcaciones = Global.DameCadenaBifurcaciones(ref bifurcaciones);

            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() miNombreCamino:{0}", miNombreCamino );
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() descripcion:{0}", descripcion);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() caminoBase:{0}", caminoActual);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() fechaInicio:{0}", fechaInicio);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() bifurcaciones:{0}", listadoBifurcaciones);    
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() etapas:{0}", listadoEtapas);

            TablaMisCaminos tmc = new TablaMisCaminos(miNombreCamino, descripcion, caminoActual, fechaInicio, listadoBifurcaciones, listadoEtapas);

            await App.Database.SaveMiCaminoAsync(tmc);
        }

        /* Estas funciones pasan al Global:
        string DameListaEtapas()
        {
            string listado = "";
            foreach (var item in miLista)
            {
                if (item.esEtapa && item.esVisible)
                {
                    listado += item.nombrePoblacion;
                    listado += VerCaminoViewModel.separador[0]; // ";";
                }
            }
            return listado;
        }
        void SetEtapasInLista(string listadoEtapas)  // listado es una secuencia de poblaciones separadas por ";". Obligatorio que acabe en ";" !!!
        {
            //string[] etapas = listado.Split(VerCaminoViewModel.separador);
            foreach (var item in miLista)    //  <<<<<<<<<<<<<<<<<<<<<<<< MIRAR A VER POR QUÉ miLista ES NULL !!!!!!!!
            {
                if (listadoEtapas.Contains(item.nombrePoblacion + ";"))
                    item.esEtapa = true;
                else
                    item.esEtapa = false;
            }
        }

        string DameCadenaBifurcaciones()
        {
            string listadoBifurcaciones = "";
            foreach (KeyValuePair<string, string> kvp in bifurcaciones)
            {
                //Console.WriteLine("DEBUG3 - VerCaminoVM - DameCadenaBifurcaciones: bifurcaciones: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                listadoBifurcaciones += kvp.Key;
                listadoBifurcaciones += VerCaminoViewModel.separadorDePares[0]; // "#"
                listadoBifurcaciones += kvp.Value;
                listadoBifurcaciones += VerCaminoViewModel.separador[0]; // ";"
            }
            return listadoBifurcaciones;
        }
        void SetBifurcaciones(string listadoBifurcaciones)
        {            
            if (bifurcaciones == null)
                bifurcaciones = new Dictionary<string, string>();
            //else
            //    bifurcaciones.Clear();

            if (listadoBifurcaciones != null)
            {
                Console.WriteLine("DEBUG3 - VerCaminoVM - SetBifurcaciones listadoBifurcaciones:<{0}>", listadoBifurcaciones);
                bifurcaciones.Clear();
                string[] arrayBifurcaciones = listadoBifurcaciones.Split(VerCaminoViewModel.separador, StringSplitOptions.RemoveEmptyEntries);
                foreach (var bifurcacion in arrayBifurcaciones)
                {
                    string[] par = bifurcacion.Split(VerCaminoViewModel.separadorDePares);
                    bifurcaciones.Add(par[0], par[1]);
                }
            }
        }
        */


        private DelegateCommand _VerResumenCamino;
        public DelegateCommand VerResumenCamino =>
            _VerResumenCamino ?? (_VerResumenCamino = new DelegateCommand(ExecuteVerResumenCamino));

        void ExecuteVerResumenCamino()
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteVerResumenCamino()");

            DialogParameters p = new DialogParameters();

            p.Add("message", "Listado de etapas:");
            p.Add("lista", miLista);
            p.Add("fechaInicio", fechaInicio);
            p.Add("resumen", resumen); // Es una cadena con el número de etapas y los kilómetros totales

            _dialogService.ShowDialog("DialogoMiCamino", p);

            int valor;
            int res = Global.FuncionGlobal(21, out valor);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteVerResumenCamino()  res:{0}   valor:{0}", res, valor);

            /*
            _dialogService.ShowDialog("DialogoMiCamino", new DialogParameters
            {
                { "message", "Hello from hell" }
            });
            */
        }



        private DelegateCommand<string> _OnCheckedChanged;
        public DelegateCommand<string> OnCheckedChanged =>
            _OnCheckedChanged ?? (_OnCheckedChanged = new DelegateCommand<string>(ExecuteOnCheckedChanged));

        void ExecuteOnCheckedChanged(string poblacion)
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteOnCheckedChanged poblacion: {0}", poblacion);
            //if (listaPuntosDePaso != null)
            //    listaPuntosDePaso[poblacion] = 
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
            {
                string _resumen;
                listaPuntosDePaso = Global.MasajearLista(ref miLista, ref bifurcaciones, ref caminoActual, ref caminoAnterior, out _resumen, camino.nombrePoblacion);
                resumen = _resumen;
            }
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
            int id2int = int.Parse(id, culture);

            TablaBaseCaminos buscar = new TablaBaseCaminos(id2int);

            indice = listaPuntosDePaso.IndexOf(buscar);

            if (indice < 0)
            {
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() retornamos antes de tiempo porque indice es {0}", indice);
                return;
            }

            TablaBaseCaminos item = listaPuntosDePaso[indice];
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado()  nombre:{0}  esVisible:{1}  esEtapa:{2}",
                item.nombrePoblacion, item.esVisible, item.esEtapa);

            if (item.esEtapa == false) // Se marca como etapa:
            {
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() esEtapa estaba a FALSE. indice:{0}", indice);
                item.esEtapa = true;
                // Ahora calculamos el valor del kilometraje de la etapa.
                for (int i=indice-1; i>=0; i--)
                {
                    if (listaPuntosDePaso[i].esEtapa == true)
                    {
                        item.acumuladoEtapa = item.acumulado - listaPuntosDePaso[i].acumulado;
                        Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() Localizado inferior i:{0}  acumuladoEtapa:{1}  restamos:{2}",
                                        i, item.acumuladoEtapa, listaPuntosDePaso[i].acumulado);
                        break;
                    }
                    if (i == 0) // Quiere decir que es la primera etapa marcada:
                    {
                        Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() Se trata de la PRIMERA ETAPA !! [{0}]", item.nombrePoblacion);
                        item.acumuladoEtapa = 0;
                        //item.acumuladoEtapa = item.acumulado;
                    }

                }

                int max = listaPuntosDePaso.Count;
                for (int i = indice + 1; i < max; i++)
                {
                    if (listaPuntosDePaso[i].esEtapa == true) // || listaPuntosDePaso[i].nodosSiguientes == "FIN_CAMINO")
                    {
                        //listaPuntosDePaso[i].acumuladoEtapa = listaPuntosDePaso[i].acumuladoEtapa - item.acumuladoEtapa;
                        listaPuntosDePaso[i].acumuladoEtapa = listaPuntosDePaso[i].acumulado - item.acumulado;
                        Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() Localizado siguiente i:{0}-{1}  acumuladoEtapa:{2}  restamos:{3}  max:{4}",
                                        i, listaPuntosDePaso[i].nombrePoblacion, listaPuntosDePaso[i].acumuladoEtapa, listaPuntosDePaso[i].acumulado, max);
                        break;
                    }

                    /* De momento comentamos lo que viene porque dejamos de considerar por defecto la última población del camino como si fuera la última etapa:
                    if (i == max) // Se ha recorrido toda la lista sin encontrar más PuntosDePaso:
                        listaPuntosDePaso[max - 1].acumuladoEtapa = listaPuntosDePaso[max - 1].acumulado - item.acumulado;
                    */

                }

            }
            else // Se desmarca esa etapa:
            {
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() esEtapa estaba a TRUE. La ponemos a false");
                item.esEtapa = false;
                // Ahora recalculamos el valor del kilometraje de la siguiente etapa:            

                // Primero miramos si la población actual era la primera etapa marcada:
                bool eraPrimeraEtapa = item.acumuladoEtapa == 0 ? true : false;
                int max = listaPuntosDePaso.Count;
                for (int i = indice + 1; i < max; i++)
                {
                    if (listaPuntosDePaso[i].esEtapa == true)
                    {
                        //listaPuntosDePaso[i].acumuladoEtapa = listaPuntosDePaso[i].acumuladoEtapa + item.acumuladoEtapa;
                        listaPuntosDePaso[i].acumuladoEtapa = eraPrimeraEtapa ? 0 : listaPuntosDePaso[i].acumuladoEtapa + item.acumuladoEtapa;
                        Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado() Localizado siguiente i:{0}-{1}  acumuladoEtapa:{2}  restamos:{3}  max:{4}",
                                        i, listaPuntosDePaso[i].nombrePoblacion, listaPuntosDePaso[i].acumuladoEtapa, listaPuntosDePaso[i].acumulado, max);
                        break;
                    }

                    /* De momento comentamos lo que viene porque dejamos de considerar por defecto la última población del camino como si fuera la última etapa:
                    if (i == max)
                        listaPuntosDePaso[max - 1].acumuladoEtapa = listaPuntosDePaso[max - 1].acumulado - item.acumulado;
                    */
                }
                item.acumuladoEtapa = 0;
            }

            //_xx_ resumen = "[ " + numEtapas.ToString() + " etapas, " + distanciaTotal.ToString(CultureInfo.CreateSpecificCulture("es-ES")) + " km ]";


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

            //listaPuntosDePaso.Remove(camino);

            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand()  nombre:{0}  esVisible:{1}  esEtapa:{2}", 
                camino.nombrePoblacion, camino.esVisible, camino.esEtapa);

            //MasajearLista(camino.nombrePoblacion);

            if (camino.esEtapa == true)
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand() esEtapa estaba a TRUE. La ponemos a false");
            else
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand() esEtapa estaba a FALSE. La ponemos a true");

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

        public void OnNavigatedTo(INavigationParameters parameters)
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

            TablaMisCaminos tmc = parameters.GetValue<TablaMisCaminos>("tmc");

            if (caminoActual != null)
            {
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: Llegamos normalmente, desde el menú CAMINOS");
                //_xx_ listaPuntosDePaso = MasajearLista();
                string _resumen;
                listaPuntosDePaso = Global.MasajearLista(ref miLista, ref bifurcaciones, ref caminoActual, ref caminoAnterior, out _resumen);
                resumen = _resumen;
            } else if (tmc != null)
            {
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: Venimos de MisCaminos con tmc");
                caminoActual = tmc.caminoBase;
                fechaInicio = tmc.dia.ToString("yyyy-MM-dd");
                miNombreCamino = tmc.miNombreCamino;
                descripcion = tmc.descripcion;
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: tmc.bifurcaciones:{0}", tmc.bifurcaciones);
                //SetBifurcaciones(tmc.bifurcaciones);
                //_xx_ listaPuntosDePaso = MasajearLista(null, tmc.bifurcaciones, tmc.etapas);
                string _resumen;
                listaPuntosDePaso = Global.MasajearLista(ref miLista, ref bifurcaciones, ref caminoActual, ref caminoAnterior, out _resumen, null, tmc.bifurcaciones, tmc.etapas);
                resumen = _resumen;
                //SetEtapasInLista(tmc.etapas);               
            } else
            {
                Console.WriteLine("ERROR - VerCaminoVM - OnNavigatedTo: OPCIÓN NO CONTEMPLADA");
            }

            
        }


        /*
        public async Task<bool> RellenarLista()
        {
            return await RellenarLista(null);
        }

        public async Task<bool> RellenarLista(string listadoEtapas=null)
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
                miLista = await App.Database.GetPoblacionesCaminoAsync(caminoActual);
                caminoAnterior = caminoActual;
                // Ahora tengo que mirar en qué poblaciones hay albergue:
                List<string> poblacionesConAlbergue = null;
                poblacionesConAlbergue = App.Database.GetPoblacionesConAlbergue(caminoActual);
                for (int i=0; i<miLista.Count; i++)
                {
                    //Console.WriteLine("DEBUG2 - VerCaminoVM - RellenarLista  esEtapa:{0}", miLista[i].esEtapa);
                    if (poblacionesConAlbergue.Contains(miLista[i].nombrePoblacion))
                        miLista[i].tieneAlbergue = true;

                    if (listadoEtapas != null)
                        if (listadoEtapas.Contains(miLista[i].nombrePoblacion + ";"))
                            miLista[i].esEtapa = true;
                        else
                            miLista[i].esEtapa = false;
                }
            }
            
            back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(miLista);

            return true;
        }
        */


        /*
        async void MasajearLista()
        {
            await MasajearLista(null, null, null);
        }
        async Task MasajearLista(string cambiarBifurcacionEn=null, string listadoBifurcaciones=null, string listadoEtapas=null)
        {
            bool estamosEnBifurcacion = false;
            double acumulado = 0;
            double distanciaTotal;
            string siguienteNodo;            

            bool esPrimeraEtapa = true;
            int numEtapas = 0;        

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista(): cambiarBifurcacionEn:{0}", 
                    cambiarBifurcacionEn == null ? "NULL" : cambiarBifurcacionEn);

            //Primero rellenamos de nuevo el respaldo de listaPuntosDePaso con todos los nodos:

            bool resp;
            ObservableCollection<TablaBaseCaminos> back_listaPuntosDePaso;
            //resp = await  RellenarLista(listadoEtapas); // Cambio esta línea por las siguientes:
            resp = Global.RellenarLista(ref miLista, ref caminoActual, ref caminoAnterior, listadoEtapas);
            if (resp == false)
            {
                Console.WriteLine("ERROR - ###########----------#######    VerCaminoVM - MasajearLista(): RellenarLista devolvió false !!");
                return;
            }
            back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(miLista);

            
            siguienteNodo = back_listaPuntosDePaso[0].nombrePoblacion; // Lo inicializamos con el nombre de la primera población.

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista() entrar  El primer nodo será siguienteNodo:{0}", siguienteNodo);

            // Lista que contendrá los nodos que habrá que eliminar de "listaPuntosDePaso" para no mostrarlos en la ListView:
            List<TablaBaseCaminos> borrar = new List<TablaBaseCaminos>();


            Global.SetBifurcaciones(ref bifurcaciones, listadoBifurcaciones);

            double acumuladoEtapa = 0;

            foreach (var item in back_listaPuntosDePaso)
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
                    string[] distancias = dataItem.distanciaNodosSiguientes.Split(Global.separador);
                    string[] nodosSiguientes = dataItem.nodosSiguientes.Split(Global.separador);

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
                    

                    if (dataItem.esEtapa) // || dataItem.nodosSiguientes == "FIN_CAMINO")
                    {
                        if (esPrimeraEtapa)
                        {
                            dataItem.acumuladoEtapa = 0;
                            esPrimeraEtapa = false;
                            numEtapas = 0;
                        }
                        else
                        {
                            dataItem.acumuladoEtapa = acumulado - acumuladoEtapa;
                            numEtapas++;
                        }

                        acumuladoEtapa = acumulado;                        
                    }

                    dataItem.acumulado = acumulado;
                    acumulado += double.Parse(distancias[indice], culture);
                    siguienteNodo = nodosSiguientes[indice];

                    Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: siguienteNodo {0}   acumulado:{1}", 
                                siguienteNodo, acumulado);
                }                

            }

            foreach (KeyValuePair<string, string> kvp in bifurcaciones)
            {
                Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: bifurcaciones: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }

            
            //Ahora relleno el campo "distanciaAlFinal" que es el que dice los kms que restan hasta el final:
            distanciaTotal = acumulado;
            resumen = "[ " + numEtapas.ToString() + " etapas, " + distanciaTotal.ToString(CultureInfo.CreateSpecificCulture("es-ES")) + " km ]";

            foreach (var item in back_listaPuntosDePaso)
            {
                var dataItem = (TablaBaseCaminos)item;
                if (dataItem.esVisible == true)
                {
                    dataItem.distanciaAlFinal = distanciaTotal - dataItem.acumulado;

                }
            }


            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Número de nodos en back_listaPuntosDePaso:{0}", back_listaPuntosDePaso.Count());
            if (borrar.Count() > 0)
                foreach (var b in borrar)
                {
                    Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Borramos {0}", b.nombrePoblacion);
                    back_listaPuntosDePaso.Remove(b);
                }

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Número de nodos final en back_listaPuntosDePaso:{0}", back_listaPuntosDePaso.Count());

            int i = 0;
            foreach (var b in back_listaPuntosDePaso)
            {
                Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: i: {0}  id: {1}  nombre: {2}  tieneAlbergue: {3}", 
                    i++, b.id, b.nombrePoblacion, b.tieneAlbergue);                
            }

            listaPuntosDePaso = back_listaPuntosDePaso;
        }
        */




        public VerCaminoViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            Console.WriteLine("DEBUG - CONSTR - VerCaminoViewModel()  caminoActual:{0}  caminoAnterior:{1}",
                caminoActual == null ? "NULL" : caminoActual, caminoAnterior == null ? "NULL" : caminoAnterior);

            _navigationService = navigationService;
            _dialogService = dialogService;

            culture = new CultureInfo("en-US");

            DateTime hoy = System.DateTime.Today;
            /*
            DateTime mañana;
            TimeSpan ts = new TimeSpan(1, 0, 0, 0);
            mañana = hoy + ts;
            fechaInicio = mañana.ToString("yyyy-MM-dd");
            */

            fechaInicio = hoy.ToString("yyyy-MM-dd");

            miLista = null;

        }

    }

  
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
