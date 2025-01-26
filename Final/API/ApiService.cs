using Final.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.API
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Carro>> GetCarrosAsync()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:5000/api/carro");
            return JsonConvert.DeserializeObject<List<Carro>>(response);
        }

        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:5000/api/usuario");
            return JsonConvert.DeserializeObject<List<Usuario>>(response);
        }

        public async Task<List<Reservacion>> GetReservacionesAsync()
        {
            var response = await _httpClient.GetStringAsync("http://localhost:5000/api/reservacion");
            return JsonConvert.DeserializeObject<List<Reservacion>>(response);
        }
    }
}
