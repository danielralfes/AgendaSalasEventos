
namespace AgendaSalasEventos.Api.Models.Response
{
    public class ResponseSalas
    {
        public int SalaId { get; set; }
        public string SalaNome { get; set; }
    }

    public class ResponseSalasCombo
    {
        public int Key { get; set; }
        public string Value { get; set; }
    }
}
