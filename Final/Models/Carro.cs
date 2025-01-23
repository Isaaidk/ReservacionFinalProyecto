using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Models
{
    public class Carro
    {
        [PrimaryKey, AutoIncrement]
        public int IdCarro { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
    }
}
