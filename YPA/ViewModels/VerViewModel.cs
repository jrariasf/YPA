using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using YPA.Models;
using YPA.Views;


namespace YPA.ViewModels
{
    public class VerViewModel : BindableBase, INavigationAware
    {
        private ObservableCollection<TablaALOJAMIENTOS> _listaAlojamientos;
        public ObservableCollection<TablaALOJAMIENTOS> listaAlojamientos
        {
            get { return _listaAlojamientos; }
            set { SetProperty(ref _listaAlojamientos, value); }
        }

        INavigationService _navigationService;
        private string _nombreBoton;
        public string nombreBoton
        {
            get { return _nombreBoton; }
            set { SetProperty(ref _nombreBoton, value); }
        }
        private DelegateCommand<string> _miComando;
        public DelegateCommand<string> MiComando =>
            _miComando ?? (_miComando = new DelegateCommand<string>(ExecuteMiComando));

        void ExecuteMiComando(string parameter)
        {
            Console.WriteLine("DEBUG - ViewVM - MiComando parameter:{0}", parameter);
        }
        public VerViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            nombreBoton = "Pulsa coñññioooo";
            Console.WriteLine("CONSTR - VerViewModel()");
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            var listado = parameters["listado"] as string;
            var idPoblacion = parameters["idPoblacion"] as string;

            Console.WriteLine("DEBUG - VerVM-OnNavigatedTo() listado:{0}  idPoblacion:{1}", listado, idPoblacion);
            //miLista = App.Database.GetAlojamientosByCity(Int32.Parse(idPoblacion));
            listaAlojamientos = new ObservableCollection<TablaALOJAMIENTOS>(App.Database.GetAlojamientosByCity(int.Parse(idPoblacion)));

            //YPA.Views.Ver.prueba = "Adios";

            //listView.
        }
    }
}
