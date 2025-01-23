using Final.Models;
using Final.Services;
using Final.ViewModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class CarroViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    public ObservableCollection<Carro> Carros { get; set; } = new();
    public Carro NuevoCarro { get; set; } = new();

    public ICommand AddCarroCommand { get; }

    public CarroViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
        LoadCarros();

        // Inicializar el comando
        AddCarroCommand = new Command(async () => await AddCarroAsync());
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
        Carros.Add(NuevoCarro);

        // Reiniciar el formulario
        NuevoCarro = new Carro();
        OnPropertyChanged(nameof(NuevoCarro));

        await Application.Current.MainPage.DisplayAlert("Éxito", "Auto agregado correctamente", "OK");
    }
}
