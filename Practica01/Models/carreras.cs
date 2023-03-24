using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace Practica01.Models
{
    public class carreras
    {
        [Key]
        public int carrera_id { get; set; }
        public string nombre_carrera { get; set; }
        public int facultad_id { get; set; }
        public string estado { get; set; }

    }
}
