using Final.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Final.ViewModel
{
    public class EditUserViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public string Email { get; set; }
        public string NuevaContrasenia { get; set; }

        public ICommand UpdatePasswordCommand { get; }

        public EditUserViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Inicializar el comando
            UpdatePasswordCommand = new Command(async () => await UpdatePasswordAsync());
        }

        private async Task UpdatePasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(NuevaContrasenia))
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Por favor, completa todos los campos", "OK");
                return;
            }

            // Buscar al usuario por email
            var usuarioExistente = await _databaseService.GetUsuarioByEmailAsync(Email);
            if (usuarioExistente == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Usuario no encontrado", "OK");
                return;
            }

            // Actualizar la contraseña
            usuarioExistente.Contrasenia = NuevaContrasenia;
            await _databaseService.UpdateUsuarioAsync(usuarioExistente);

            await Application.Current.MainPage.DisplayAlert("Éxito", "Contraseña actualizada correctamente", "OK");

            // Opcional: Navegar a otra pantalla
            Application.Current.MainPage = new NavigationPage(new Views.LoginPage
            {
                BindingContext = new LoginViewModel(_databaseService)
            });
        }
    }
}
