using Final.Models;
using Final.Services;
using Final.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace Final.ViewModel
{
    public class ReservacionViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;
        private readonly Usuario _usuarioActual;

        public ObservableCollection<Reservacion> Reservaciones { get; set; } = new();
        public ObservableCollection<Carro> Carros { get; set; } = new(); // Lista de autos disponibles
        public Reservacion NuevaReservacion { get; set; } = new(); // Reservación a crear
        public Carro CarroSeleccionado { get; set; } // Auto seleccionado
        public ICommand AddReservacionCommand { get; }
        public ICommand DeleteReservacionCommand { get; }
        public ICommand DeleteAllReservacionesCommand { get; }
        public ICommand DeleteSelectedReservacionCommand { get; }
        public Reservacion SelectedReservacion { get; set; }
        public ICommand LogoutCommand { get; }

        // Propiedad para saber si hay una reservación seleccionada
        public bool IsReservacionSelected => SelectedReservacion != null;

        public ReservacionViewModel(DatabaseService databaseService, Usuario usuarioActual)
        {
            _databaseService = databaseService;
            _usuarioActual = usuarioActual;

            // Cargar reservaciones asociadas al usuario actual
            LoadReservaciones();
            LoadCarros(); // Cargar autos disponibles

            AddReservacionCommand = new Command(async () => await AddReservacionAsync());
            DeleteReservacionCommand = new Command<Reservacion>(async (reservacion) => await DeleteReservacionAsync(reservacion));
            DeleteAllReservacionesCommand = new Command(async () => await DeleteAllReservacionesAsync());
            DeleteSelectedReservacionCommand = new Command(async () => await DeleteSelectedReservacionAsync());
            LogoutCommand = new Command(Logout);
        }

        //logout
        private void Logout()
        {
            // Mensaje de cierre de sesión
            Application.Current.MainPage.DisplayAlert("Sesión Cerrada", "Has cerrado sesión exitosamente.", "OK");

            // Navegar al login
            Application.Current.MainPage = new NavigationPage(new LoginPage
            {
                BindingContext = new LoginViewModel(_databaseService)
            });
        }


        // Cargar las reservaciones del usuario logueado
        private async void LoadReservaciones()
        {
            var reservaciones = await _databaseService.GetReservacionesByEmailAsync(_usuarioActual.Email);
            Reservaciones.Clear();
   
               

            foreach (var reservacion in reservaciones) {

                var carro = await _databaseService.GetCarroByIdAsync(reservacion.IdCarro);
                if (carro != null)
                {
                    reservacion.NombreCarro = carro.Marca; // Almacenar la marca en la reservación (esto es opcional si quieres tenerla)
                }
                    Reservaciones.Add(reservacion);
                }

        }

        // Cargar los carros disponibles
        private async void LoadCarros()
        {
            var carros = await _databaseService.GetCarrosAsync();
            Carros.Clear();
            foreach (var carro in carros)
                Carros.Add(carro);
        }

        // Crear una nueva reservación
        public async Task AddReservacionAsync()
        {
            if (CarroSeleccionado == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Selecciona un auto", "OK");
                return;
            }

            // Asignar el ID del carro seleccionado y el correo del usuario actual a la nueva reservación
            NuevaReservacion.IdCarro = CarroSeleccionado.IdCarro;
            NuevaReservacion.EmailUsuario = _usuarioActual.Email;
            NuevaReservacion.FechaReserva = DateTime.Now;

            await _databaseService.SaveReservacionAsync(NuevaReservacion);
            Reservaciones.Add(NuevaReservacion);

            // Reiniciar formulario
            NuevaReservacion = new Reservacion();
            OnPropertyChanged(nameof(NuevaReservacion));

            await Application.Current.MainPage.DisplayAlert("Éxito", "Reservación creada", "OK");
            LoadReservaciones();
        }

        // Eliminar una reservación
        public async Task DeleteReservacionAsync(Reservacion reservacion)
        {
            if (reservacion == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Selecciona una reservación para eliminar", "OK");
                return;
            }

            var result = await _databaseService.DeleteReservacionAsync(reservacion);

            if (result > 0)
            {
                // Eliminar la reservación de la lista local
                Reservaciones.Remove(reservacion);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Reservación eliminada", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar la reservación", "OK");
            }
        }

        // Eliminar la reservación seleccionada
        public async Task DeleteSelectedReservacionAsync()
        {
            if (SelectedReservacion == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Selecciona una reservación para eliminar", "OK");
                return;
            }

            var result = await _databaseService.DeleteReservacionAsync(SelectedReservacion);

            if (result > 0)
            {
                Reservaciones.Remove(SelectedReservacion);
                SelectedReservacion = null; // Limpiar la reservación seleccionada
                await Application.Current.MainPage.DisplayAlert("Éxito", "Reservación eliminada", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo eliminar la reservación", "OK");
            }
        }


        // Eliminar todas las reservaciones
        public async Task DeleteAllReservacionesAsync()
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Confirmación", "¿Estás seguro de que quieres eliminar todas las reservaciones?", "Sí", "No");

            if (confirm)
            {
                try
                {
                    var result = await _databaseService.DeleteAllReservacionesAsync();

                    if (result > 0)
                    {
                        Reservaciones.Clear();
                        await Application.Current.MainPage.DisplayAlert("Éxito", "Todas las reservaciones han sido eliminadas", "OK");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "No se pudieron eliminar las reservaciones", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
                }
            }
        }
    }
}
