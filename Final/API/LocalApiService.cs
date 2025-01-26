using Final.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Final.API
{
    public class LocalApiService
    {
        private readonly DatabaseService _databaseService;

        public LocalApiService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task StartApiAsync()
        {
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/"); // Define el puerto y la URL base

            listener.Start();
            Console.WriteLine("API Local Iniciada en http://localhost:5000/");

            while (true)
            {
                var context = await listener.GetContextAsync();
                HandleRequest(context);
            }
        }

        private async void HandleRequest(HttpListenerContext context)
        {
            var response = context.Response;
            var request = context.Request;
            string responseString = "";
            var statusCode = HttpStatusCode.OK;

            try
            {
                if (request.Url.AbsolutePath == "/api/carro" && request.HttpMethod == "GET")
                {
                    var carros = await _databaseService.GetCarrosAsync();
                    responseString = JsonConvert.SerializeObject(carros);
                }
                else if (request.Url.AbsolutePath == "/api/usuario" && request.HttpMethod == "GET")
                {
                    var usuarios = await _databaseService.GetUsuariosAsync();
                    responseString = JsonConvert.SerializeObject(usuarios);
                }
                else if (request.Url.AbsolutePath == "/api/reservacion" && request.HttpMethod == "GET")
                {
                    var reservaciones = await _databaseService.GetReservacionesAsync();
                    responseString = JsonConvert.SerializeObject(reservaciones);
                }
                else
                {
                    statusCode = HttpStatusCode.NotFound;
                    responseString = "No encontrado";
                }
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;
                responseString = $"Error: {ex.Message}";
            }

            // Escribir la respuesta
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.StatusCode = (int)statusCode;
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();
        }
    }
    }
