using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using YPA.Models;

namespace YPA.ViewModels.Formularios
{
    public class EntryPOBLACIONESViewModel : BindableBase, INavigationAware
    {
        INavigationService _navigationService;

        private TablaPOBLACIONES _poblacion;
        public TablaPOBLACIONES poblacion
        {
            get { return _poblacion; }
            set { SetProperty(ref _poblacion, value); }
        }

        private string _nombrePoblacion;
        public string nombrePoblacion
        {
            get { return _nombrePoblacion; }
            set { SetProperty(ref _nombrePoblacion, value); }
        }

        private DelegateCommand<string> _OnSaveButtonClicked;
        public DelegateCommand<string> OnSaveButtonClicked =>
            _OnSaveButtonClicked ?? (_OnSaveButtonClicked = new DelegateCommand<string>(ExecuteOnSaveButtonClicked));

        async void ExecuteOnSaveButtonClicked(string parameter)
        {
            Console.WriteLine("DEBUG - EntryPOBLACIONESVM - ExecuteOnSaveButtonClicked({0})", parameter);
            //var note = (TablaPOBLACIONES)BindingContext;
            //note.fecUltMod = DateTime.UtcNow;
            //await App.Database.SavePoblacionesAsync(note);
            //await Navigation.PopAsync();

            if (poblacion == null)
                Console.WriteLine("DEBUG - EntryPOBLACIONESVM - ExecuteOnSaveButtonClicked  poblacion es null");
            else
            {
                int resp = await App.Database.SavePoblacionesAsync(poblacion);
                Console.WriteLine("DEBUG - EntryPOBLACIONESVM - ExecuteOnSaveButtonClicked  SavePoblacionesAsync devuelve {0}", resp);
            }

            await _navigationService.GoBackAsync();

        }

        private DelegateCommand<string> _OnDeleteButtonClicked;
        public DelegateCommand<string> OnDeleteButtonClicked =>
            _OnDeleteButtonClicked ?? (_OnDeleteButtonClicked = new DelegateCommand<string>(ExecuteOnDeleteButtonClicked));

        async void ExecuteOnDeleteButtonClicked(string parameter)
        {
            Console.WriteLine("DEBUG - EntryPOBLACIONESVM - ExecuteOnDeleteButtonClicked({0})", parameter);
            //var note = (TablaPOBLACIONES)BindingContext;
            //await App.Database.DeletePoblacionesAsync(note);
            //await Navigation.PopAsync();

            if (poblacion == null)
                Console.WriteLine("DEBUG - EntryPOBLACIONESVM - ExecuteOnDeleteButtonClicked  poblacion es null");
            else
            {
                int resp = await App.Database.DeletePoblacionesAsync(poblacion);
                Console.WriteLine("DEBUG - EntryPOBLACIONESVM - ExecuteOnDeleteButtonClicked  DeletePoblacionesAsync devuelve {0}", resp);
            }

            await _navigationService.GoBackAsync();
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            Console.WriteLine("DEBUG - EntryPOBLACIONESVM - OnNavigatedFrom()");
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters lapoblacion)
        {
            Console.WriteLine("DEBUG - EntryPOBLACIONESVM - OnNavigatedTo({0})", lapoblacion == null ? "lapoblacion es null" : lapoblacion.ToString());

            poblacion = lapoblacion.GetValue<TablaPOBLACIONES>("poblacion");

            if (poblacion == null)
            {
                Console.WriteLine("DEBUG - EntryPOBLACIONESVM - OnNavigatedTo:  poblacion es null. Hacemos un new TablaALOJAMIENTOS()");
                poblacion = new TablaPOBLACIONES();
            }
            //throw new NotImplementedException();
        }

        public EntryPOBLACIONESViewModel(INavigationService navigationService)
        {
            Console.WriteLine("DEBUG - EntryPOBLACIONESVM - CONSTRUCTOR");
            _navigationService = navigationService;

            nombrePoblacion = "Viana de Cega";


        }
    }
}
