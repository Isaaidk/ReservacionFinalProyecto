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
    public class CreateUserViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public bool EsAdmin { get; set; }

        public ICommand CreateUserCommand { get; }

        public CreateUserViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Inicializar el comando
            CreateUserCommand = new Command(async () => await CreateUserAsync());
        }

        private async Task CreateUserAsync()
        {
            if (string.IsNullOrWhiteSpace(Nombre) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Contrasenia))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, completa todos los campos", "OK");
                return;
            }

            var nuevoUsuario = new Usuario
            {
                Nombre = Nombre,
                Email = Email,
                Contrasenia = Contrasenia,
                EsAdmin = EsAdmin
            };

            await _databaseService.SaveUsuarioAsync(nuevoUsuario);
            await Application.Current.MainPage.DisplayAlert("Éxito", "Usuario creado correctamente", "OK");

            // Volver a la pantalla de inicio de sesión
            Application.Current.MainPage = new NavigationPage(new Views.LoginPage
            {
                BindingContext = new LoginViewModel(_databaseService)
            });
        }
    }
}
