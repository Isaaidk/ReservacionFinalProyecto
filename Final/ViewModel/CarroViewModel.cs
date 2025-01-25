using Final.Models;
using Final.Services;
using Final.ViewModel;
using Final.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class CarroViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<Carro> Carros { get; set; } = new();
    public Carro NuevoCarro { get; set; } = new();
    public Carro CarroSeleccionado { get; set; }

    public ICommand AddCarroCommand { get; }
    public ICommand DeleteCarroCommand { get; }
    public ICommand EditCarroCommand { get; }
    public ICommand NavigateToCrearAutoPageCommand { get; }
    public ICommand LogoutCommand { get; }
    public ICommand RefreshCarrosCommand { get; }



    public CarroViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadCarros();

        // Inicializar el comando
        AddCarroCommand = new Command(async () => await AddCarroAsync());
        DeleteCarroCommand = new Command(async () => await DeleteCarroAsync(), () => CarroSeleccionado != null);
        EditCarroCommand = new Command(async () => await EditCarroAsync(), () => CarroSeleccionado != null);
        NavigateToCrearAutoPageCommand = new Command(NavigateToCrearAutoPage);
        LogoutCommand = new Command(Logout);
        RefreshCarrosCommand = new Command(() => LoadCarros());
        LoadCarros();

    }

    private void NavigateToCrearAutoPage()
    {
        var crearAutoPage = new CrearAutoPage
        {
            BindingContext = this // Mantener el mismo ViewModel
        };

        Application.Current.MainPage.Navigation.PushAsync(crearAutoPage);
    }

    private void OnCarroCreado()
    {
        // Método que se llama después de agregar un carro para recargar los datos
        LoadCarros();
    }

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

    private async void LoadCarros()
    {
        var carros = await _databaseService.GetCarrosAsync();
        Carros.Clear();
        foreach (var carro in carros)
            Carros.Add(carro);
    }

    private async Task AddCarroAsync()
    {
        if (string.IsNullOrWhiteSpace(NuevoCarro.Marca) || string.IsNullOrWhiteSpace(NuevoCarro.Modelo))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Completa todos los campos", "OK");
            return;
        }

        await _databaseService.SaveCarroAsync(NuevoCarro);

        // Recargar la lista
        LoadCarros();

        // Reiniciar el formulario
        NuevoCarro = new Carro();
        OnPropertyChanged(nameof(NuevoCarro));

        // Mostrar mensaje de éxito
        await Application.Current.MainPage.DisplayAlert("Éxito", "Auto agregado correctamente", "OK");

        // Regresar a la página anterior
        await Application.Current.MainPage.Navigation.PopAsync();
    }

    private async Task DeleteCarroAsync()
    {
        if (CarroSeleccionado == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Selecciona un carro para eliminar", "OK");
            return;
        }

        var confirm = await Application.Current.MainPage.DisplayAlert("Confirmar", "¿Estás seguro de que deseas eliminar este carro?", "Sí", "No");
        if (!confirm)
            return;

        // Eliminar el carro de la base de datos
        await _databaseService.DeleteCarroAsync(CarroSeleccionado);

        // Eliminar el carro de la lista observable
        Carros.Remove(CarroSeleccionado);

        await Application.Current.MainPage.DisplayAlert("Éxito", "Carro eliminado correctamente", "OK");
    }

    private async Task EditCarroAsync()
    {
        if (CarroSeleccionado == null)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Selecciona un carro para editar", "OK");
            return;
        }

        if (string.IsNullOrWhiteSpace(CarroSeleccionado.Marca) || string.IsNullOrWhiteSpace(CarroSeleccionado.Modelo))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Completa todos los campos", "OK");
            return;
        }

        // Actualizar el carro en la base de datos
        await _databaseService.UpdateCarroAsync(CarroSeleccionado);

        // Recargar los datos para reflejar el cambio
        LoadCarros();

        await Application.Current.MainPage.DisplayAlert("Éxito", "Carro actualizado correctamente", "OK");
    }

}
