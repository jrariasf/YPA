using Prism;
using Prism.Ioc;
using YPA.ViewModels;
using YPA.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using YPA.Data;
using System.IO;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace YPA
{
    public partial class App
    {
        static Database database;
        public string camino = null;

        public static Database Database
        {
            get
            {
                string pathSQLite;
                if (database == null)
                {
                    //database = new Database(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CaminoSantiago.db3"));
                    pathSQLite = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "CaminoSantiago.db3");
                    Console.WriteLine("Path SQLite: " + pathSQLite);
                    System.Console.WriteLine("DEBUG - App:Database  Se va a llamar a new Database()");
                    database = new Database(pathSQLite);
                    //Console.WriteLine("Path SQLite: " + Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)));
                }
                else
                    System.Console.WriteLine("DEBUG - App:Database  database no es null");
                return database;
            }
        }

        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            //await NavigationService.NavigateAsync("MainMasterDetail/NavigationPage/Poblaciones");
            await NavigationService.NavigateAsync("MainMasterDetail/NavigationPage/Ver?listado=albergues&idPoblacion=100");
        }

        public async void IrA(string page)
        {
            Console.WriteLine("DEBUG - App:IrA({0})", page);
            await NavigationService.NavigateAsync(page);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<MainMasterDetail, MainMasterDetailViewModel>();
            containerRegistry.RegisterForNavigation<Poblaciones, PoblacionesViewModel>();
            containerRegistry.RegisterForNavigation<Caminos, CaminosViewModel>();
            containerRegistry.RegisterForNavigation<VerCamino, VerCaminoViewModel>();
            containerRegistry.RegisterForNavigation<Alojamientos, AlojamientosViewModel>();
            containerRegistry.RegisterForNavigation<Ver, VerViewModel>();
            containerRegistry.RegisterForNavigation<Views.Formularios.EntryPOBLACIONES, ViewModels.Formularios.EntryPOBLACIONESViewModel>();
            containerRegistry.RegisterForNavigation<Views.Formularios.PruebaUno, PruebaUnoViewModel>();
        }
    }
}
