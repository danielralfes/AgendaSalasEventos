using System;

namespace AgendaSalasEventos.Api.Models.Request
{
    public class RequestEvento
    {
        public Guid? EventoId { get; set; }
        public int SalaId { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Responsavel { get; set; }

    }
}
