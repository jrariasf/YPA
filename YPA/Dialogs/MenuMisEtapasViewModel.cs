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

        private Etapa _etapa;
        public Etapa etapa
        {
            get { return _etapa; }
            set { SetProperty(ref _etapa, value); }
        }

        private string _listaSTR_Etapas;
        public string listaSTR_Etapas
        {
            get { return _listaSTR_Etapas; }
            set { SetProperty(ref _listaSTR_Etapas, value); }
        }

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

        private DelegateCommand<string> _InsertarDiaAlInicio;
        public DelegateCommand<string> InsertarDiaAlInicio =>
            _InsertarDiaAlInicio ?? (_InsertarDiaAlInicio = new DelegateCommand<string>(ExecuteInsertarDiaAlInicio));

        void ExecuteInsertarDiaAlInicio(string parameter)
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - ExecuteInsertarDiaAlInicio  etapa <{0}>  listaEtapas <{1}>",
                              etapa == null ? "NULL" : etapa.poblacion_inicio_etapa,
                              listaSTR_Etapas == null ? "NULL" : listaSTR_Etapas);

            listaSTR_Etapas = listaSTR_Etapas + "Deseiro;Aldrei;";
            DialogParameters param = new DialogParameters();
            param.Add("listaEtapas", listaSTR_Etapas);

            RequestClose(param);

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - OnDialogOpened");
            etapa = parameters.GetValue<Etapa>("etapa");
            listaSTR_Etapas = parameters.GetValue<string>("listaEtapas");

            Console.WriteLine("DEBUG - MenuMisEtapasVM - OnDialogOpened  etapa <{0}>  listaEtapas <{1}>",
                              etapa == null ? "NULL" : etapa.ToString(),
                              listaSTR_Etapas == null ? "NULL" : listaSTR_Etapas);
        }

        public MenuMisEtapasViewModel()
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - CONSTRUCTOR");
            etapa = null;
            listaSTR_Etapas = null;

        }
    }
}
