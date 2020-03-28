using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using YPA.Models;

namespace YPA.ViewModels
{
    public class VerEtapasViewModel : BindableBase, INavigationAware, INotifyPropertyChanged
    {
        INavigationService _navigationService;
        public new event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - VerEtapasVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public VerEtapasViewModel()
        {

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        
        public void OnNavigatedTo(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
            var navigationMode = parameters.GetNavigationMode();

            Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo");

            if (navigationMode == NavigationMode.Back)
            {
                Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo: Como estamos en BACK, retornamos sin masajear la lista");
                return;
            }

            //caminoActual = parameters.GetValue<string>("camino");

            TablaMisCaminos tmc = parameters.GetValue<TablaMisCaminos>("tmc");

            if (tmc != null)
            {
                Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo: Venimos de MisCaminos con tmc");
                /*
                caminoActual = tmc.caminoBase;
                fechaInicio = tmc.dia.ToString("yyyy-MM-dd");
                miNombreCamino = tmc.miNombreCamino;
                descripcion = tmc.descripcion;
                Console.WriteLine("DEBUG2 - VerEtapasVM - OnNavigatedTo: tmc.bifurcaciones:{0}", tmc.bifurcaciones);
                //SetBifurcaciones(tmc.bifurcaciones);
                MasajearLista(null, tmc.bifurcaciones, tmc.etapas);
                */

                //SetEtapasInLista(tmc.etapas);               
            }
            else
            {
                Console.WriteLine("ERROR - VerEtapasVM - OnNavigatedTo: OPCIÓN NO CONTEMPLADA");
            }


        }
    }
}
