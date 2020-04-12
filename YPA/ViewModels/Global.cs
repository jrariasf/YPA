using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using YPA.Models;

namespace YPA.ViewModels
{
    public class Global : BindableBase
    {
        public static char[] separador = { ';' };
        public static char[] separadorDePares = { '#' };
        public static CultureInfo culture = new CultureInfo("en-US");
        public static String[] diaDeLaSemana = new String[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };

        public static string nombreFicheroDeMiCamino = null;

        public Global()
        {

        }

        public static int FuncionGlobal(int numero, out int resultado)
        {
            resultado = numero * 4;
            return resultado * 2;
        }

    }

    public class Etapa
    {
        public string dia { get; set; }
        public string poblacion_inicio_etapa { get; set; }
        public string poblacion_fin_etapa { get; set; }
        //public string distancia { get; set; }
        public double distancia { get; set; }

        public Etapa(string _dia, string _poblacion_INI, string _poblacion_FIN, double _distancia)
        {
            dia = _dia;
            poblacion_inicio_etapa = _poblacion_INI;
            poblacion_fin_etapa = _poblacion_FIN;
            distancia = _distancia;
        }
    }


}
