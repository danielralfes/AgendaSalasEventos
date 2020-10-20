using System;
using System.ComponentModel.DataAnnotations;

namespace AgendaSalasEventos.Api.Models
{
    public class Evento
    {
        [Key]
        public Guid EventoId { get; set; }
        public int SalaId { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Responsavel { get; set; }

        public Sala Sala{ get; set; }
    }
}
