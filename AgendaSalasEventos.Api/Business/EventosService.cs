using AgendaSalasEventos.Api.Controllers;
using AgendaSalasEventos.Api.Data;
using AgendaSalasEventos.Api.DI;
using AgendaSalasEventos.Api.Models;
using AgendaSalasEventos.Api.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaSalasEventos.Api.Business
{
    public class EventosService : IEventosService
    {
        private readonly ILogger<EventosController> _logger;
        private readonly AplicacaoContext _db;

        public EventosService(ILogger<EventosController> logger, AplicacaoContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<List<ResponseSalasEventos>> ListarTodosEventos()
        {
            _logger.LogInformation("[EventosService] Listar todos os registros");

            var retornaLista = await _db.Eventos
                                        .Include(i => i.Sala)
                                        .Select(s=> new ResponseSalasEventos()
                                        {
                                            EventoId    = s.EventoId,
                                            SalaId      = s.SalaId,
                                            SalaNome    = s.Sala.Nome,
                                            DataInicial = s.DataInicial,
                                            DataFinal   = s.DataFinal,
                                            Responsavel = s.Responsavel
                                        })
                                        .OrderBy(t      => t.SalaId)
                                        .ThenBy(t       => t.DataInicial)
                                        .AsNoTracking()
                                        .ToListAsync();

            return retornaLista;
        }

        public async Task<Resultado> AtualizarEventoDaSala(Evento evento)
        {
            _logger.LogInformation("[EventosService] Atualizar EventoId", evento.EventoId);

            var resultado = new Resultado()
            {
                Acao = "Atualização de Evento"
            };

            var existeHorario = await VerificarEventoHorarioExistente(evento.SalaId, evento.EventoId, evento.DataInicial, evento.DataFinal);

            if (!existeHorario)
            {
                _db.Eventos.Update(evento).State = EntityState.Modified;

                var resultDb = await _db.SaveChangesAsync();

                if (resultDb < 1)
                    resultado.Inconsistencias.Add("Registro não atualizado");
            }
            else 
            {
                resultado.Inconsistencias.Add("Horário coincide com outro evento nesse mesmo horário e sala.");
            }

            return resultado;
        }

        public async Task<Resultado> CriarEventoDaSala(Evento evento)
        {
            _logger.LogInformation("[EventosService] Criar EventoId", evento.EventoId);

            var resultado = new Resultado()
            {
                Acao = "Criação de Evento"
            };

            var existeHorario = await VerificarEventoHorarioExistente(evento.SalaId, evento.EventoId, evento.DataInicial, evento.DataFinal);

            if (!existeHorario)
            {
                _db.Eventos.Add(evento);

                var resultDb = await _db.SaveChangesAsync();

                if (resultDb < 1)
                    resultado.Inconsistencias.Add("Registro não criado");
            }
            else
            {
                resultado.Inconsistencias.Add("Horário coincide com outro evento nessa sala.");
            }

            return resultado;
        }

        public async Task<Resultado> ExcluirEventoDaSala(Guid eventoId)
        {
            _logger.LogInformation("[EventoService] Exclusão EventoId", eventoId);

            var resultado = new Resultado()
            {
                Acao = "Exclusão de Evento"
            };

            _db.Eventos.Remove(new Evento() { EventoId = eventoId });

            var resultDb = await _db.SaveChangesAsync();

            if (resultDb < 1)
                resultado.Inconsistencias.Add("Não foi possível excluir o Produto");

            return resultado;
        }

        public async Task<bool> VerificarEventoHorarioExistente(int salaId, Guid? eventoId, DateTime dataInicial, DateTime dataFinal)
        {
            _logger.LogInformation("[EventosService] Verifica se existem horários conflitantes na SalaId", salaId);

            var existeHorario = await _db.Eventos
                                         .Where(t => t.SalaId == salaId
                                                  && t.EventoId != eventoId
                                                  &&((dataInicial.Date >= t.DataInicial.Date && dataFinal.Date <= t.DataFinal.Date))
                                                )
                                         .AsNoTracking()
                                         .AnyAsync();
            return existeHorario;

            //TODO:Finalizar lógica acima
        }
    }
}
