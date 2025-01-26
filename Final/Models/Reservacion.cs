using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Models
{
    public class Reservacion
    {
        [PrimaryKey, AutoIncrement]
        public int IdReservacion { get; set; }
        [ForeignKey("IdCarro")]
        public int IdCarro { get; set; }
        [ForeignKey("IdUsuario")]
        public int IdUsuario { get; set; }
        public DateTime FechaReserva { get; set; }

        public string EmailUsuario { get; set; }
   
        public string NombreCarro { get; set; }

        [Ignore]
        public Carro Carro { get; set; }

        [Ignore]
        public Usuario Usuario { get; set; }
    }
}
