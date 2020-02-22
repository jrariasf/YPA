using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using YPA.Models;

namespace YPA.ViewModels
{
    public class PoblacionesViewModel : BindableBase, INavigationAware, INotifyPropertyChanged
    {
        INavigationService _navigationService;

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - PoblacionesVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<TablaPOBLACIONES> _listaPoblaciones;
        public ObservableCollection<TablaPOBLACIONES> listaPoblaciones
        {
            get { return _listaPoblaciones; }
            ///set { SetProperty(ref _listaPoblaciones, value); }
            set
            {
                if (_listaPoblaciones == value)
                    return;
                SetProperty(ref _listaPoblaciones, value);
                RaisePropertyChanged(nameof(listaPoblaciones));
            }
        }


        private DelegateCommand<string> _PoblacionTocada;
        public DelegateCommand<string> PoblacionTocada =>
            _PoblacionTocada ?? (_PoblacionTocada = new DelegateCommand<string>(ExecutePoblacionTocada));

        void ExecutePoblacionTocada(string id)
        {
            Console.WriteLine("DEBUG - PoblacionesVM - ExecutePoblacionTocada({0})", id == null ? "id es NULL" : id);
            _navigationService.NavigateAsync("EntryPOBLACIONES");

        }

        private DelegateCommand<string> _AddPoblacionClicked;
        public DelegateCommand<string> AddPoblacionClicked =>
            _AddPoblacionClicked ?? (_AddPoblacionClicked = new DelegateCommand<string>(ExecuteAddPoblacionClicked));

        void ExecuteAddPoblacionClicked(string parameter)
        {
            Console.WriteLine("DEBUG - PoblacionesVM - ExecuteAddPoblacionClicked({0})", parameter);
            Console.WriteLine("DEBUG - PoblacionesVM - ExecuteAddPoblacionClicked  UriPath: {0}", _navigationService.GetNavigationUriPath());
            _navigationService.NavigateAsync("EntryPOBLACIONES");
        }

        private DelegateCommand<TablaPOBLACIONES> _ItemTappedCommand;
        public DelegateCommand<TablaPOBLACIONES> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<TablaPOBLACIONES>(ExecuteItemTappedCommand));

        void ExecuteItemTappedCommand(TablaPOBLACIONES poblacion)
        {
            Console.WriteLine("DEBUG - PoblacionesVM - ExecuteItemTappedCommand({0})  entrar...", poblacion);
            var navigationParams = new NavigationParameters();
            navigationParams.Add("poblacion", poblacion);
            _navigationService.NavigateAsync("EntryPOBLACIONES", navigationParams);
        }

        private DelegateCommand<string> _VerAlojamientosDePoblacion;
        public DelegateCommand<string> VerAlojamientosDePoblacion =>
            _VerAlojamientosDePoblacion ?? (_VerAlojamientosDePoblacion = new DelegateCommand<string>(ExecuteVerAlojamientosDePoblacion));

        void ExecuteVerAlojamientosDePoblacion(string id)
        {
            Console.WriteLine("DEBUG - PoblacionesVM - ExecuteVerAlojamientosDePoblacion({0})", id);
            //string query = "select * from TablaALOJAMIENTOS where ";
            //List<TablaALOJAMIENTOS> miLista = await App.Database.GetAlojamientosByCityAsync(); // QueryAsync<TablaPOBLACIONES>(query);            
            //listaAlojamientos = new ObservableCollection<TablaALOJAMIENTOS>(miLista);
            _navigationService.NavigateAsync("Ver?listado=albergues&idPoblacion=" + id);
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
            Console.WriteLine("DEBUG2 - PoblacionesVM - OnNavigatedTo()  navigationMode:{0}", navigationMode);
            
            /* Esto que viene ya no lo hago porque he conseguido que al cambiar algún campo de TablaALOJAMIENTOS, el UI se entere y actualice el valor:
            if (navigationMode == NavigationMode.Back)
            {
                Console.WriteLine("DEBUG2 - PoblacionesVM - OnNavigatedTo() Vamos a recargar el listaPoblaciones...");
                CargarPoblacionesAsync();
            }
            */
            
            string poblacion = parameters.GetValue<string>("poblacion");
            Console.WriteLine("DEBUG2 - PoblacionesVM - OnNavigatedTo(poblacion:{0})", poblacion);

            if (navigationMode == NavigationMode.Back)
            {
                Console.WriteLine("DEBUG2 - VerCaminoVM - OnNavigatedTo: Como estamos en BACK, retornamos sin mas");
                return;
            }

            CargarPoblacionesAsync(poblacion);

        }



        async void CargarPoblacionesAsync(string poblacion)
        {
            Console.WriteLine("DEBUG - PoblacionesVM - CargarPoblacionesAsync  poblacion: {0}",
                poblacion == null ? "NULL" : poblacion);

            List<TablaPOBLACIONES> miLista;

            if (poblacion == null)
                miLista = await App.Database.GetPoblacionesAsync();
            else
                miLista = await App.Database.DamePoblacionesPorNombre(poblacion);

            listaPoblaciones = new ObservableCollection<TablaPOBLACIONES>(miLista);
        }

        public PoblacionesViewModel(INavigationService navigationService)
        {
            Console.WriteLine("DEBUG - CONSTR - PoblacionesViewModel()");
            _navigationService = navigationService;

            //CargarPoblacionesAsync(null);
        }
    }
}
