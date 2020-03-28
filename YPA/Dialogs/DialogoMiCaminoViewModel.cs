using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using YPA.Models;

namespace YPA.Dialogs
{

    public class Etapa
    {
        public string dia { get; set; }
        public string poblacion_inicio_etapa { get; set; }
        public string poblacion_fin_etapa { get; set; }
        public string distancia { get; set; }

        public Etapa(string _dia, string _poblacion_INI, string _poblacion_FIN, string _distancia)
        {
            dia = _dia;
            poblacion_inicio_etapa = _poblacion_INI;
            poblacion_fin_etapa = _poblacion_FIN;
            distancia = _distancia;
        }
    }
    public class DialogoMiCaminoViewModel : BindableBase, IDialogAware, INotifyPropertyChanged
    {

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - PoblacionesVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Etapa> _listaEtapas;
        public ObservableCollection<Etapa> listaEtapas
        {
            get { return _listaEtapas; }
            set
            {
                if (_listaEtapas == value)
                    return;
                SetProperty(ref _listaEtapas, value);
                RaisePropertyChanged(nameof(listaEtapas));
            }
            //set { SetProperty(ref _listaEtapas, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { 
                SetProperty(ref _message, value);
                RaisePropertyChanged(nameof(Message));
            }
        }

        private string _etapas;
        public string etapas
        {
            get { return _etapas; }
            set { SetProperty(ref _etapas, value); }
        }

        private int _alturaLabel;
        public int alturaLabel
        {
            get { return _alturaLabel; }
            set { SetProperty(ref _alturaLabel, value); }
        }

        private DelegateCommand _closeCommand;
        public DelegateCommand CloseCommand =>
            _closeCommand ?? (_closeCommand = new DelegateCommand(ExecuteCloseCommand));

        void ExecuteCloseCommand()
        {
            RequestClose(null);
        }

        private DelegateCommand _printCommand;
        public DelegateCommand PrintCommand =>
            _printCommand ?? (_printCommand = new DelegateCommand(ExecutePrintCommand));

        void ExecutePrintCommand()
        {
            Console.WriteLine("DEBUG - DialogoMiCaminoVM - ExecutePrintCommand NumEtapas: {0}   Message: {1}",
                listaEtapas == null ? 0 : listaEtapas.Count, Message == null ? "null" : Message);

        }

        public DialogoMiCaminoViewModel()
        {
            Console.WriteLine("DEBUG - DialogoMiCaminoVM - DialogoMiCaminoViewModel - CONSTRUCTOR");
        }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
            //throw new NotImplementedException();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            //throw new NotImplementedException();
            Console.WriteLine("DEBUG - DialogoMiCaminoVM - OnDialogOpened");
            string resumen = parameters.GetValue<string>("resumen");

            Message = parameters.GetValue<string>("message") + "  " + resumen;

            List<TablaBaseCaminos> miLista = parameters.GetValue<List<TablaBaseCaminos>>("lista");
            String fechaInicio = parameters.GetValue<string>("fechaInicio");

            etapas = "";
            alturaLabel = 15;

            if (fechaInicio == null)
                fechaInicio = System.DateTime.Today.ToString("yyyy-MM-dd");

            var fecha = DateTime.Parse(fechaInicio);
            TimeSpan ts = new TimeSpan(1, 0, 0, 0);

            listaEtapas = new ObservableCollection<Etapa>();


            if (miLista == null)
            {
                Console.WriteLine("DEBUG - DialogoMiCaminoVM - OnDialogOpened   miLista es null");
                etapas = "No se ha recibido ninguna lista de etapas.";                
            }
            else
            {
                String[] diaDeLaSemana = new String[]{ "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
                bool esPrimeraEtapa = true;
                string poblacion_INI = "";
               
                foreach (var item in miLista)
                {
                    if (item.esEtapa && item.esVisible)
                    {
                        /*
                        alturaLabel += 15;
                        string nombrePoblacion = item.nombrePoblacion.Substring(0, item.nombrePoblacion.Length > 20 ? 20 : item.nombrePoblacion.Length);
                        etapas = etapas + String.Format("{0,-26}", fecha.ToString("yyyy-MM-dd") + " (" + diaDeLaSemana[(int)fecha.DayOfWeek] + "):") +
                                 String.Format("{0,-22}", nombrePoblacion + ":") + String.Format("{0,10:0.0}", item.acumuladoEtapa) + " km\n";
                        fecha = fecha + ts;
                        */

                        if (esPrimeraEtapa)
                        {
                            esPrimeraEtapa = false;
                            poblacion_INI = item.nombrePoblacion; // Guarda la población inicio de etapa.
                            continue;
                        }
                        string dia = fecha.ToString("dd-MM-yy") + " (" + diaDeLaSemana[(int)fecha.DayOfWeek].Substring(0,3) + ")";

                        Etapa etapa = new Etapa(dia, poblacion_INI, item.nombrePoblacion, String.Format("{0:0.0}", item.acumuladoEtapa) + " km");
                        listaEtapas.Add(etapa);

                        poblacion_INI = item.nombrePoblacion; // Guarda la población inicio de etapa para la próxima.
                        fecha = fecha + ts;
                    }
                }
            }

        }
    }
}
