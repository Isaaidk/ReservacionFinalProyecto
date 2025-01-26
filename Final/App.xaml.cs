using Final.API;
using Final.Services;
using Final.ViewModel;

namespace Final
{
    public partial class App : Application
    {
        private readonly DatabaseService _databaseService;

        public App()
        {
            InitializeComponent();

            // Inicializar el servicio de base de datos
            string dbPath = Path.Combine(FileSystem.AppDataDirectory, "AppDatabase.db3");
            _databaseService = new DatabaseService(dbPath);

            // Iniciar la API local en un hilo separado
            Task.Run(async () => await new LocalApiService(_databaseService).StartApiAsync());

            // Establecer la página principal en LoginPage
            MainPage = new NavigationPage(new Views.LoginPage()
            {
                BindingContext = new LoginViewModel(_databaseService)
            });
        }
    }
}
