using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace YPA.ViewModels
{
    public class PoblacionesViewModel : BindableBase, INavigationAware, INotifyPropertyChanged
    {
        public PoblacionesViewModel()
        {
            Console.WriteLine("CONSTR - PoblacionesViewModel()");
        }

        private DelegateCommand<string> _PoblacionTocada;
        public DelegateCommand<string> PoblacionTocada =>
            _PoblacionTocada ?? (_PoblacionTocada = new DelegateCommand<string>(ExecutePoblacionTocada));

        void ExecutePoblacionTocada(string id)
        {
            Console.WriteLine("DEBUG - PoblacionesVM - ExecutePoblacionTocada({0})", id == null ? "id es NULL" : id);

        }
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
            Console.WriteLine("DEBUG - PoblacionesVM - OnNavigatedTo()");
        }
    }
}
