using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using YPA.Models;
using YPA.Views.Formularios;

namespace YPA.ViewModels
{
    public class CaminosViewModel : BindableBase, INavigationAware
    {
        INavigationService _navigationService;

        private DelegateCommand<string> _onNoteAddedClicked;
        public DelegateCommand<string> OnNoteAddedClicked =>
            _onNoteAddedClicked ?? (_onNoteAddedClicked = new DelegateCommand<string>(ExecuteOnNoteAddedClicked));

        void ExecuteOnNoteAddedClicked(string arg)
        {
            Console.WriteLine("CaminosVM - ExecuteOnNoteAddedClicked entrar...");
            ContentPage page = new EntryCAMINOS
            {
                BindingContext = new TablaCAMINOS()
            };
            Console.WriteLine("CaminosVM - ExecuteOnNoteAddedClicked - {0}", _navigationService.GetNavigationUriPath());
            Console.WriteLine("CaminosVM - ExecuteOnNoteAddedClicked - page: {0}", page.ToString());
            _navigationService.NavigateAsync(page.ToString());

           
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            Console.WriteLine("CaminosVM - OnNavigatedFrom");
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            Console.WriteLine("CaminosVM - OnNavigatedTo");
        }

        public CaminosViewModel(INavigationService navigationService)
        {
            Console.WriteLine("CaminosVM - Constructor");
            _navigationService = navigationService;
        }
    }
}
