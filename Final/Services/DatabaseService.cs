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
        public Task<List<Reservacion>> GetReservacionesAsync() => _database.Table<Reservacion>().ToListAsync();
        public Task<int> SaveReservacionAsync(Reservacion reservacion) => _database.InsertAsync(reservacion);
    }
}
