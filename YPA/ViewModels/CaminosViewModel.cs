using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;
using YPA.Models;
using YPA.Views.Formularios;

namespace YPA.ViewModels
{
    public class CaminosViewModel : BindableBase, INavigationAware
    {
        INavigationService _navigationService;

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - CaminosVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<TablaCAMINOS> _listaCaminos;
        public ObservableCollection<TablaCAMINOS> listaCaminos
        {
            get { return _listaCaminos; }
            ///set { SetProperty(ref _listaPoblaciones, value); }
            set
            {
                if (_listaCaminos == value)
                    return;
                SetProperty(ref _listaCaminos, value);
                RaisePropertyChanged(nameof(listaCaminos));
            }
        }


        private DelegateCommand<string> _AddCaminoClicked;
        public DelegateCommand<string> AddCaminoClicked =>
            _AddCaminoClicked ?? (_AddCaminoClicked = new DelegateCommand<string>(ExecuteAddCaminoClicked));

        void ExecuteAddCaminoClicked(string parameter)
        {
            Console.WriteLine("DEBUG - CaminosVM - ExecuteAddCaminoClicked({0})", parameter);
            Console.WriteLine("DEBUG - CaminosVM - ExecuteAddCaminoClicked  UriPath: {0}", _navigationService.GetNavigationUriPath());
            _navigationService.NavigateAsync("EntryCAMINOS");
        }

        private DelegateCommand<TablaCAMINOS> _ItemTappedCommand;
        public DelegateCommand<TablaCAMINOS> ItemTappedCommand =>
            _ItemTappedCommand ?? (_ItemTappedCommand = new DelegateCommand<TablaCAMINOS>(ExecuteItemTappedCommand));

        void ExecuteItemTappedCommand(TablaCAMINOS camino)
        {
            Console.WriteLine("DEBUG - CaminosVM - ExecuteItemTappedCommand({0})  entrar...", camino);
            var navigationParams = new NavigationParameters();
            navigationParams.Add("camino", camino);
            _navigationService.NavigateAsync("EntryCAMINOS", navigationParams);
        }

        private DelegateCommand<string> _VerEtapasCamino;
        public DelegateCommand<string> VerEtapasCamino =>
            _VerEtapasCamino ?? (_VerEtapasCamino = new DelegateCommand<string>(ExecuteVerEtapasCamino));

        void ExecuteVerEtapasCamino(string camino)
        {
            Console.WriteLine("DEBUG - CaminosVM - ExecuteVerEtapasCamino({0})", camino);
            Console.WriteLine("DEBUG - CaminosVM - ExecuteVerEtapasCamino  UriPath: {0}", _navigationService.GetNavigationUriPath());
            _navigationService.NavigateAsync("VerCamino?camino=" + camino);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            Console.WriteLine("DEBUG - CaminosVM - OnNavigatedFrom");
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            Console.WriteLine("DEBUG - CaminosVM - OnNavigatedTo");
        }

        async void CargarCaminosAsync()
        {
            Console.WriteLine("DEBUG - CaminosVM - CargarCaminosAsync");
            //string query = "select * from TablaCAMINOS";
            List<TablaCAMINOS> miLista = await App.Database.GetCaminosAsync(); // QueryAsync<TablaCAMINOS>(query);
            listaCaminos = new ObservableCollection<TablaCAMINOS>(miLista);
        }
        public CaminosViewModel(INavigationService navigationService)
        {
            Console.WriteLine("DEBUG - CaminosVM - Constructor");
            _navigationService = navigationService;

            CargarCaminosAsync();
        }
    }
}
