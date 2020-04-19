using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using YPA.ViewModels;

namespace YPA.Dialogs
{
    public class MenuMisEtapasViewModel : BindableBase, IDialogAware, INotifyPropertyChanged
    {

        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - MenuMisEtapasVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


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

        public int alturaMenu;

        private bool _botonUnirVisible;
        public bool botonUnirVisible    
        {
            get { return _botonUnirVisible; }
            set
            {
                if (_botonUnirVisible == value)
                    return;
                SetProperty(ref _botonUnirVisible, value);
                RaisePropertyChanged(nameof(botonUnirVisible));
            }
            //set { SetProperty(ref _botonUnirVisible, value); }
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
            /*
            string listaEtapasNEW;
            string buscar = Global.separador[0] + etapa.poblacion_inicio_etapa + Global.separador[0];
            int posicion;
            posicion = listaSTR_Etapas.IndexOf(buscar, 0);
            if (posicion == 0)
                listaEtapasNEW = buscar + listaSTR_Etapas;
            else if (posicion > 0)
            {
                listaEtapasNEW = listaSTR_Etapas.Substring(0, posicion + 1);
                listaEtapasNEW += buscar;
                listaEtapasNEW += listaSTR_Etapas.Substring(posicion);
            }
            else // Es -1
            {
                Console.WriteLine("ERROR - MenuMisEtapasVM - ExecuteInsertarDiaAlInicio  No se ha localizado <{0}> en <{1}>",
                       buscar, listaSTR_Etapas);
                listaEtapasNEW = listaSTR_Etapas;
            }

            //listaSTR_Etapas = listaSTR_Etapas + "Deseiro;Aldrei;";
            DialogParameters param = new DialogParameters();
            param.Add("listaEtapas", listaEtapasNEW);

            RequestClose(param);
            */

            // Buscamos el ";" localizado en la posición indicada por etapa.orden:
            string listaEtapasNEW;
            string buscar = Global.separador[0] + etapa.poblacion_inicio_etapa + Global.separador[0];
            int posicion = 0;
            int startIndex = 0;
            int contador = etapa.orden + 1; // Le sumamos 1 para tener en cuenta el ";" del principio del todo.
            int i;
            for (i = 0; i < contador; i++)
            {
                posicion = listaSTR_Etapas.IndexOf(Global.separador[0], startIndex);
                startIndex = posicion + 1;
            }

            Console.WriteLine("DEBUG - MenuMisEtapasVM - ExecuteUnirEtapas  Despues del FOR  i <{0}>  posicion <{1}>", i, posicion);

            if (posicion == 0) // Aquí podríamos asegurarnos que realmente a partir de "posicion" tenemos la cadena "buscar"  _xx_PENDIENTE !!
                //listaEtapasNEW = buscar + listaSTR_Etapas;
                //listaEtapasNEW = Global.separador[0] + etapa.poblacion_inicio_etapa + listaSTR_Etapas;
                listaEtapasNEW = buscar + listaSTR_Etapas.Substring(1); // Cojo el SubString a partir del elemento 1 para saltarme el ";" que ya lo lleva la variable "buscar".
            else if (posicion > 0)
            {
                listaEtapasNEW = listaSTR_Etapas.Substring(0, posicion); // Poniendo solamente "posicion" estoy ya quitando el ";"
                //listaEtapasNEW += etapa.poblacion_inicio_etapa; // Quizas habría que comprobar que despues va a ir un ";"  _xx_PENDIENTE
                listaEtapasNEW += buscar;
                listaEtapasNEW += listaSTR_Etapas.Substring(posicion + 1); // Le sumo 1 a posicion para saltarme el ";" que ya venía en la variable "buscar".
            }
            else // Es -1
            {
                Console.WriteLine("ERROR - MenuMisEtapasVM - ExecuteUnirEtapas  No se ha localizado <{0}> en <{1}>",
                       Global.separador[0], listaSTR_Etapas);
                listaEtapasNEW = listaSTR_Etapas;
            }

            DialogParameters param = new DialogParameters();
            param.Add("listaEtapas", listaEtapasNEW);

            RequestClose(param);

        }

        private DelegateCommand<string> _InsertarDiaAlFinal;
        public DelegateCommand<string> InsertarDiaAlFinal =>
            _InsertarDiaAlFinal ?? (_InsertarDiaAlFinal = new DelegateCommand<string>(ExecuteInsertarDiaAlFinal));

        void ExecuteInsertarDiaAlFinal(string parameter)
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - ExecuteInsertarDiaAlFinal  etapa <{0}>  listaEtapas <{1}>",
                              etapa == null ? "NULL" : etapa.poblacion_inicio_etapa,
                              listaSTR_Etapas == null ? "NULL" : listaSTR_Etapas);
            /*
            string listaEtapasNEW;
            string buscar = Global.separador[0] + etapa.poblacion_fin_etapa + Global.separador[0];
            int posicion;
            posicion = listaSTR_Etapas.IndexOf(buscar, 0);
            if (posicion == 0)
                listaEtapasNEW = buscar + listaSTR_Etapas;
            else if (posicion > 0)
            {
                listaEtapasNEW = listaSTR_Etapas.Substring(0, posicion + 1);
                listaEtapasNEW += buscar;
                listaEtapasNEW += listaSTR_Etapas.Substring(posicion);
            }
            else // Es -1
            {
                Console.WriteLine("ERROR - MenuMisEtapasVM - ExecuteInsertarDiaAlFinal  No se ha localizado <{0}> en <{1}>",
                       buscar, listaSTR_Etapas);
                listaEtapasNEW = listaSTR_Etapas;
            }

            //listaSTR_Etapas = listaSTR_Etapas + "Deseiro;Aldrei;";
            DialogParameters param = new DialogParameters();
            param.Add("listaEtapas", listaEtapasNEW);

            RequestClose(param);
            */

            // Buscamos el ";" localizado en la posición indicada por etapa.orden:
            string listaEtapasNEW;
            string buscar = Global.separador[0] + etapa.poblacion_fin_etapa + Global.separador[0];
            int posicion = 0;
            int startIndex = 0;
            int contador = etapa.orden + 2; // Para contar el ";" del principio y el hecho de que se trata de la población final de la etapa.
            int i;
            for (i = 0; i < contador; i++)
            {
                posicion = listaSTR_Etapas.IndexOf(Global.separador[0], startIndex);
                startIndex = posicion + 1;
            }

            Console.WriteLine("DEBUG - MenuMisEtapasVM - ExecuteUnirEtapas  Despues del FOR  i <{0}>  posicion <{1}>", i, posicion);

            if (posicion == 0) // Aquí podríamos asegurarnos que realmente a partir de "posicion" tenemos la cadena "buscar"  _xx_PENDIENTE !!
                listaEtapasNEW = buscar + listaSTR_Etapas;
            else if (posicion > 0)
            {
                listaEtapasNEW = listaSTR_Etapas.Substring(0, posicion); // No pillo el ";"
                listaEtapasNEW += buscar;
                listaEtapasNEW += listaSTR_Etapas.Substring(posicion + 1); // Me salto el ";".  Pero tendría que ver la posibilidad de que me estuviera saliendo de madre.
            }
            else // Es -1
            {
                Console.WriteLine("ERROR - MenuMisEtapasVM - ExecuteUnirEtapas  No se ha localizado <{0}> en <{1}>",
                       Global.separador[0], listaSTR_Etapas);
                listaEtapasNEW = listaSTR_Etapas;
            }

            DialogParameters param = new DialogParameters();
            param.Add("listaEtapas", listaEtapasNEW);

            RequestClose(param);


        }

        private DelegateCommand<string> _UnirEtapas;
        public DelegateCommand<string> UnirEtapas =>
            _UnirEtapas ?? (_UnirEtapas = new DelegateCommand<string>(ExecuteUnirEtapas));

        void ExecuteUnirEtapas(string parameter)
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - ExecuteUnirEtapas  etapa ({0}, <{1}>)  listaEtapas <{2}>",
                              etapa == null ? -1 : etapa.orden,
                              etapa == null ? "NULL" : etapa.poblacion_inicio_etapa,
                              listaSTR_Etapas == null ? "NULL" : listaSTR_Etapas);
            /*
            string listaEtapasNEW;
            string buscar = Global.separador[0] + etapa.poblacion_inicio_etapa + Global.separador[0];
            int posicion;
            posicion = listaSTR_Etapas.IndexOf(buscar, 0);
            if (posicion == 0)
                listaEtapasNEW = buscar + listaSTR_Etapas;
            else if (posicion > 0)
            {
                listaEtapasNEW = listaSTR_Etapas.Substring(0, posicion + 1);
                listaEtapasNEW += buscar;
                listaEtapasNEW += listaSTR_Etapas.Substring(posicion);
            }
            else // Es -1
            {
                Console.WriteLine("ERROR - MenuMisEtapasVM - ExecuteUnirEtapas  No se ha localizado <{0}> en <{1}>",
                       buscar, listaSTR_Etapas);
                listaEtapasNEW = listaSTR_Etapas;
            }

            //listaSTR_Etapas = listaSTR_Etapas + "Deseiro;Aldrei;";
            DialogParameters param = new DialogParameters();
            param.Add("listaEtapas", listaEtapasNEW);

            RequestClose(param);
            */

            // Buscamos el ";" localizado en la posición indicada por etapa.orden:
            string listaEtapasNEW;
            string buscar = Global.separador[0] + etapa.poblacion_fin_etapa + Global.separador[0];
            int posicion = 0;
            int startIndex = 0;
            int contador = etapa.orden + 2;
            int maxPosicion = listaSTR_Etapas.Length - 1;
            int i;
            for (i = 0; i < contador; i++)
            {
                posicion = listaSTR_Etapas.IndexOf(Global.separador[0], startIndex);
                startIndex = posicion + 1;
            }

            Console.WriteLine("DEBUG - MenuMisEtapasVM - ExecuteUnirEtapas  Despues del FOR  i <{0}>  posicion <{1}>", i, posicion);

            if (posicion == 0) // Aquí podríamos asegurarnos que realmente a partir de "posicion" tenemos la cadena "buscar"  _xx_PENDIENTE !! Aunque por aquí nunca va a entrar.
            {
                Console.WriteLine("ERROR - MenuMisEtapasVM - ExecuteUnirEtapas  Por aquí no debería pasar nunca o eso creo");
                listaEtapasNEW = listaSTR_Etapas; // Por darle un valor siempre.

                // Ahora buscamos la población de final de etapa para eliminarla:             
                //posicion = listaSTR_Etapas.IndexOf(buscar, posicion + 1);
                //listaEtapasNEW = listaSTR_Etapas.Substring(0, posicion + 1);           
                //listaEtapasNEW += listaSTR_Etapas.Substring(posicion + buscar.Length);
            }
            else if (posicion > 0 && posicion < maxPosicion)
            {
                listaEtapasNEW = listaSTR_Etapas.Substring(0, posicion + 1); // Pillo el ";"
                posicion = listaSTR_Etapas.IndexOf(Global.separador[0], posicion + 1); // Sumo 1 para adelantar el ";" y buscar a partir de ahí el siguiente ";".
                if (posicion != -1 && posicion < maxPosicion) // Si se encuentra el siguiente ";" (y no nos salimos de madre)
                   listaEtapasNEW += listaSTR_Etapas.Substring(posicion + 1); // Me salto el ";" y cojo lo que haya hasta el final.
            }
            else // Es -1
            {
                Console.WriteLine("ERROR - MenuMisEtapasVM - ExecuteUnirEtapas  No se ha localizado <{0}> en <{1}>",
                       Global.separador[0], listaSTR_Etapas);
                listaEtapasNEW = listaSTR_Etapas;
            }

            DialogParameters param = new DialogParameters();
            param.Add("listaEtapas", listaEtapasNEW);

            RequestClose(param);
           
        }


        public void OnDialogOpened(IDialogParameters parameters)
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - OnDialogOpened");
            etapa = parameters.GetValue<Etapa>("etapa");
            listaSTR_Etapas = parameters.GetValue<string>("listaEtapas");
            botonUnirVisible = true;
            bool res = parameters.GetValue<bool>("esUltimaEtapa");
            if (res == true)
            {
                botonUnirVisible = false;
                alturaMenu = 250;
            }

                Console.WriteLine("DEBUG - MenuMisEtapasVM - OnDialogOpened  etapa ({0}, <{1}>)  listaEtapas <{2}>",
                              etapa == null ? -1 : etapa.orden,
                              etapa == null ? "NULL" : etapa.poblacion_inicio_etapa,
                              listaSTR_Etapas == null ? "NULL" : listaSTR_Etapas);
        }

        public MenuMisEtapasViewModel()
        {
            Console.WriteLine("DEBUG - MenuMisEtapasVM - CONSTRUCTOR");
            etapa = null;
            listaSTR_Etapas = null;
            alturaMenu = 330;

        }
    }
}
