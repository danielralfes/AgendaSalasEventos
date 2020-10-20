using System;

namespace AgendaSalasEventos.Api.Models.Response
{
    public class ResponseSalasEventos
    {
        public Guid EventoId { get; set; }
        public int SalaId { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public string Responsavel { get; set; }

        public string SalaNome { get; set; }
    }
}
