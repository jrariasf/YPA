using Prism.Mvvm;
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
#pragma warning disable CS0108 // 'MiCamino.PropertyChanged' oculta el miembro heredado 'BindableBase.PropertyChanged'. Use la palabra clave new si su intención era ocultarlo.
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // 'MiCamino.PropertyChanged' oculta el miembro heredado 'BindableBase.PropertyChanged'. Use la palabra clave new si su intención era ocultarlo.
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
        //public string backupEtapas;


        private int _numEtapas;
        public int numEtapas
        {
            //get => _numEtapas;
            get
            {
                //return _numEtapas < 2 ? 0 : _numEtapas - 1;
                return _numEtapas;
            }
            set
            {
                Console.WriteLine("DEBUG3 - MiCamino - numEtapas <{0}>", value);
                SetProperty(ref _numEtapas, value);
                //resumen = "[ " + NumEtapas() + " etapas, " + distanciaTotalMiCamino.ToString(Global.culture) + " kms ]";
                resumen = NumEtapas() + " etapas, " + distanciaTotalMiCamino.ToString(Global.culture) + " kms";
                RaisePropertyChanged(nameof(numEtapas));
            }
        }

        private int _numDias;
        public int numDias
        {
            //get { return _numDias; }
            //set { _numDias = value; }
            get => _numDias;
            set
            {                
                SetProperty(ref _numDias, value);                
                RaisePropertyChanged(nameof(numDias));
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
                //resumen = "[ " + NumEtapas() + " etapas, " + distanciaTotalMiCamino.ToString(Global.culture) + " kms ]";
                resumen = NumEtapas() + " etapas, " + distanciaTotalMiCamino.ToString(Global.culture) + " kms";
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
            miNombreCamino = null;
            descripcion = null;
            caminoAnterior = null;
            bifurcaciones = null;
            str_bifurcaciones = null;
            etapas = null;
            //backupEtapas = null;
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

        public void MostrarInfo()
        {
            string mensaje;

            mensaje = "\n" + "miNombreCamino: " + miNombreCamino;
            mensaje += "\n" + "descripcion: " + descripcion;
            mensaje += "\n" + "str_bifurcaciones: " + (str_bifurcaciones == null ? "NULL" : str_bifurcaciones);
            mensaje += "\n" + "etapas: " + (etapas == null ? "NULL" : etapas);
            mensaje += "\n" + "distanciaTotal: " + distanciaTotal;
            mensaje += "\n" + "numEtapas: " + numEtapas;
            mensaje += "\n" + "numDias: " + numDias;
            mensaje += "\n";

            Console.WriteLine("DEBUG - MiCamino - MostrarInfo(): {0}", mensaje);
            /*
            Console.WriteLine("DEBUG - MiCamino - MostrarInfo():");
            Console.WriteLine("DEBUG - MiCamino - MostrarInfo():");
            Console.WriteLine("DEBUG - MiCamino - MostrarInfo():");
            Console.WriteLine("DEBUG - MiCamino - MostrarInfo():");
            Console.WriteLine("DEBUG - MiCamino - MostrarInfo():");
            */


        }
        public int NumEtapas()
        {
            // Retorna el número real de etaps:
            return numEtapas < 2 ? 0 : numEtapas - 1;
        }

        public string DameStringListaEtapas()
        {
            string listado = "" + Global.separador[0];
            numEtapas = 0;
            numDias = 0;

            foreach (var item in miLista)
            {
                if (item.esEtapa > 0 && item.esVisible)
                {
                    for (int i=0; i < item.esEtapa; i++)
                    {
                        listado += item.nombrePoblacion;
                        listado += Global.separador[0]; // ";";
                        numDias++;   //_xx_numEtapas   numEtapas++;
                    }
                    numEtapas++;     //_xx_numEtapas   numDias += item.esEtapa;
                }
            }
            //_xx_numEtapas:  Esto se añade nuevo:
            //_xx_numEtapas  numEtapas--;
            numDias--;

            Console.WriteLine("DEBUG2 - MiCamino - DameStringListaEtapas()  numEtapas <{0}>   numDias <{1}>", numEtapas, numDias);

            return listado;
        }

        public void SetEtapasInLista(string listadoEtapas)  // listado es una secuencia de poblaciones separadas por ";". Obligatorio que empiece y acabe en ";" !!!
        {
            //string[] etapas = listado.Split(VerCaminoViewModel.separador);
            foreach (var item in miLista)
            {
                int posicion = 0;
                item.esEtapa = 0;
                string buscar = Global.separador[0] + item.nombrePoblacion + Global.separador[0];
                while ((posicion = listadoEtapas.IndexOf(buscar, posicion)) > -1)
                {   //_xx_ETAPAS  Tenemos que contar cuantos días estamos en ese nodo o población:
                    item.esEtapa++;
                    posicion += (buscar.Length - 1);
                }
            }
            etapas = listadoEtapas; //_xx_ETAPAS
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


        public ObservableCollection<Etapa> DameListaEtapas() //out ObservableCollection<Etapa> listaEtapas)
        {            
            bool esPrimeraEtapa = true;
            string poblacion_INI = "";
            int orden = 0;

            var fecha = DateTime.Parse(fechaInicio);
            TimeSpan ts = new TimeSpan(1, 0, 0, 0);

            ObservableCollection<Etapa> listaEtapas = new ObservableCollection<Etapa>();            

            string dia;
            Etapa etapa;
            foreach (var item in miLista)
            {
                if (item.esEtapa > 0 && item.esVisible)
                {
                    if (esPrimeraEtapa)
                    {
                        esPrimeraEtapa = false;
                        poblacion_INI = item.nombrePoblacion; // Guarda la población inicio de etapa.
                        if (item.esEtapa == 1)
                            continue;

                        for (int i = 0; i < item.esEtapa - 1; i++)
                        {
                            dia = fecha.ToString("dd-MM-yy") + " (" + Global.diaDeLaSemana[(int)fecha.DayOfWeek].Substring(0, 3) + ")";
                            etapa = new Etapa(orden++, dia, poblacion_INI, poblacion_INI, 0);
                            listaEtapas.Add(etapa);
                            fecha = fecha + ts;
                        }
                        continue;
                    }


                    dia = fecha.ToString("dd-MM-yy") + " (" + Global.diaDeLaSemana[(int)fecha.DayOfWeek].Substring(0, 3) + ")";
                    etapa = new Etapa(orden++, dia, poblacion_INI, item.nombrePoblacion, item.acumuladoEtapa);
                    listaEtapas.Add(etapa);
                    fecha = fecha + ts;
                    poblacion_INI = item.nombrePoblacion; // Guarda la población inicio de etapa para la próxima.

                    for (int i = 0; i < item.esEtapa - 1; i++)
                    {
                        dia = fecha.ToString("dd-MM-yy") + " (" + Global.diaDeLaSemana[(int)fecha.DayOfWeek].Substring(0, 3) + ")";
                        etapa = new Etapa(orden++, dia, poblacion_INI, poblacion_INI, 0);
                        listaEtapas.Add(etapa);
                        fecha = fecha + ts;
                    }

                }
            }

            return listaEtapas;
        }
        


        public bool RellenarLista()
        {
            Console.WriteLine("DEBUG2 - MiCamino - RellenarLista   caminoActual:{0}  caminoAnterior:{1}",
                caminoActual == null ? "NULL" : caminoActual, caminoAnterior == null ? "NULL" : caminoAnterior);

            if (caminoActual == null)
            {
                Console.WriteLine("ERROR - MiCamino - RellenarLista()  caminoActual es null !!!");
                return false;
            }


            if (caminoAnterior == null || caminoAnterior != caminoActual)
            {
                Console.WriteLine("DEBUG - MiCamino - RellenarLista  caminoAnterior == null || caminoAnterior != caminoActual");
                miLista = App.Database.GetPoblacionesCamino(caminoActual);
                Console.WriteLine("DEBUG - MiCamino - RellenarLista   miLista.Count:{0}", miLista.Count);
                caminoAnterior = caminoActual;

                // Ahora tengo que mirar en qué poblaciones hay albergue:
                List<string> poblacionesConAlbergue = null;
                poblacionesConAlbergue = App.Database.GetPoblacionesConAlbergue(caminoActual);
                for (int i = 0; i < miLista.Count; i++)
                {
                    //Console.WriteLine("DEBUG2 - VerCaminoVM - RellenarLista  esEtapa:{0}", miLista[i].esEtapa);
                    // Esto podríamos hacerlo sólo entre los nodos que van del indiceInicial al indiceFinal:
                    if (poblacionesConAlbergue.Contains(miLista[i].nombrePoblacion))
                        miLista[i].tieneAlbergue = true;

                    int posicion = 0;
                    miLista[i].esEtapa = 0;
                    string buscar = Global.separador[0] + miLista[i].nombrePoblacion + Global.separador[0];
                    if (etapas != null)
                    {
                        while ((posicion = etapas.IndexOf(buscar, posicion)) > -1)
                        {   //_xx_ETAPAS  Tenemos que contar cuantos días estamos en ese nodo o población:
                            miLista[i].esEtapa++;
                            posicion += (buscar.Length - 1);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("DEBUG3 - MiCamino - RellenarLista  #### NO SE HACE NADA NUEVO !!!  Quizás se marquen los nodos que son etapas ####");
                //_xx_ETAPAS: Voy a marcar las etapas por si desde el MenuMisEtapas hemos modificado algo:
                /*  Modificado ligeramente abajo:
                for (int i = 0; i < miLista.Count; i++)
                {                    
                    int posicion = 0;
                    miLista[i].esEtapa = 0;
                    string buscar = Global.separador[0] + miLista[i].nombrePoblacion + Global.separador[0];
                    if (etapas != null)
                    {
                        while ((posicion = etapas.IndexOf(buscar, posicion)) > -1)
                        {   //_xx_ETAPAS  Tenemos que contar cuantos días estamos en ese nodo o población:
                            miLista[i].esEtapa++;
                            posicion += (buscar.Length - 1);
                        }
                    }
                }
                */
                if (etapas != null)
                {
                    for (int i = 0; i < miLista.Count; i++)
                    {
                        int posicion = 0;
                        miLista[i].esEtapa = 0;
                        string buscar = Global.separador[0] + miLista[i].nombrePoblacion + Global.separador[0];
                        while ((posicion = etapas.IndexOf(buscar, posicion)) > -1)
                        {   //_xx_ETAPAS  Tenemos que contar cuantos días estamos en ese nodo o población:
                            miLista[i].esEtapa++;
                            posicion += (buscar.Length - 1);
                        }
                    }
                }
            }

            return true;
        }

        public ObservableCollection<TablaBaseCaminos> MasajearLista(string cambiarBifurcacionEn = null, string primerNodoEtapa=null, string ultimoNodoEtapa=null)
            //ref List<TablaBaseCaminos> miLista, ref Dictionary<string, string> bifurcaciones,
            //ref string caminoActual, ref string caminoAnterior, out string resumen,
            //string cambiarBifurcacionEn = null, string listadoBifurcaciones = null, string listadoEtapas = null)
        {
            int estamosEnBifurcacion = 0;
            double acumulado = 0;
            double distanciaAlInicioPrimeraEtapa = 0;
            double distanciaAlInicioUltimaEtapa = 0;
            string siguienteNodo;
            int numEtapasLocal = 0;
            int numDiasLocal = 0;
            int indiceInicial=-1, indiceFinal=-1;

            bool esPrimeraEtapa = true;          

            Console.WriteLine("DEBUG - MiCamino - MasajearLista(): cambiarBifurcacionEn:{0}  primerNodoEtapa:{1}  ultimoNodoEtapa:{2}",
                    cambiarBifurcacionEn == null ? "NULL" : cambiarBifurcacionEn,
                    primerNodoEtapa == null ? "NULL" : primerNodoEtapa,
                    ultimoNodoEtapa == null ? "NULL" : ultimoNodoEtapa);

            Console.WriteLine("DEBUG4 - MiCamino - MasajearLista() INI: etapas <{0}>", etapas);
            Console.WriteLine("DEBUG4 - MiCamino - MasajearLista() INI: numEtapas <{0}>    numDias <{1}>", numEtapas, numDias);        
                 
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


            if (primerNodoEtapa != null && ultimoNodoEtapa != null)
            {
                Console.WriteLine("DEBUG - MiCamino - MasajearLista primerNodoEtapa:{0}   ultimoNodoEtapa:{1}", primerNodoEtapa, ultimoNodoEtapa);
                
                TablaBaseCaminos nodoInicial = new TablaBaseCaminos(primerNodoEtapa);
                TablaBaseCaminos nodoFinal = new TablaBaseCaminos(ultimoNodoEtapa);
                indiceInicial = miLista.IndexOf(nodoInicial);
                indiceFinal = miLista.IndexOf(nodoFinal);
                Console.WriteLine("DEBUG - MiCamino - RellenarLista   sindiceInicial:{0}   indiceFinal:{1}", indiceInicial, indiceFinal);

                miLista[indiceInicial].checkboxEnabled = false;
                miLista[indiceFinal].checkboxEnabled = false;               
            }            

            siguienteNodo = miLista[0].nombrePoblacion; // Lo inicializamos con el nombre de la primera población.

            Console.WriteLine("DEBUG - MiCamino - MasajearLista() entrar  El primer nodo será siguienteNodo:{0}", siguienteNodo);

            // Lista que contendrá los nodos que habrá que eliminar de "listaPuntosDePaso" para no mostrarlos en la ListView:
            List<TablaBaseCaminos> borrar = new List<TablaBaseCaminos>();

            double acumuladoEtapa = 0;

            //_xx_esVisible  foreach (var item in back_listaPuntosDePaso)
            foreach (var item in miLista)
            {
                var dataItem = (TablaBaseCaminos)item;

                Console.WriteLine("DEBUG - MiCamino - MasajearLista() estamosEnBifurcacion:{0}  siguienteNodo:{1}  nodo actual:{2}",
                            estamosEnBifurcacion, siguienteNodo, dataItem.nombrePoblacion);

                if (estamosEnBifurcacion == 0)
                {
                    /*
                    dataItem.esVisible = true;
                    // Si no estamos en bifurcación, el nombre de la población debería coincidir con el que hay en siguienteNodo:
                    if (dataItem.nombrePoblacion != siguienteNodo)
                        Console.WriteLine("ERROR - MiCamino - MasajearLista: Se esperaba {0} y estamos en {1}",
                            siguienteNodo, dataItem.nombrePoblacion);
                    */
                    if (dataItem.nombrePoblacion != siguienteNodo)
                    {
                        // He descubierto un caso en el que sucede esto y es cuando se visualizan los nodos de una etapa concreta
                        // (por ejemplo entre Laza y Ourense en el camino sanabrés) y resulta que se ha iniciado una bifurcación
                        // antes de empezar esa etapa y todavía no se ha cerrado la bifurcación:
                        Console.WriteLine("ERROR - MiCamino - MasajearLista: Al parecer NO estamos en bifurcación y el siguienteNodo no coincide con el nombrePoblacion");
                        dataItem.esVisible = false;
                        borrar.Add(dataItem);
                        // Quizás para poner las cosas en su sitio podría ser bueno poner estamosEnBifurcacion a true:
                        if (primerNodoEtapa != null && ultimoNodoEtapa == null)
                        {
                            Console.WriteLine("DEBUG - MiCamino - MasajearLista: Hemos FORZADO el poner estamosEnBifurcacion a 1");
                            estamosEnBifurcacion = 1;
                        }

                    }
                    else
                    {
                        dataItem.esVisible = true;
                    }

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
                        estamosEnBifurcacion--; //_xx_bif  Igual hay que controlar que como mínimo sea 0, que nunca sea negativo !!!
                        if (estamosEnBifurcacion < 0)
                        {
                            Console.WriteLine("ERROR - MiCamino - MasajearLista: Aquí finaliza una bifurcación en {0}   Ahora estamosEnBifurcacion se fuerza a {1}", dataItem.nombrePoblacion, estamosEnBifurcacion);
                            estamosEnBifurcacion = 0;
                        } else
                            Console.WriteLine("DEBUG3 - MiCamino - MasajearLista: Aquí finaliza una bifurcación en {0}   Ahora estamosEnBifurcacion será {1}", dataItem.nombrePoblacion, estamosEnBifurcacion);
                        
                    }

                    if (dataItem.IniBifurcacion)
                    {                        
                        estamosEnBifurcacion++;
                        Console.WriteLine("DEBUG3 - MiCamino - MasajearLista: Estamos en INI bifurcacion en {0}   Ahora estamosEnBifurcacion será {1}", dataItem.nombrePoblacion, estamosEnBifurcacion);

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


                    if (dataItem.esEtapa > 0) // || dataItem.nodosSiguientes == "FIN_CAMINO")
                    {
                        //_xx_ETAPAS:
                        //numEtapas++; // Lo comento y utilizo numEtapasLocal para que no se esté actualizando continuamente la información el "resumen" en la UI (User Interface)
                        numEtapasLocal++;
                        numDiasLocal += dataItem.esEtapa;

                        if (esPrimeraEtapa)
                        {
                            dataItem.acumuladoEtapa = 0;
                            esPrimeraEtapa = false;                            
                            distanciaAlInicioPrimeraEtapa = acumulado;
                        }
                        else
                        {
                            dataItem.acumuladoEtapa = acumulado - acumuladoEtapa;
                        }

                        acumuladoEtapa = acumulado;
                        distanciaAlInicioUltimaEtapa = acumulado;
                    }

                    dataItem.acumulado = acumulado;
                    //acumulado += double.Parse(distancias[indice], Global.culture);
                    double dist;
                    try
                    {
                        if (double.TryParse(distancias[indice], NumberStyles.Float, Global.culture, out dist) == true)
                            acumulado += dist;
                        else
                        {
                            Console.WriteLine("ERROR - MiCamino - MasajearLista: Distancia incorrecta en {0}", dataItem.nombrePoblacion);
                            NumberFormatInfo nfi = Global.culture.NumberFormat;
                            Console.WriteLine("ERROR - MiCamino - MasajearLista: Separador decimal utilizado: <{0}>", nfi.NumberDecimalSeparator);
                            // No haremos nada, es como si la distancia fuese 0 y no sumásemos nada a "acumulado".
                        }
                    } catch (IndexOutOfRangeException e)
                    {
                        Console.WriteLine("ERROR - MiCamino - MasajearLista: IndexOutOfRangeException  indice: <{0}>  No especificada distancia en <{1}>", indice, dataItem.nombrePoblacion);
                    }


                    try
                    {
                        siguienteNodo = nodosSiguientes[indice];
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        // Es posible que nos hayamos de especificar los posibles nodos siguientes en esa bifurcación (se supone que estarán separados por ";"):
                        Console.WriteLine("ERROR - MiCamino - MasajearLista: IndexOutOfRangeException  indice: <{0}>  No especificada nodoSiguiente en <{1}>", indice, dataItem.nombrePoblacion);
                        // Al menos intentamos coger el valor que haya:
                        siguienteNodo = nodosSiguientes[0];
                    }


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
            //_xx_numEtapas   numEtapas = numEtapasLocal > 0 ? numEtapasLocal - 1 : 0;  
            numEtapas = numEtapasLocal;

            distanciaTotalMiCamino = NumEtapas() == 0 ? 0 : distanciaAlInicioUltimaEtapa - distanciaAlInicioPrimeraEtapa;

            numDias = numDiasLocal > 0 ? numDiasLocal - 1 : 0;  //_xx_numEtapas    numDias = numDiasLocal;

            //_xx_esVisible  foreach (var item in back_listaPuntosDePaso)
            foreach (var item in miLista)
            {
                var dataItem = (TablaBaseCaminos)item;
                if (dataItem.esVisible == true)
                {
                    dataItem.distanciaAlFinal = distanciaTotal - dataItem.acumulado;

                }
            }


            //_xx_esVisible  Esto es ahora nuevo: (lo he movido aquí)
            if (primerNodoEtapa != null && ultimoNodoEtapa != null)
            {
                int numNodos = indiceFinal - indiceInicial + 1;
                TablaBaseCaminos[] array = new TablaBaseCaminos[numNodos];
                miLista.CopyTo(indiceInicial, array, 0, numNodos);
                List<TablaBaseCaminos> listaNodosDeEtapa = new List<TablaBaseCaminos>(array);
                back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(listaNodosDeEtapa);
            } else
                back_listaPuntosDePaso = new ObservableCollection<TablaBaseCaminos>(miLista);


            Console.WriteLine("DEBUG - MiCamino - MasajearLista: Número de nodos en back_listaPuntosDePaso:{0}", back_listaPuntosDePaso.Count);
            if (borrar.Count > 0)
                foreach (var b in borrar)
                {
                    Console.WriteLine("DEBUG - MiCamino - MasajearLista: Borramos {0}", b.nombrePoblacion);
                    back_listaPuntosDePaso.Remove(b);
                }

            Console.WriteLine("DEBUG - MiCamino - MasajearLista: Número de nodos final en back_listaPuntosDePaso:{0}", back_listaPuntosDePaso.Count);

            Console.WriteLine("DEBUG4 - MiCamino - MasajearLista() FIN: etapas <{0}>", etapas);
            Console.WriteLine("DEBUG4 - MiCamino - MasajearLista() FIN: numEtapas <{0}>    numDias <{1}>", numEtapas, numDias);

            return back_listaPuntosDePaso;
        }



        public void ExecuteCheckPulsado(int id)
        {
            Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado({0})", id);

            int indice;
            TablaBaseCaminos buscar = new TablaBaseCaminos(id);

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

            if (item.esEtapa == 0) // Se marca como etapa:
            {
                Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() esEtapa estaba a FALSE. indice:{0}", indice);
                item.esEtapa++;
                if (item.esVisible)
                {
                    numEtapas++;
                    numDias++;
                }
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
                        if (miLista[i].esEtapa > 0)
                        {
                            //Console.WriteLine("DEBUG - MiCamino - ExecuteCheckPulsado() Restamos {0} - {1}", item.acumulado, miLista[i].acumulado);
                            item.acumuladoEtapa = Math.Round(item.acumulado - (double)miLista[i].acumulado, 1);
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
                        if (miLista[i].esEtapa > 0) // || listaPuntosDePaso[i].nodosSiguientes == "FIN_CAMINO")
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
                item.esEtapa = 0; //_xx_ETAPAS: Se ponía a false. Ahora creo que basta con ponerla a 0 (Lo digo porque igual había que decrementar pero es que si no es etapa, pues no es etapa y punto).
                if (item.esVisible)
                {
                    numEtapas--;
                    numDias--;
                }
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
                        if (miLista[i].esEtapa > 0)
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
                   
                }
                item.acumuladoEtapa = 0;
            }

            //_xx_numEtapas   Añado esto nuevo:
            /*
            if (numDias == 1)
                numDias = 0;
            if (numEtapas == 1)
                numEtapas = 0;
            */


        }


    }
}
