﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using YPA.Dialogs;
using YPA.Models;

namespace YPA.ViewModels
{
    
    public class MiCamino: BindableBase, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private new void RaisePropertyChanged(string propertyName = null)
        {
            //Console.WriteLine("DEBUG3 - PoblacionesVM - RaisePropertyChanged{0}", propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private List<TablaBaseCaminos> _miLista;
        public List<TablaBaseCaminos> miLista
        {
            get { return _miLista; }
            set
            {
                Console.WriteLine("DEBUG3 - MiCamino - Cambio en miLista !!!");
                if (_miLista == value)
                    return;
                SetProperty(ref _miLista, value);
                RaisePropertyChanged(nameof(miLista));
            }
        }


        private Dictionary<string, string> _bifurcaciones;
        public Dictionary<string, string> bifurcaciones
        {
            get
            {
                if (_bifurcaciones == null)
                    _bifurcaciones = new Dictionary<string, string>();
                return _bifurcaciones;
            }
            set { SetProperty(ref _bifurcaciones, value); }
        }

        public string str_bifurcaciones;

        public string caminoActual;
        public string caminoAnterior;

        private string _fechaInicio;
        public string fechaInicio
        {
            get { return _fechaInicio; }
            set
            {
                SetProperty(ref _fechaInicio, value);
                RaisePropertyChanged(nameof(fechaInicio));
            }
        }

        private string _miNombreCamino;
        public string miNombreCamino
        {
            get { return _miNombreCamino; }
            set
            {
                SetProperty(ref _miNombreCamino, value);
                RaisePropertyChanged(nameof(miNombreCamino));
            }
        }

        private string _descripcion;
        public string descripcion
        {
            get { return _descripcion; }
            set
            {
                SetProperty(ref _descripcion, value);
                RaisePropertyChanged(nameof(descripcion));
            }
        }

        private double _distanciaTotal;
        public double distanciaTotal
        {
            get => _distanciaTotal;
            set
            {
                //Console.WriteLine("DEBUG3 - MiCamino - distanciaTotal <{0}>", value);
                SetProperty(ref _distanciaTotal, value);                
                RaisePropertyChanged(nameof(distanciaTotal));
            }
        }


        public string etapas;


        private int _numEtapas;
        public int numEtapas
        {
            get => _numEtapas;
            set
            {
                Console.WriteLine("DEBUG3 - MiCamino - numEtapas <{0}>", value);
                SetProperty(ref _numEtapas, value);
                //_xx_ETAPAS  resumen = "[ " + numEtapas.ToString() + " etapas, " + distanciaTotal.ToString(CultureInfo.CreateSpecificCulture("es-ES")) + " km ]";
                //resumen = "[ " + NumEtapas() + " etapas, " + distanciaTotalMiCamino.ToString(CultureInfo.CreateSpecificCulture("es-ES")) + " kms ]";
                resumen = "[ " + NumEtapas() + " etapas, " + distanciaTotalMiCamino.ToString(Global.culture) + " kms ]";
                RaisePropertyChanged(nameof(numEtapas));
            }
        }

        private double _distanciaTotalMiCamino;
        public double distanciaTotalMiCamino
        { 
            get => _distanciaTotalMiCamino;
            set
            {
                Console.WriteLine("DEBUG3 - MiCamino - distanciaTotalMiCamino; <{0}>", value);
                SetProperty(ref _distanciaTotalMiCamino, value);

                //_xx_ETAPAS resumen = "[ " + numEtapas.ToString() + " etapas, " + distanciaTotalMiCamino.ToString(CultureInfo.CreateSpecificCulture("es-ES")) + " kms ]";
                resumen = "[ " + NumEtapas() + " etapas, " + distanciaTotalMiCamino.ToString(Global.culture) + " kms ]";
                RaisePropertyChanged(nameof(distanciaTotalMiCamino));
            }
        }

        
        private string _resumen;
        public string resumen
        {
            get { return _resumen; }
            set
            {
                Console.WriteLine("DEBUG3 - MiCamino - resumen <{0}>", value);
                SetProperty(ref _resumen, value);
                RaisePropertyChanged(nameof(resumen));
            }
        }


        public MiCamino()
        {
            caminoActual = null;
            Init();
        }
        public MiCamino(string camino)
        {
            caminoActual = camino;
            Init();
        }

        public void Init()
        { 
            caminoAnterior = null;
            bifurcaciones = null;
            str_bifurcaciones = null;
            etapas = null;
            resumen = "KAKA";
        }

        public void Init(TablaMisCaminos tmc)
        {
            caminoActual = tmc.caminoBase;
            fechaInicio = tmc.dia.ToString("yyyy-MM-dd");
            miNombreCamino = tmc.miNombreCamino;
            descripcion = tmc.descripcion;
            SetBifurcaciones(tmc.bifurcaciones);
            if (tmc.etapas == null || tmc.etapas == "")
                etapas = null;
            else
                etapas = tmc.etapas;
        }

        public int NumEtapas()
        {
            // Retorna el número real de etaps:
            return numEtapas < 2 ? 0 : numEtapas - 1;
        }

        public string DameListaEtapas()
        {
            string listado = "";
            numEtapas = 0;
            foreach (var item in miLista)
            {
                if (item.esEtapa && item.esVisible)
                {
                    listado += item.nombrePoblacion;
                    listado += Global.separador[0]; // ";";
                    numEtapas++;
                }
            }
            //_xx_ETAPAS if (numEtapas > 0)
            //_xx_ETAPAS     numEtapas--;
            return listado;
        }
        public void SetEtapasInLista(string listadoEtapas)  // listado es una secuencia de poblaciones separadas por ";". Obligatorio que acabe en ";" !!!
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

        public string DameCadenaBifurcaciones()
        {
            string listadoBifurcaciones = "";
            foreach (KeyValuePair<string, string> kvp in bifurcaciones)
            {
                //Console.WriteLine("DEBUG3 - MiCamino - DameCadenaBifurcaciones: bifurcaciones: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                listadoBifurcaciones += kvp.Key;
                listadoBifurcaciones += Global.separadorDePares[0]; // "#"
                listadoBifurcaciones += kvp.Value;
                listadoBifurcaciones += Global.separador[0]; // ";"
            }
            return listadoBifurcaciones;
        }

        public void SetBifurcaciones(string listadoBifurcaciones)
        {
            str_bifurcaciones = listadoBifurcaciones;

            if (bifurcaciones == null)
                bifurcaciones = new Dictionary<string, string>();

            if (listadoBifurcaciones != null)
            {
                Console.WriteLine("DEBUG3 - MiCamino - SetBifurcaciones listadoBifurcaciones:<{0}>", listadoBifurcaciones);
                bifurcaciones.Clear();
                string[] arrayBifurcaciones = listadoBifurcaciones.Split(Global.separador, StringSplitOptions.RemoveEmptyEntries);
                foreach (var bifurcacion in arrayBifurcaciones)
                {
                    string[] par = bifurcacion.Split(Global.separadorDePares);
                    bifurcaciones.Add(par[0], par[1]);
                }
            }            
        }


        public void DameListaEtapas(out ObservableCollection<Etapa> listaEtapas)
        {
            //String[] diaDeLaSemana = new String[] { "Domingo", "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado" };
            bool esPrimeraEtapa = true;
            string poblacion_INI = "";

            var fecha = DateTime.Parse(fechaInicio);
            TimeSpan ts = new TimeSpan(1, 0, 0, 0);

            listaEtapas = new ObservableCollection<Etapa>();

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
                    string dia = fecha.ToString("dd-MM-yy") + " (" + Global.diaDeLaSemana[(int)fecha.DayOfWeek].Substring(0, 3) + ")";

                    Etapa etapa = new Etapa(dia, poblacion_INI, item.nombrePoblacion, String.Format("{0:0.0}", item.acumuladoEtapa) + " km");
                    listaEtapas.Add(etapa);

                    poblacion_INI = item.nombrePoblacion; // Guarda la población inicio de etapa para la próxima.
                    fecha = fecha + ts;
                }
            }

        }
        


        public bool RellenarLista()
        {
            Console.WriteLine("DEBUG2 - MiCamino - RellenarLista");

            if (caminoActual == null)
            {
                Console.WriteLine("ERROR - MiCamino - RellenarLista()  caminoActual es null !!!");
                return false;
            }

            if (caminoAnterior == null || caminoAnterior != caminoActual)
            {
                Console.WriteLine("DEBUG - MiCamino - RellenarLista  caminoAnterior == null || caminoAnterior != caminoActual");
                miLista = App.Database.GetPoblacionesCamino(caminoActual);
                caminoAnterior = caminoActual;
                // Ahora tengo que mirar en qué poblaciones hay albergue:
                List<string> poblacionesConAlbergue = null;
                poblacionesConAlbergue = App.Database.GetPoblacionesConAlbergue(caminoActual);
                for (int i = 0; i < miLista.Count; i++)
                {
                    //Console.WriteLine("DEBUG2 - VerCaminoVM - RellenarLista  esEtapa:{0}", miLista[i].esEtapa);
                    if (poblacionesConAlbergue.Contains(miLista[i].nombrePoblacion))
                        miLista[i].tieneAlbergue = true;

                    if (etapas != null)
                        if (etapas.Contains(miLista[i].nombrePoblacion + ";"))
                            miLista[i].esEtapa = true;
                        else
                            miLista[i].esEtapa = false;
                }
            }

            // Esto lo tengo que sacar fuera de la función:
            //back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(miLista);

            return true;
        }

        public ObservableCollection<TablaBaseCaminos> MasajearLista(string cambiarBifurcacionEn = null)
            //ref List<TablaBaseCaminos> miLista, ref Dictionary<string, string> bifurcaciones,
            //ref string caminoActual, ref string caminoAnterior, out string resumen,
            //string cambiarBifurcacionEn = null, string listadoBifurcaciones = null, string listadoEtapas = null)
        {
            bool estamosEnBifurcacion = false;
            double acumulado = 0;
            double distanciaAlInicioPrimeraEtapa = 0;
            double distanciaAlInicioUltimaEtapa = 0;
            string siguienteNodo;
            int numEtapasLocal = 0;

            bool esPrimeraEtapa = true;          

            Console.WriteLine("DEBUG - MiCamino - MasajearLista(): cambiarBifurcacionEn:{0}",
                    cambiarBifurcacionEn == null ? "NULL" : cambiarBifurcacionEn);

            //Primero rellenamos de nuevo el respaldo de listaPuntosDePaso con todos los nodos:

            bool resp;
            ObservableCollection<TablaBaseCaminos> back_listaPuntosDePaso;

            //_xx_ resp = Global.RellenarLista(listadoEtapas);
            resp = RellenarLista();
            if (resp == false)
            {
                Console.WriteLine("ERROR - ###########----------#######    MiCamino - MasajearLista(): RellenarLista devolvió false !!");
                //_xx_PENDIENTE  resumen = "";
                return null;
            }
            back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(miLista);


            siguienteNodo = back_listaPuntosDePaso[0].nombrePoblacion; // Lo inicializamos con el nombre de la primera población.

            Console.WriteLine("DEBUG - MiCamino - MasajearLista() entrar  El primer nodo será siguienteNodo:{0}", siguienteNodo);

            // Lista que contendrá los nodos que habrá que eliminar de "listaPuntosDePaso" para no mostrarlos en la ListView:
            List<TablaBaseCaminos> borrar = new List<TablaBaseCaminos>();


            //Global.SetBifurcaciones(ref bifurcaciones, listadoBifurcaciones);

            double acumuladoEtapa = 0;

            foreach (var item in back_listaPuntosDePaso)
            {
                var dataItem = (TablaBaseCaminos)item;

                Console.WriteLine("DEBUG - MiCamino - MasajearLista() estamosEnBifurcacion:{0}  siguienteNodo:{1}  nodo actual:{2}",
                            estamosEnBifurcacion, siguienteNodo, dataItem.nombrePoblacion);

                if (!estamosEnBifurcacion)
                {
                    dataItem.esVisible = true;
                    // Si no estamos en bifurcación, el nombre de la población debería coincidir con el que hay en siguienteNodo:
                    if (dataItem.nombrePoblacion != siguienteNodo)
                        Console.WriteLine("ERROR - MiCamino - MasajearLista: Se esperaba {0} y estamos en {1}",
                            siguienteNodo, dataItem.nombrePoblacion);
                }
                else
                {
                    // Estamos en una bifurcación:                    
                    if (dataItem.nombrePoblacion == siguienteNodo)
                    {
                        Console.WriteLine("DEBUG - MiCamino - MasajearLista: Estamos en bifurcación y el nodo pertenece a la bifurcación");
                        dataItem.esVisible = true;
                    }
                    else
                    {
                        Console.WriteLine("DEBUG - MiCamino - MasajearLista: Estamos en bifurcación y el nodo NO pertenece a la bifurcación");
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

                    Console.WriteLine("DEBUG - MiCamino - MasajearLista - #distancias:{0}  #nodosSiguientes:{1}",
                                distancias.Length, nodosSiguientes.Length);

                    //Primero hay que comprobar si estamos en un fin de bifurcación. 
                    //Y más adelante se comprobará si estamos en un inicio de bifurcación (un nodo puede ser las dos cosas):
                    if (dataItem.FinBifurcacion)
                    {
                        estamosEnBifurcacion = false;
                        Console.WriteLine("DEBUG - MiCamino - MasajearLista: Aquí finaliza una bifurcación");
                    }

                    if (dataItem.IniBifurcacion)
                    {
                        Console.WriteLine("DEBUG3 - MiCamino - MasajearLista: Estamos en INI bifurcacion en {0}", dataItem.nombrePoblacion);
                        estamosEnBifurcacion = true;

                        string bifConf = "";
                        if (bifurcaciones.TryGetValue(dataItem.nombrePoblacion, out bifConf))
                        {
                            int len = nodosSiguientes.Length;
                            indice = Array.IndexOf(nodosSiguientes, bifConf);
                            Console.WriteLine("DEBUG3 - MiCamino - MasajearLista: bifConf: {0}  len:{1}  indice:{2}", bifConf, len, indice);
                            if (cambiarBifurcacionEn != null && cambiarBifurcacionEn == dataItem.nombrePoblacion)
                            {
                                // Este es el nodo o población donde tenemos que cambiar la bifurcación a tomar.
                                // Hay que buscar la siguiente bifurcación posible en este nodo,
                                // teniendo en cuenta que la bifurcación que había configurada está ahora en "bifConf":
                                indice = indice == len - 1 ? 0 : ++indice;
                                bifurcaciones[dataItem.nombrePoblacion] = nodosSiguientes[indice];
                                Console.WriteLine("DEBUG3 - MiCamino - MasajearLista: indice:{0}  bif CAMBIADA en {1} hacia {2}",
                                    indice, dataItem.nombrePoblacion, bifurcaciones[dataItem.nombrePoblacion]);
                            }
                        }
                        else // No tenemos configurado ninguna bifurcación en el diccionario para este nodo.
                        {
                            // Si nos pasan algo en cambiarBifurcacionEn habrá que añadirla al diccionario:
                            if (cambiarBifurcacionEn != null && cambiarBifurcacionEn == dataItem.nombrePoblacion)
                            {
                                Console.WriteLine("DEBUG3 - MiCamino - MasajearLista: ****SE AÑADIRA la bif de {0} hacia {1}",
                                    dataItem.nombrePoblacion, nodosSiguientes[1]);
                                bifurcaciones.Add(dataItem.nombrePoblacion, nodosSiguientes[1]);
                                indice = 1;
                            }

                        }

                    }


                    if (dataItem.esEtapa) // || dataItem.nodosSiguientes == "FIN_CAMINO")
                    {
                        //_xx_ETAPAS:
                        //numEtapas++; // Lo comento y utilizo numEtapasLocal para que no se esté actualizando continuamente la información el "resumen" en la UI (User Interface)
                        numEtapasLocal++;
                        if (esPrimeraEtapa)
                        {
                            dataItem.acumuladoEtapa = 0;
                            esPrimeraEtapa = false;
                            //_xx_ETAPAS numEtapas = 0;
                            distanciaAlInicioPrimeraEtapa = acumulado;
                        }
                        else
                        {
                            dataItem.acumuladoEtapa = acumulado - acumuladoEtapa;
                            //_xx_ETAPAS numEtapas++;
                        }

                        acumuladoEtapa = acumulado;
                        distanciaAlInicioUltimaEtapa = acumulado;
                    }

                    dataItem.acumulado = acumulado;
                    acumulado += double.Parse(distancias[indice], Global.culture);
                    siguienteNodo = nodosSiguientes[indice];

                    Console.WriteLine("DEBUG - MiCamino - MasajearLista: siguienteNodo {0}   acumulado:{1}",
                                siguienteNodo, acumulado);
                }

            }

            foreach (KeyValuePair<string, string> kvp in bifurcaciones)
            {
                Console.WriteLine("DEBUG3 - MiCamino - MasajearLista: bifurcaciones: Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            }


            //Ahora relleno el campo "distanciaAlFinal" que es el que dice los kms que restan hasta el final:
            distanciaTotal = acumulado;
            numEtapas = numEtapasLocal;
            //_xx_ETAPAS distanciaTotalMiCamino = numEtapas == 0 ? 0 : distanciaTotal - distanciaAlInicioPrimeraEtapa;
            //_xx_ distanciaTotalMiCamino = NumEtapas() == 0 ? 0 : distanciaTotal - distanciaAlInicioPrimeraEtapa;
            distanciaTotalMiCamino = NumEtapas() == 0 ? 0 : distanciaAlInicioUltimaEtapa - distanciaAlInicioPrimeraEtapa;

            foreach (var item in back_listaPuntosDePaso)
            {
                var dataItem = (TablaBaseCaminos)item;
                if (dataItem.esVisible == true)
                {
                    dataItem.distanciaAlFinal = distanciaTotal - dataItem.acumulado;

                }
            }


            Console.WriteLine("DEBUG - MiCamino - MasajearLista: Número de nodos en back_listaPuntosDePaso:{0}", back_listaPuntosDePaso.Count);
            if (borrar.Count > 0)
                foreach (var b in borrar)
                {
                    Console.WriteLine("DEBUG - MiCamino - MasajearLista: Borramos {0}", b.nombrePoblacion);
                    back_listaPuntosDePaso.Remove(b);
                }

            Console.WriteLine("DEBUG - MiCamino - MasajearLista: Número de nodos final en back_listaPuntosDePaso:{0}", back_listaPuntosDePaso.Count);

            int i = 0;
            foreach (var b in back_listaPuntosDePaso)
            {
                Console.WriteLine("DEBUG - MiCamino - MasajearLista: i: {0}  id: {1}  nombre: {2}  tieneAlbergue: {3}",
                    i++, b.id, b.nombrePoblacion, b.tieneAlbergue);
            }

            //_xx_ Lo siguiente lo tengo que hacer desde fuera de este método:
            // listaPuntosDePaso = back_listaPuntosDePaso;
            return back_listaPuntosDePaso;
        }



        public void ExecuteCheckPulsado(string id)
        {
            Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado({0})", id);

            int indice;
            int id2int = int.Parse(id, Global.culture);

            TablaBaseCaminos buscar = new TablaBaseCaminos(id2int);

            indice = miLista.IndexOf(buscar);

            
            Console.WriteLine("DEBUG3 - MiCamino - ExecuteCheckPulsado()  indice <{0}>", indice);

            if (indice < 0)
            {
                Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() retornamos antes de tiempo porque indice es {0}", indice);
                return;
            }

            int max = miLista.Count;

            TablaBaseCaminos item = miLista[indice];
            
            Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado()  nombre:{0}  esVisible:{1}  esEtapa:{2}",
                item.nombrePoblacion, item.esVisible, item.esEtapa);

            double incrementoKmsMiCamino = 0;
            bool etapaAnadidaEnLosExtremos = false;

            if (item.esEtapa == false) // Se marca como etapa:
            {
                Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() esEtapa estaba a FALSE. indice:{0}", indice);
                item.esEtapa = true;
                if (item.esVisible)
                    numEtapas++;
                // Ahora calculamos el valor del kilometraje de la etapa.
                if (indice == 0) // Hay que tratar el caso especial de que se haya marcado como etapa la primera de las poblaciones del camino:
                {
                    item.acumuladoEtapa = 0;
                    etapaAnadidaEnLosExtremos = true;
                }
                else
                {
                    for (int i = indice - 1; i >= 0; i--)
                    {
                        if (miLista[i].esEtapa == true)
                        {
                            item.acumuladoEtapa = item.acumulado - miLista[i].acumulado;
                            Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() Localizado inferior i:{0}  acumuladoEtapa:{1}  restamos:{2}",
                                        i, item.acumuladoEtapa, miLista[i].acumulado);
                            incrementoKmsMiCamino = item.acumuladoEtapa;
                            break;
                        }
                        if (i == 0) // Quiere decir que es la primera etapa marcada:
                        {
                            Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() Se trata de la PRIMERA ETAPA !! [{0}]", item.nombrePoblacion);
                            item.acumuladoEtapa = 0;
                            etapaAnadidaEnLosExtremos = true;
                            //item.acumuladoEtapa = item.acumulado;
                        }

                    }
                }
                
                if (indice == max - 1) // Hay que tratar el caso especial en el que se ha marcado como etapa la última población de ese camino:
                {
                    distanciaTotalMiCamino += incrementoKmsMiCamino;

                }
                else
                {
                    for (int i = indice + 1; i < max; i++)
                    {
                        if (miLista[i].esEtapa == true) // || listaPuntosDePaso[i].nodosSiguientes == "FIN_CAMINO")
                        {
                            //listaPuntosDePaso[i].acumuladoEtapa = listaPuntosDePaso[i].acumuladoEtapa - item.acumuladoEtapa;
                            miLista[i].acumuladoEtapa = miLista[i].acumulado - item.acumulado;
                            Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() Localizado siguiente i:{0}-{1}  acumuladoEtapa:{2}  restamos:{3}  max:{4}",
                                        i, miLista[i].nombrePoblacion, miLista[i].acumuladoEtapa, miLista[i].acumulado, max);
                            if (etapaAnadidaEnLosExtremos)
                                distanciaTotalMiCamino += miLista[i].acumuladoEtapa;
                            break;
                        }
                        if (i == max - 1)  // Quiere decir que era la última etapa marcada:
                        {
                            distanciaTotalMiCamino += incrementoKmsMiCamino;
                        }

                        /* De momento comentamos lo que viene porque dejamos de considerar por defecto la última población del camino como si fuera la última etapa:
                        if (i == max) // Se ha recorrido toda la lista sin encontrar más PuntosDePaso:
                            listaPuntosDePaso[max - 1].acumuladoEtapa = listaPuntosDePaso[max - 1].acumulado - item.acumulado;
                        */

                    }
                }
            }
            else // Se desmarca esa etapa:
            {
                Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() esEtapa estaba a TRUE. La ponemos a false");
                item.esEtapa = false;
                if (item.esVisible)
                    numEtapas--;
                // Ahora recalculamos el valor del kilometraje de la siguiente etapa:            

                // Primero miramos si la población actual era la primera etapa marcada:
                bool eraPrimeraEtapa = item.acumuladoEtapa == 0 ? true : false;
                
                if (indice == max - 1) // Hay que trata el caso especial en el que se ha desmarcado como etapa la última población de ese camino:
                {
                    if (eraPrimeraEtapa) // Si además era la única etapa:
                    {
                        distanciaTotalMiCamino = 0;
                        //_xx_ETAPAS numEtapas = 0;
                    }
                    else
                    {
                        distanciaTotalMiCamino -= item.acumuladoEtapa;
                    }
                }
                else
                {
                    for (int i = indice + 1; i < max; i++)
                    {
                        if (miLista[i].esEtapa == true)
                        {
                            if (eraPrimeraEtapa)
                            {
                                //distanciaTotal -= item.acumuladoEtapa;
                                distanciaTotalMiCamino -= miLista[i].acumuladoEtapa;
                                miLista[i].acumuladoEtapa = 0;
                            }
                            else
                            {
                                miLista[i].acumuladoEtapa = miLista[i].acumuladoEtapa + item.acumuladoEtapa;
                            }
                            //miLista[i].acumuladoEtapa = eraPrimeraEtapa ? 0 : miLista[i].acumuladoEtapa + item.acumuladoEtapa;
                            Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() Localizado siguiente i:{0}-{1}  acumuladoEtapa:{2}  restamos:{3}  max:{4}",
                                        i, miLista[i].nombrePoblacion, miLista[i].acumuladoEtapa, miLista[i].acumulado, max);
                            break;
                        }
                        if (i == max - 1) // Se ha desmarcado la que era la última etapa:
                        {
                            distanciaTotalMiCamino -= item.acumuladoEtapa;
                        }

                        /* De momento comentamos lo que viene porque dejamos de considerar por defecto la última población del camino como si fuera la última etapa:
                        if (i == max)
                            listaPuntosDePaso[max - 1].acumuladoEtapa = listaPuntosDePaso[max - 1].acumulado - item.acumulado;
                        */
                    }
                    //item.acumuladoEtapa = 0;
                }
                item.acumuladoEtapa = 0;
            }            


        }


    }
}
