using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AgendaSalasEventos.Api.Models
{
    public class Sala
    {
        [Key]
        public int SalaId { get; set; }
        public string  Nome { get; set; }
        public List<Evento> Eventos { get; set; } = new List<Evento>();
    }
}
