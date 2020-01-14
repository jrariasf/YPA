using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YPA.ViewModels
{
    public class MainMasterDetailViewModel : BindableBase, INavigationAware
    {
        INavigationService _navigationService;

        private DelegateCommand<string> _navigateCommand;
        public DelegateCommand<string> NavigateCommand =>
            _navigateCommand ?? (_navigateCommand = new DelegateCommand<string>(ExecuteCommandName));

        void ExecuteCommandName(string page)
        {
            Console.WriteLine("MainMasterDetailViewModel - ExecuteCommandName() Vamos a {0}", page);
            //_navigationService.NavigateAsync(new Uri(page));
            //_navigationService.NavigateAsync(page, useModalNavigation: false);            
            //_navigationService.NavigateAsync(new Uri(page, UriKind.Relative));
            _navigationService.NavigateAsync(page);
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            Console.WriteLine("DEBUG - MainMasterDetailViewModel - OnNavigatedFrom: {0}", _navigationService.GetNavigationUriPath());
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            Console.WriteLine("MainMasterDetailViewModel - OnNavigatedTo  parameters {0}", parameters.ToString());
        }

        public MainMasterDetailViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
