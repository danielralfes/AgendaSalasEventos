using AgendaSalasEventos.Api.Models;
using AgendaSalasEventos.Api.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaSalasEventos.Api.DI
{
    public interface IEventosService
    {
        Task<Resultado> CriarEventoDaSala(Evento evento);
        Task<Resultado> ExcluirEventoDaSala(Guid eventoId);
        Task<Resultado> AtualizarEventoDaSala(Evento evento);
        Task<bool> VerificarEventoHorarioExistente(int salaId, Guid? eventoId, DateTime dataInicial, DateTime dataFinal);
        Task<List<ResponseSalasEventos>> ListarTodosEventos();
    }
}
