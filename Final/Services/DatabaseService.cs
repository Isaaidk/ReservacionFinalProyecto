using Final.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Services
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _database;


        public DatabaseService(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Carro>().Wait();
            _database.CreateTableAsync<Usuario>().Wait();
            _database.CreateTableAsync<Reservacion>().Wait();
        }

        // Carro Operations
        public Task<List<Carro>> GetCarrosAsync() => _database.Table<Carro>().ToListAsync();
        public Task<int> SaveCarroAsync(Carro carro) => _database.InsertAsync(carro);
        public Task<int> UpdateCarroAsync(Carro carro)
        {
            return _database.UpdateAsync(carro);
        }

        public Task<int> DeleteCarroAsync(Carro carro)
        {
            return _database.DeleteAsync(carro);
        }

        // Usuario Operations
        public Task<List<Usuario>> GetUsuariosAsync() => _database.Table<Usuario>().ToListAsync();
        public Task<int> SaveUsuarioAsync(Usuario usuario) => _database.InsertAsync(usuario);
        public Task<Usuario> GetUsuarioByEmailAsync(string email) => _database.Table<Usuario>().FirstOrDefaultAsync(u => u.Email == email);
    
        // Reservacion Operations
        // Añadir una nueva reservación
        public Task<int> SaveReservacionAsync(Reservacion reservacion) => _database.InsertAsync(reservacion);

        // Obtener todas las reservaciones
        public Task<List<Reservacion>> GetReservacionesAsync() => _database.Table<Reservacion>().ToListAsync();

        // Obtener reservaciones por el email del usuario
        public Task<List<Reservacion>> GetReservacionesByEmailAsync(string email)
        {
            return _database.Table<Reservacion>().Where(r => r.EmailUsuario == email).ToListAsync();

        }

        // Actualizar una reservación existente
        public Task<int> UpdateReservacionAsync(Reservacion reservacion) => _database.UpdateAsync(reservacion);

        // Eliminar una reservación
        public Task<int> DeleteReservacionAsync(Reservacion reservacion) => _database.DeleteAsync(reservacion);

        // Eliminar todas las reservaciones
        public Task<int> DeleteAllReservacionesAsync() => _database.DeleteAllAsync<Reservacion>();




        public Task<Carro> GetCarroByIdAsync(int idCarro) => _database.Table<Carro>().FirstOrDefaultAsync(c => c.IdCarro == idCarro);
    }


}



