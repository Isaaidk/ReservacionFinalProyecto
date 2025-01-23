using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Models
{
    public class Reservacion
    {
        [PrimaryKey, AutoIncrement]
        public int IdReservacion { get; set; }
        public int IdCarro { get; set; }
        public int IdUsuario { get; set; }
        public DateTime FechaReserva { get; set; }

        [Ignore]
        public string NombreUsuario { get; set; }
        [Ignore]
        public string NombreCarro { get; set; }
    }
}
