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
        public Global()
        {

        }

        public static int FuncionGlobal(int numero, out int resultado)
        {
            resultado = numero * 4;
            return resultado * 2;
        }

/*
        public static string DameListaEtapas(ref List<TablaBaseCaminos> miLista)
        {
            string listado = "";
            foreach (var item in miLista)
            {
                if (item.esEtapa && item.esVisible)
                {
                    listado += item.nombrePoblacion;
                    listado += Global.separador[0]; // ";";
                }
            }
            return listado;
        }
        public static void SetEtapasInLista(ref List<TablaBaseCaminos> miLista, string listadoEtapas)  // listado es una secuencia de poblaciones separadas por ";". Obligatorio que acabe en ";" !!!
        {
            //string[] etapas = listado.Split(VerCaminoViewModel.separador);
            foreach (var item in miLista)
            {
                if (listadoEtapas.Contains(item.nombrePoblacion + ";"))
                    item.esEtapa = true;
                else
                    item.esEtapa = false;
            }
        }

        public static string DameCadenaBifurcaciones(ref Dictionary<string, string> bifurcaciones)
        {
            string listadoBifurcaciones = "";
            foreach (KeyValuePair<string, string> kvp in bifurcaciones)
            {
                //Console.WriteLine("DEBUG3 - VerCaminoVM - DameCadenaBifurcaciones: bifurcaciones: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                listadoBifurcaciones += kvp.Key;
                listadoBifurcaciones += Global.separadorDePares[0]; // "#"
                listadoBifurcaciones += kvp.Value;
                listadoBifurcaciones += Global.separador[0]; // ";"
            }
            return listadoBifurcaciones;
        }
        public static void SetBifurcaciones(ref Dictionary<string, string> bifurcaciones, string listadoBifurcaciones)
        {
            if (bifurcaciones == null)
                bifurcaciones = new Dictionary<string, string>();

            if (listadoBifurcaciones != null)
            {
                Console.WriteLine("DEBUG3 - VerCaminoVM - SetBifurcaciones listadoBifurcaciones:<{0}>", listadoBifurcaciones);
                bifurcaciones.Clear();
                string[] arrayBifurcaciones = listadoBifurcaciones.Split(Global.separador, StringSplitOptions.RemoveEmptyEntries);
                foreach (var bifurcacion in arrayBifurcaciones)
                {
                    string[] par = bifurcacion.Split(Global.separadorDePares);
                    bifurcaciones.Add(par[0], par[1]);
                }
            }
        }

        public static bool RellenarLista(ref MiCamino miCamino)
        {
            return RellenarLista(ref miCamino, null);
        }

        public static bool RellenarLista(ref MiCamino miCamino, string listadoEtapas = null)
        {
            Console.WriteLine("DEBUG2 - Global - RellenarLista");

            if (miCamino.caminoActual == null)
            {
                Console.WriteLine("ERROR - Global - RellenarLista()  caminoActual es null !!!");
                return false;
            }

            if (miCamino.caminoAnterior == null || miCamino.caminoAnterior != miCamino.caminoActual)
            {
                miCamino.miLista = App.Database.GetPoblacionesCamino(miCamino.caminoActual);
                miCamino.caminoAnterior = miCamino.caminoActual;
                // Ahora tengo que mirar en qué poblaciones hay albergue:
                List<string> poblacionesConAlbergue = null;
                poblacionesConAlbergue = App.Database.GetPoblacionesConAlbergue(miCamino.caminoActual);
                for (int i = 0; i < miCamino.miLista.Count; i++)
                {
                    //Console.WriteLine("DEBUG2 - VerCaminoVM - RellenarLista  esEtapa:{0}", miLista[i].esEtapa);
                    if (poblacionesConAlbergue.Contains(miCamino.miLista[i].nombrePoblacion))
                        miCamino.miLista[i].tieneAlbergue = true;

                    if (listadoEtapas != null)
                        if (listadoEtapas.Contains(miCamino.miLista[i].nombrePoblacion + ";"))
                            miCamino.miLista[i].esEtapa = true;
                        else
                            miCamino.miLista[i].esEtapa = false;
                }
            }

            // Esto lo tengo que sacar fuera de la función:
            //back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(miLista);

            return true;
        }



        public static bool RellenarLista(ref List<TablaBaseCaminos> miLista, ref string caminoActual, ref string caminoAnterior)
        {
            return RellenarLista(ref miLista, ref caminoActual, ref caminoAnterior, null);
        }

        public static bool RellenarLista(ref List<TablaBaseCaminos> miLista,
                                               ref string caminoActual,
                                               ref string caminoAnterior,
                                               string listadoEtapas = null)
        {
            Console.WriteLine("DEBUG2 - Global - RellenarLista");

            if (caminoActual == null)
            {
                Console.WriteLine("ERROR - Global - RellenarLista()  caminoActual es null !!!");
                return false;
            }

            if (caminoAnterior == null || caminoAnterior != caminoActual)
            {
                miLista =  App.Database.GetPoblacionesCamino(caminoActual);
                caminoAnterior = caminoActual;
                // Ahora tengo que mirar en qué poblaciones hay albergue:
                List<string> poblacionesConAlbergue = null;
                poblacionesConAlbergue = App.Database.GetPoblacionesConAlbergue(caminoActual);
                for (int i = 0; i < miLista.Count; i++)
                {
                    //Console.WriteLine("DEBUG2 - VerCaminoVM - RellenarLista  esEtapa:{0}", miLista[i].esEtapa);
                    if (poblacionesConAlbergue.Contains(miLista[i].nombrePoblacion))
                        miLista[i].tieneAlbergue = true;

                    if (listadoEtapas != null)
                        if (listadoEtapas.Contains(miLista[i].nombrePoblacion + ";"))
                            miLista[i].esEtapa = true;
                        else
                            miLista[i].esEtapa = false;
                }
            }
           
            // Esto lo tengo que sacar fuera de la función:
            //back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(miLista);

            return true;
        }

        public static ObservableCollection<TablaBaseCaminos> MasajearLista(ref List<TablaBaseCaminos> miLista, ref Dictionary<string, string> bifurcaciones,
                                         ref string caminoActual, ref string caminoAnterior, out string resumen)
        {
            return MasajearLista(ref miLista, ref bifurcaciones, ref caminoActual, ref caminoAnterior, out resumen, null, null, null);
        }
        public static ObservableCollection<TablaBaseCaminos> MasajearLista(ref List<TablaBaseCaminos> miLista, ref Dictionary<string, string> bifurcaciones,
                                         ref string caminoActual, ref string caminoAnterior, out string resumen,
                                         string cambiarBifurcacionEn = null, string listadoBifurcaciones = null, string listadoEtapas = null)
        {
            bool estamosEnBifurcacion = false;
            double acumulado = 0;
            double distanciaTotal;
            string siguienteNodo;

            bool esPrimeraEtapa = true;
            int numEtapas = 0;

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista(): cambiarBifurcacionEn:{0}",
                    cambiarBifurcacionEn == null ? "NULL" : cambiarBifurcacionEn);

            //Primero rellenamos de nuevo el respaldo de listaPuntosDePaso con todos los nodos:

            bool resp;
            ObservableCollection<TablaBaseCaminos> back_listaPuntosDePaso;
            
            resp = Global.RellenarLista(ref miLista, ref caminoActual, ref caminoAnterior, listadoEtapas);
            if (resp == false)
            {
                Console.WriteLine("ERROR - ###########----------#######    VerCaminoVM - MasajearLista(): RellenarLista devolvió false !!");
                resumen = "";
                return null;
            }
            back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(miLista);


            siguienteNodo = back_listaPuntosDePaso[0].nombrePoblacion; // Lo inicializamos con el nombre de la primera población.

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista() entrar  El primer nodo será siguienteNodo:{0}", siguienteNodo);

            // Lista que contendrá los nodos que habrá que eliminar de "listaPuntosDePaso" para no mostrarlos en la ListView:
            List<TablaBaseCaminos> borrar = new List<TablaBaseCaminos>();


            Global.SetBifurcaciones(ref bifurcaciones, listadoBifurcaciones);

            double acumuladoEtapa = 0;

            foreach (var item in back_listaPuntosDePaso)
            {
                var dataItem = (TablaBaseCaminos)item;

                Console.WriteLine("DEBUG - MasajearLista() estamosEnBifurcacion:{0}  siguienteNodo:{1}  nodo actual:{2}",
                            estamosEnBifurcacion, siguienteNodo, dataItem.nombrePoblacion);

                if (!estamosEnBifurcacion)
                {
                    dataItem.esVisible = true;
                    // Si no estamos en bifurcación, el nombre de la población debería coincidir con el que hay en siguienteNodo:
                    if (dataItem.nombrePoblacion != siguienteNodo)
                        Console.WriteLine("ERROR - VerCaminoVM - MasajearLista: Se esperaba {0} y estamos en {1}",
                            siguienteNodo, dataItem.nombrePoblacion);
                }
                else
                {
                    // Estamos en una bifurcación:                    
                    if (dataItem.nombrePoblacion == siguienteNodo)
                    {
                        Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Estamos en bifurcación y el nodo pertenece a la bifurcación");
                        dataItem.esVisible = true;
                    }
                    else
                    {
                        Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Estamos en bifurcación y el nodo NO pertenece a la bifurcación");
                        dataItem.esVisible = false;
                        borrar.Add(dataItem);
                    }
                }



                //Lo siguiente sólo es válido para un nodo que pertenezca al camino configurado:
                if (dataItem.esVisible == true)
                {
                    int indice = 0;
                    string[] distancias = dataItem.distanciaNodosSiguientes.Split(Global.separador);
                    string[] nodosSiguientes = dataItem.nodosSiguientes.Split(Global.separador);

                    Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista - #distancias:{0}  #nodosSiguientes:{1}",
                                distancias.Count(), nodosSiguientes.Count());

                    //Primero hay que comprobar si estamos en un fin de bifurcación. 
                    //Y más adelante se comprobará si estamos en un inicio de bifurcación (un nodo puede ser las dos cosas):
                    if (dataItem.FinBifurcacion)
                    {
                        estamosEnBifurcacion = false;
                        Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Aquí finaliza una bifurcación");
                    }

                    if (dataItem.IniBifurcacion)
                    {
                        Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: Estamos en INI bifurcacion en {0}", dataItem.nombrePoblacion);
                        estamosEnBifurcacion = true;

                        string bifConf = "";
                        if (bifurcaciones.TryGetValue(dataItem.nombrePoblacion, out bifConf))
                        {
                            int len = nodosSiguientes.Length;
                            indice = Array.IndexOf(nodosSiguientes, bifConf);
                            Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: bifConf: {0}  len:{1}  indice:{2}", bifConf, len, indice);
                            if (cambiarBifurcacionEn != null && cambiarBifurcacionEn == dataItem.nombrePoblacion)
                            {
                                // Este es el nodo o población donde tenemos que cambiar la bifurcación a tomar.
                                // Hay que buscar la siguiente bifurcación posible en este nodo,
                                // teniendo en cuenta que la bifurcación que había configurada está ahora en "bifConf":
                                indice = indice == len - 1 ? 0 : ++indice;
                                bifurcaciones[dataItem.nombrePoblacion] = nodosSiguientes[indice];
                                Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: indice:{0}  bif CAMBIADA en {1} hacia {2}",
                                    indice, dataItem.nombrePoblacion, bifurcaciones[dataItem.nombrePoblacion]);
                            }
                        }
                        else // No tenemos configurado ninguna bifurcación en el diccionario para este nodo.
                        {
                            // Si nos pasan algo en cambiarBifurcacionEn habrá que añadirla al diccionario:
                            if (cambiarBifurcacionEn != null && cambiarBifurcacionEn == dataItem.nombrePoblacion)
                            {
                                Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: ****SE AÑADIRA la bif de {0} hacia {1}",
                                    dataItem.nombrePoblacion, nodosSiguientes[1]);
                                bifurcaciones.Add(dataItem.nombrePoblacion, nodosSiguientes[1]);
                                indice = 1;
                            }

                        }

                    }


                    if (dataItem.esEtapa) // || dataItem.nodosSiguientes == "FIN_CAMINO")
                    {
                        if (esPrimeraEtapa)
                        {
                            dataItem.acumuladoEtapa = 0;
                            esPrimeraEtapa = false;
                            numEtapas = 0;
                        }
                        else
                        {
                            dataItem.acumuladoEtapa = acumulado - acumuladoEtapa;
                            numEtapas++;
                        }

                        acumuladoEtapa = acumulado;
                    }

                    dataItem.acumulado = acumulado;
                    acumulado += double.Parse(distancias[indice], culture);
                    siguienteNodo = nodosSiguientes[indice];

                    Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: siguienteNodo {0}   acumulado:{1}",
                                siguienteNodo, acumulado);
                }

            }

            foreach (KeyValuePair<string, string> kvp in bifurcaciones)
            {
                Console.WriteLine("DEBUG3 - VerCaminoVM - MasajearLista: bifurcaciones: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }


            //Ahora relleno el campo "distanciaAlFinal" que es el que dice los kms que restan hasta el final:
            distanciaTotal = acumulado;
            resumen = "[ " + numEtapas.ToString() + " etapas, " + distanciaTotal.ToString(CultureInfo.CreateSpecificCulture("es-ES")) + " km ]";

            foreach (var item in back_listaPuntosDePaso)
            {
                var dataItem = (TablaBaseCaminos)item;
                if (dataItem.esVisible == true)
                {
                    dataItem.distanciaAlFinal = distanciaTotal - dataItem.acumulado;

                }
            }


            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Número de nodos en back_listaPuntosDePaso:{0}", back_listaPuntosDePaso.Count());
            if (borrar.Count() > 0)
                foreach (var b in borrar)
                {
                    Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Borramos {0}", b.nombrePoblacion);
                    back_listaPuntosDePaso.Remove(b);
                }

            Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: Número de nodos final en back_listaPuntosDePaso:{0}", back_listaPuntosDePaso.Count());

            int i = 0;
            foreach (var b in back_listaPuntosDePaso)
            {
                Console.WriteLine("DEBUG - VerCaminoVM - MasajearLista: i: {0}  id: {1}  nombre: {2}  tieneAlbergue: {3}",
                    i++, b.id, b.nombrePoblacion, b.tieneAlbergue);
            }

            //_xx_ Lo siguiente lo tengo que hacer desde fuera de este método:
            // listaPuntosDePaso = back_listaPuntosDePaso;
            return back_listaPuntosDePaso;
        }
*/


    }
}
