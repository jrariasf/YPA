using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using YPA.Models;

namespace YPA.ViewModels.Formularios
{
    public class EntryCAMINOSViewModel : BindableBase, INavigationAware
    {
        INavigationService _navigationService;

        private TablaCAMINOS _camino;
        public TablaCAMINOS camino
        {
            get { return _camino; }
            set { SetProperty(ref _camino, value); }
        }
       

        private DelegateCommand<string> _OnSaveButtonClicked;
        public DelegateCommand<string> OnSaveButtonClicked =>
            _OnSaveButtonClicked ?? (_OnSaveButtonClicked = new DelegateCommand<string>(ExecuteOnSaveButtonClicked));

        async void ExecuteOnSaveButtonClicked(string parameter)
        {
            Console.WriteLine("DEBUG - EntryCAMINOSVM - ExecuteOnSaveButtonClicked({0})", parameter);
            //var note = (TablaPOBLACIONES)BindingContext;
            //note.fecUltMod = DateTime.UtcNow;
            //await App.Database.SavePoblacionesAsync(note);
            //await Navigation.PopAsync();

            if (camino == null)
                Console.WriteLine("DEBUG - EntryCAMINOSVM - ExecuteOnSaveButtonClicked  camino es null");
            else
            {
                int resp = await App.Database.SaveCaminosAsync(camino);
                Console.WriteLine("DEBUG - EntryCAMINOSVM - ExecuteOnSaveButtonClicked  SaveCaminosAsync devuelve {0}", resp);
            }

            await _navigationService.GoBackAsync();

        }

        private DelegateCommand<string> _OnDeleteButtonClicked;
        public DelegateCommand<string> OnDeleteButtonClicked =>
            _OnDeleteButtonClicked ?? (_OnDeleteButtonClicked = new DelegateCommand<string>(ExecuteOnDeleteButtonClicked));

        async void ExecuteOnDeleteButtonClicked(string parameter)
        {
            Console.WriteLine("DEBUG - EntryCAMINOSVM - ExecuteOnDeleteButtonClicked({0})", parameter);
            //var note = (TablaPOBLACIONES)BindingContext;
            //await App.Database.DeletePoblacionesAsync(note);
            //await Navigation.PopAsync();

            if (camino == null)
                Console.WriteLine("DEBUG - EntryCAMINOSVM - ExecuteOnDeleteButtonClicked  poblacion es null");
            else
            {
                int resp = await App.Database.DeleteCaminosAsync(camino);
                Console.WriteLine("DEBUG - EntryCAMINOSVM - ExecuteOnDeleteButtonClicked  DeleteCaminosAsync devuelve {0}", resp);
            }

            await _navigationService.GoBackAsync();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
            Console.WriteLine("DEBUG - EntryCAMINOSVM - OnNavigatedFrom()");
        }

        public void OnNavigatedTo(INavigationParameters elcamino)
        {
            //throw new NotImplementedException();
            Console.WriteLine("DEBUG - EntryCAMINOSVM - OnNavigatedTo({0})", elcamino == null ? "lapoblacion es null" : elcamino.ToString());

            camino = elcamino.GetValue<TablaCAMINOS>("camino");

            if (camino == null)
            {
                Console.WriteLine("DEBUG - EntryCAMINOSVM - OnNavigatedTo:  camino es null. Hacemos un new TablaCAMINOS()");
                camino = new TablaCAMINOS();
            }
        }

        public EntryCAMINOSViewModel(INavigationService navigationService)
        {
            Console.WriteLine("DEBUG - EntryCAMINOSVM - CONSTRUCTOR");
            _navigationService = navigationService;

        }

    }
}
