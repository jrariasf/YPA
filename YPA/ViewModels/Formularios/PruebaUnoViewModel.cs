using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YPA.ViewModels
{
    public class PruebaUnoViewModel : BindableBase
    {
        private string _MiTexto;
        public string MiTexto
        {
            get { return _MiTexto; }
            set { SetProperty(ref _MiTexto, value); }
        }
        public PruebaUnoViewModel()
        {
            MiTexto = "Esto es una cagada buenaaa";
        }
    }
}
