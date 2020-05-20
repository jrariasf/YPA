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

        string primerNodoEtapa = null, ultimoNodoEtapa = null;

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - PoblacionesVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool _mostrarCabecera;
        public bool mostrarCabecera
        {
            get { return _mostrarCabecera; }
            set
            {
                SetProperty(ref _mostrarCabecera, value);
                RaisePropertyChanged(nameof(mostrarCabecera));
            }
        }
        

        private MiCamino _miCamino;
        public MiCamino miCamino
        {
            get { return _miCamino; }
            set { SetProperty(ref _miCamino, value); }
        }


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

            string listadoEtapas = miCamino.DameStringListaEtapas();
            string listadoBifurcaciones = miCamino.DameCadenaBifurcaciones();

            if (miCamino.miNombreCamino == null || miCamino.miNombreCamino.Trim().Length == 0)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    App.Current.MainPage.DisplayAlert("Error", "Hay que dar un nombre a tu camino", "OK");
                });

                return;
            }
            
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() miNombreCamino:{0}",
                              miCamino.miNombreCamino == null ? "NULL" : miCamino.miNombreCamino);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() descripcion:{0}",
                              miCamino.descripcion == null ? "NULL" : miCamino.descripcion);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() caminoBase:{0}",
                              miCamino.caminoActual == null ? "NULL" : miCamino.caminoActual);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() fechaInicio:{0}",
                              miCamino.fechaInicio == null ? "NULL" : miCamino.fechaInicio);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() bifurcaciones:{0}", listadoBifurcaciones);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() etapas:{0}", listadoEtapas);
            

            TablaMisCaminos tmc = new TablaMisCaminos(miCamino.miNombreCamino, miCamino.descripcion, miCamino.caminoActual,
                                                      miCamino.fechaInicio, listadoBifurcaciones, listadoEtapas);

            Global.subetapasModificadas = listadoEtapas;

            await App.Database.SaveMiCaminoAsync(tmc);

        }


        private DelegateCommand _VerResumenCamino;
        public DelegateCommand VerResumenCamino =>
            _VerResumenCamino ?? (_VerResumenCamino = new DelegateCommand(ExecuteVerResumenCamino));

        void ExecuteVerResumenCamino()
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteVerResumenCamino()");

            DialogParameters p = new DialogParameters();

            p.Add("message", "Listado de etapas:");
            p.Add("miCamino", miCamino);
            p.Add("resumen", resumen); // Es una cadena con el número de etapas y los kilómetros totales

            _dialogService.ShowDialog("DialogoMiCamino", p);

            int valor;
            int res = Global.FuncionGlobal(21, out valor);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteVerResumenCamino()  res:{0}   valor:{0}", res, valor);

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


        private DelegateCommand _GuardarCambios;
        public DelegateCommand GuardarCambios =>
            _GuardarCambios ?? (_GuardarCambios = new DelegateCommand(ExecuteGuardarCambios, CanExecuteGuardarCambios));

        void ExecuteGuardarCambios()
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCambios");

        }

        bool CanExecuteGuardarCambios()
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
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteLabelPulsada() primerNodoEtapa <{0}>   ultimoNodoEtapa <{1}>",
                      primerNodoEtapa == null ? "NULL" : primerNodoEtapa, ultimoNodoEtapa == null ? "NULL" : ultimoNodoEtapa);
                listaPuntosDePaso = miCamino.MasajearLista(camino.nombrePoblacion, primerNodoEtapa, ultimoNodoEtapa);
            }
            else
                Console.WriteLine("DEBUG - VerCaminoVM - ExecuteLabelPulsada()  No hacemos nada porque NO es una bifurcación");

        }


        private DelegateCommand<TablaBaseCaminos> _CheckPulsado;
        public DelegateCommand<TablaBaseCaminos> CheckPulsado =>
            _CheckPulsado ?? (_CheckPulsado = new DelegateCommand<TablaBaseCaminos>(ExecuteCheckPulsado, CanExecuteCheckPulsado));

        void ExecuteCheckPulsado(TablaBaseCaminos tbc)
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteCheckPulsado({0})", tbc.id);

            miCamino.ExecuteCheckPulsado(tbc.id);

        }

        bool CanExecuteCheckPulsado(TablaBaseCaminos tbc)
        {
            return tbc.checkboxEnabled;
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
            var navigationMode = parameters.GetNavigationMode();
            Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedFrom()  navigationMode:{0}", navigationMode);
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
            int option;
            var navigationMode = parameters.GetNavigationMode();

            if (miCamino != null)
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo(caminoActual:{0})  navigationMode:{1}   caminoAnterior:{2}",
                         miCamino.caminoActual == null ? "NULL" : miCamino.caminoActual, navigationMode,
                         miCamino.caminoAnterior == null ? "NULL" : miCamino.caminoAnterior);
            else
            {
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo  miCamino es NULL");
                miCamino = new MiCamino();
            }

            if (navigationMode == NavigationMode.Back)
            {
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: Como estamos en BACK, retornamos sin masajear la lista");
                return;
            }

            option = parameters.GetValue<int>("option");
            //string camino = parameters.GetValue<string>("camino");
            
            mostrarCabecera = true;

            switch (option)
            {
                case 1: // Recibimos el nombre de un camino para visualizar todos sus puntos de paso:
                    Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo: option 1");
                    string camino = parameters.GetValue<string>("camino");
                    if (camino != null)
                    {
                        miCamino.caminoActual = camino;
                        Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo: Llegamos normalmente, desde el menú CAMINOS  camino:{0}", miCamino.caminoActual);
                        listaPuntosDePaso = miCamino.MasajearLista();
                    } else
                    {
                        Console.WriteLine("DEBUG3 - VerCaminoVM - OnNavigatedTo: Error en las opciones pasadas, falta el nombre del camino");
                        return;
                    }
                    break;
                case 2: // Recibimos un tmc que es un camino configurado por el usuario:
                    Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo: option 2");
                    TablaMisCaminos tmc = parameters.GetValue<TablaMisCaminos>("tmc");
                    if (tmc != null)
                    {
                        Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo: Venimos de MisCaminos con tmc");
                        miCamino.Init(tmc);
                        Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo: tmc.bifurcaciones:{0}", tmc.bifurcaciones);
                        listaPuntosDePaso = miCamino.MasajearLista();
                    } else
                    {
                        Console.WriteLine("DEBUG3 - VerCaminoVM - OnNavigatedTo: Error en las opciones pasadas, falta el tmc");
                        return;
                    }
                    break;
                case 3: // Sólo se quieren mostrar los puntos de paso de la etapa que nos dicen:
                    Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo: option 3");
                    mostrarCabecera = false;
                    MiCamino miCam = parameters.GetValue<MiCamino>("miCamino");
                    
                    if (miCam != null)
                    {
                        miCam.caminoAnterior = null; // Para forzar que al llamar a RellenarLista se recree el miLista.
                        //string primerNodoEtapa = null, ultimoNodoEtapa = null;
                        primerNodoEtapa = parameters.GetValue<string>("primerNodoEtapa");
                        ultimoNodoEtapa = parameters.GetValue<string>("ultimoNodoEtapa");
                        Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo: Venimos de VerEtapas con miCamino primerNodoEtapa <{0}>  ultimoNodoEtapa <{1}>",
                                          primerNodoEtapa == null ? "NULL" : primerNodoEtapa, ultimoNodoEtapa == null ? "NULL" : ultimoNodoEtapa);
                        //miCamino.Init(tmc);
                        miCamino = miCam;
                        //miCamino.backupEtapas = miCamino.etapas; //_xx_ETAPAS: Guardamos las etapas del camino completo porque al llamar a MasajearLista se va a recrear la cadena de etapas con los nodos de esa etapa !!!
                        Console.WriteLine("DEBUG - VerCaminoVM - OnNavigatedTo: miCamino.bifurcaciones:{0}", miCamino.DameCadenaBifurcaciones());
                        listaPuntosDePaso = miCamino.MasajearLista(null, primerNodoEtapa, ultimoNodoEtapa);
                    }
                    else
                    {
                        Console.WriteLine("ERROR - VerCaminoVM - OnNavigatedTo: Error en las opciones pasadas, falta el miCamino");
                        return;
                    }
                    break;
                default:
                    Console.WriteLine("DEBUG3 - VerCaminoVM - OnNavigatedTo: OPCIÓN NO CONTEMPLADA");
                    break;
            }
         
        }


        public VerCaminoViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            if (miCamino != null)
               Console.WriteLine("DEBUG - CONSTR - VerCaminoViewModel()  caminoActual:{0}  caminoAnterior:{1}",
                miCamino.caminoActual == null ? "NULL" : miCamino.caminoActual,
                miCamino.caminoAnterior == null ? "NULL" : miCamino.caminoAnterior);
            else
                Console.WriteLine("DEBUG - CONSTR - VerCaminoViewModel()  miCamino es NULL");

            _navigationService = navigationService;
            _dialogService = dialogService;

            culture = new CultureInfo("en-US");

            DateTime hoy = System.DateTime.Today;

            mostrarCabecera = true;

            miCamino = new MiCamino();
            miCamino.fechaInicio = hoy.ToString("yyyy-MM-dd");

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
