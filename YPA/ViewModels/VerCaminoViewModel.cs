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
        

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - PoblacionesVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
            /*
            _dialogService.ShowDialog("DialogoMiCamino", new DialogParameters
            {
                { "message", "Hello from hell !!" }
            });
            */

            string listadoEtapas = miCamino.DameListaEtapas();
            string listadoBifurcaciones = miCamino.DameCadenaBifurcaciones();

            //_xx_PENDIENTE:  Mirar los campos de abajo como "fechaInicio" que igual hay que coger lo que tenemos en miCamino:
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() miNombreCamino:{0}", miCamino.miNombreCamino);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() descripcion:{0}", miCamino.descripcion);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() caminoBase:{0}", miCamino.caminoActual);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() fechaInicio:{0}", miCamino.fechaInicio);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() bifurcaciones:{0}", listadoBifurcaciones);
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteGuardarCamino() etapas:{0}", listadoEtapas);

            TablaMisCaminos tmc = new TablaMisCaminos(miCamino.miNombreCamino, miCamino.descripcion, miCamino.caminoActual,
                                                      miCamino.fechaInicio, listadoBifurcaciones, listadoEtapas);

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
            p.Add("lista", miCamino.miLista); //_xx_
            p.Add("fechaInicio", miCamino.fechaInicio);
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
                listaPuntosDePaso = miCamino.MasajearLista(camino.nombrePoblacion);
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

            miCamino.ExecuteCheckPulsado(id);

        }

        private DelegateCommand<TablaBaseCaminos> _ItemTappedCommand;
        public DelegateCommand<TablaBaseCaminos> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<TablaBaseCaminos>(ExecuteItemTappedCommand));

        void ExecuteItemTappedCommand(TablaBaseCaminos camino)
        {
            Console.WriteLine("DEBUG - VerCaminoVM - ExecuteItemTappedCommand({0})    caminoActual:{1}  entrar...", camino, miCamino.caminoActual);
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

            string camino = parameters.GetValue<string>("camino");

            TablaMisCaminos tmc = parameters.GetValue<TablaMisCaminos>("tmc");

            if (camino != null)
            {
                miCamino.caminoActual = camino;
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: Llegamos normalmente, desde el menú CAMINOS  camino:{0}", miCamino.caminoActual);
                listaPuntosDePaso = miCamino.MasajearLista();
            } else if (tmc != null)
            {
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: Venimos de MisCaminos con tmc");
                miCamino.Init(tmc);                
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: tmc.bifurcaciones:{0}", tmc.bifurcaciones);            
                listaPuntosDePaso = miCamino.MasajearLista();           
            } else
            {
                Console.WriteLine("ERROR - VerCaminoVM - OnNavigatedTo: OPCIÓN NO CONTEMPLADA");
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
            /*
            DateTime mañana;
            TimeSpan ts = new TimeSpan(1, 0, 0, 0);
            mañana = hoy + ts;
            fechaInicio = mañana.ToString("yyyy-MM-dd");
            */

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
