using Final.Models;
using Final.Services;
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

        public ObservableCollection<Reservacion> Reservaciones { get; set; } = new();
        public ObservableCollection<Carro> Carros { get; set; } = new(); // Lista de autos disponibles
        public Reservacion NuevaReservacion { get; set; } = new(); // Reservación a crear
        public Carro CarroSeleccionado { get; set; } // Auto seleccionado
        public ICommand AddReservacionCommand { get; }


        public ReservacionViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            LoadReservaciones();
            LoadCarros(); // Cargar autos disponibles
            AddReservacionCommand = new Command(async () => await AddReservacionAsync());

        }

        private async void LoadReservaciones()
        {
            var reservaciones = await _databaseService.GetReservacionesAsync();
            Reservaciones.Clear();
            foreach (var reservacion in reservaciones)
                Reservaciones.Add(reservacion);
        }

        private async void LoadCarros()
        {
            var carros = await _databaseService.GetCarrosAsync();
            Carros.Clear();
            foreach (var carro in carros)
                Carros.Add(carro);
        }

        public async Task AddReservacionAsync()
        {
            if (CarroSeleccionado == null)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Selecciona un auto", "OK");
                return;
            }

            // Asignar el ID del auto seleccionado a la reservación
            NuevaReservacion.IdCarro = CarroSeleccionado.IdCarro;
            NuevaReservacion.FechaReserva = DateTime.Now;

            await _databaseService.SaveReservacionAsync(NuevaReservacion);
            Reservaciones.Add(NuevaReservacion);

            // Reiniciar formulario
            NuevaReservacion = new Reservacion();
            OnPropertyChanged(nameof(NuevaReservacion));

            await Application.Current.MainPage.DisplayAlert("Éxito", "Reservación creada", "OK");
        }
    }

}
