using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
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
        private IDialogService _dialogService { get; }

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - VerEtapasVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        
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
        

        private MiCamino _miCamino;
        public MiCamino miCamino
        {
            get { return _miCamino; }
            set { SetProperty(ref _miCamino, value); }
        }


        public VerEtapasViewModel(INavigationService navigationService, IDialogService dialogService)
        {
            Console.WriteLine("DEBUG - VerEtapasVM - CONSTRUCTOR");
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        

        private DelegateCommand<Etapa> _ItemTappedCommand;
        public DelegateCommand<Etapa> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<Etapa>(ExecuteItemTappedCommand));

        async void ExecuteItemTappedCommand(Etapa etapa)
        {
            Console.WriteLine("DEBUG2 - VerEtapasVM - ExecuteItemTappedCommand  etapa:{0}", etapa);
            var navigationParams = new NavigationParameters();

            /*
            TablaMisCaminos tmc = await App.Database.GetMisCaminosAsync(int.Parse(id));

            if (tmc == null)
            {
                Console.WriteLine("DEBUG3 - VerEtapasVM - ExecuteAmpliarMiCamino NO HAY REGISTROS. retornamos");
                return;
            }
            */

            navigationParams.Add("option", 3);
            navigationParams.Add("miCamino", miCamino);
            navigationParams.Add("primerNodoEtapa", etapa.poblacion_inicio_etapa);
            navigationParams.Add("ultimoNodoEtapa", etapa.poblacion_fin_etapa);
            Console.WriteLine("DEBUG - VerEtapasVM - ExecuteItemTappedCommand  parámetros:{0}", navigationParams.ToString());
            _navigationService.NavigateAsync("VerCamino", navigationParams);


        }

        private DelegateCommand<Etapa> _OpcionesSobreEtapaCommand;
        public DelegateCommand<Etapa> OpcionesSobreEtapaCommand =>
            _OpcionesSobreEtapaCommand ?? (_OpcionesSobreEtapaCommand = new DelegateCommand<Etapa>(ExecuteOpcionesSobreEtapaCommand));

        void ExecuteOpcionesSobreEtapaCommand(Etapa parameter)
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - ExecuteOpcionesSobreEtapaCommand");

            _dialogService.ShowDialog("MenuMisEtapas");
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

            miCamino = new MiCamino();
            
            TablaMisCaminos tmc = parameters.GetValue<TablaMisCaminos>("tmc");

            if (tmc == null)
            {
                Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo  tmc == null   retornamos");
                return;
            }

            Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo: Venimos de MisCaminos con tmc");
            miCamino.Init(tmc);
            miCamino.MasajearLista();

            listaEtapas = miCamino.DameListaEtapas(); 
        }
    }
}
