using Final.Models;
using Final.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Final.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public Usuario UsuarioActual { get; private set; }

        public ICommand LoginCommand { get; }
        public ICommand NavigateToCreateUserCommand { get; }
        public ICommand ForgotPasswordCommand { get; }

        public LoginViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Inicializar comandos
            LoginCommand = new Command(async () => await LoginAndNavigateAsync());
            NavigateToCreateUserCommand = new Command(NavigateToCreateUser);
            ForgotPasswordCommand = new Command(NavigateToForgotPassword);
        }

        private void NavigateToForgotPassword()
        {
            // Navegar a la página de cambio de contraseña
            Application.Current.MainPage = new NavigationPage(new Views.ForgotPasswordPage
            {
                BindingContext = new EditUserViewModel(_databaseService)
            });
        }
        private void NavigateToCreateUser()
        {
            Application.Current.MainPage = new NavigationPage(new Views.CreateUserPage
            {
                BindingContext = new CreateUserViewModel(_databaseService)
            });
        }

        private async Task LoginAndNavigateAsync()
        {
            var usuario = await LoginAsync();
            if (usuario == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Credenciales incorrectas o usuario no encontrado", "OK");
                return;
            }

            // Navegar según el rol del usuario
            if (usuario.EsAdmin)
            {
                Application.Current.MainPage = new NavigationPage(new Views.CarroPage()
                {
                    BindingContext = new CarroViewModel(_databaseService)
                });
            }
            else
            {
                // Aquí pasamos el UsuarioActual al ViewModel de Reservacion
                Application.Current.MainPage = new NavigationPage(new Views.ReservacionPage()
                {
                    BindingContext = new ReservacionViewModel(_databaseService, usuario)
                });
            }
        }

        public async Task<Usuario> LoginAsync()
        {
            var usuario = await _databaseService.GetUsuarioByEmailAsync(Email);
            if (usuario == null || usuario.Contrasenia != Contrasenia)
                return null;

            UsuarioActual = usuario;
            return usuario;
        }

        public async Task CrearUsuarioAsync(Usuario nuevoUsuario)
        {
            await _databaseService.SaveUsuarioAsync(nuevoUsuario);
        }
    }
}
