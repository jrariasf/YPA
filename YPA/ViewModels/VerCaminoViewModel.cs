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
    public class VerCaminoViewModel : BindableBase, INavigationAware, INotifyPropertyChanged
    {
        INavigationService _navigationService;
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName = null)
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

        }


        public VerCaminoViewModel(INavigationService navigationService)
        {
            Console.WriteLine("DEBUG - CONSTR - VerCaminoViewModel()");
            _navigationService = navigationService;
            

        }
    }
}
