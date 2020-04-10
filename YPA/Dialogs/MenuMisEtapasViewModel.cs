using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using YPA.ViewModels;

namespace YPA.Dialogs
{
    public class MenuMisEtapasViewModel : BindableBase, IDialogAware //, INotifyPropertyChanged
    {
        

        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(ExecuteCloseCommand));

        void ExecuteCloseCommand()
        {
            RequestClose(null);
        }



        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - OnDialogOpened");
            //string resumen = parameters.GetValue<string>("resumen");

            //Message = parameters.GetValue<string>("message") + "  " + resumen;
        }

        public MenuMisEtapasViewModel()
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - CONSTRUCTOR");

        }
    }
}
