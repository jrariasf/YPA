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
    public class VerEtapasViewModel : BindableBase, INavigationAware, INotifyPropertyChanged
    {
        INavigationService _navigationService;
        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - VerEtapasVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /*
        private ObservableCollection<Etapa> _listaEtapas;
        public ObservableCollection<Etapa> listaEtapas
        {
            get { return _listaEtapas; }
            set
            {
                if (_listaEtapas == value)
                    return;
                SetProperty(ref _listaEtapas, value);
                RaisePropertyChanged(nameof(listaEtapas));
            }            
        }
        */

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

        public VerEtapasViewModel()
        {
            Console.WriteLine("DEBUG - VerEtapasVM - CONSTRUCTOR");
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
            var navigationMode = parameters.GetNavigationMode();

            Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo");

            if (navigationMode == NavigationMode.Back)
            {
                Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo: Como estamos en BACK, retornamos sin masajear la lista");
                return;
            }

            //caminoActual = parameters.GetValue<string>("camino");

            TablaMisCaminos tmc = parameters.GetValue<TablaMisCaminos>("tmc");

            if (tmc != null)
            {
                Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo: Venimos de MisCaminos con tmc");                
                //miCamino.Init(tmc);
                //miCamino.MasajearLista();
            }
            else
            {
                Console.WriteLine("ERROR - VerEtapasVM - OnNavigatedTo: OPCIÓN NO CONTEMPLADA");
            }


        }
    }
}
