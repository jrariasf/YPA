using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YPA.ViewModels
{
    public class VerCaminoViewModel : BindableBase
    {
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
        public VerCaminoViewModel()
        {

        }
    }
}
