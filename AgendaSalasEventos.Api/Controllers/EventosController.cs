using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AgendaSalasEventos.Api.Business;
using AgendaSalasEventos.Api.Models;
using AgendaSalasEventos.Api.Models.Request;
using AgendaSalasEventos.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AgendaSalasEventos.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class EventosController : ControllerBase
    {
        private readonly ILogger<EventosController> _logger;
        private readonly EventosService _service;

        public EventosController(ILogger<EventosController> logger, EventosService service)
        {
            _logger  = logger;
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseSalasEventos), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<ResponseSalasEventos>> ListarTodosEventos()
        {
            _logger.LogInformation($"[SalasEventos.Api.Eventos] Listagem de todos os eventos");

            try
            {
                var resultado = await _service.ListarTodosEventos();

                if (!resultado.Any())
                    return NotFound("Não existe registros de eventos");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SalasEventos.Api.Eventos] Erro na chamada do 'Get'");

                return BadRequest("Erro ao executar chamada");
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Resultado>> CriarEventoDaSala(RequestEvento evento)
        {
            _logger.LogInformation($"[SalasEventos.Api.Eventos] Criar eventos");

            try
            {
                var eventoDb = new Evento() 
                { 
                    EventoId    = Guid.NewGuid(),
                    DataInicial = evento.DataInicial,
                    DataFinal   = evento.DataFinal,
                    Responsavel = evento.Responsavel,
                    SalaId      = evento.SalaId
                };

                var resultado = await _service.CriarEventoDaSala(eventoDb);

                if (resultado.Inconsistencias.Count > 0)
                {
                    _logger.LogError("[SalasEventos.Api.Eventos] " + Util.GetJSONResultado(resultado));
                    return BadRequest(resultado);
                }
                else
                    _logger.LogInformation("[SalasEventos.Api.Eventos] Inclusão efetuada com sucesso");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SalasEventos.Api.Eventos] Erro na chamada do 'Post'");

                return BadRequest(new Resultado() 
                { 
                    Acao            = "Criação de Evento",
                    Inconsistencias = { "Exceção ao efetuar a inclusão do evento" }
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<Resultado>> AtualizarEventoDaSala(RequestEvento evento)
        {
            _logger.LogInformation($"[SalasEventos.Api.Eventos] Atualizar EventoId: {evento.EventoId}");

            try
            {
                var eventoDb = new Evento()
                {
                    EventoId    = evento.EventoId.GetValueOrDefault(),
                    DataInicial = evento.DataInicial,
                    DataFinal   = evento.DataFinal,
                    Responsavel = evento.Responsavel,
                    SalaId      = evento.SalaId
                };

                var resultado = await _service.AtualizarEventoDaSala(eventoDb);

                if (resultado.Inconsistencias.Count > 0)
                {
                    _logger.LogError(Util.GetJSONResultado(resultado));
                    return BadRequest(resultado);
                }
                else
                    _logger.LogInformation("[SalasEventos.Api.Eventos] Alteracao efetuada com sucesso");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SalasEventos.Api.Eventos] Erro na chamada do 'Put'");

                return BadRequest(new Resultado()
                {
                    Acao            = "Atualização de Evento",
                    Inconsistencias = { "Exceção ao efeturar a inclusão do evento" }
                });
            }
        }

        [HttpDelete("{eventoId}")]
        [ProducesResponseType(typeof(Resultado), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<Resultado>> ExcluirEventoDaSala(Guid eventoId)
        {

            _logger.LogInformation($"[SalasEventos.Api.Eventos] Excluir EventoId: {eventoId}");

            try
            {
                var resultado = await _service.ExcluirEventoDaSala(eventoId);
                if (resultado.Inconsistencias.Count > 0)
                {
                    _logger.LogError(Util.GetJSONResultado(resultado));
                    return BadRequest(resultado);
                }
                else
                    _logger.LogInformation("[SalasEventos.Api.Eventos] Exclusão efetuada com sucesso");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[SalasEventos.Api.Eventos] Erro na chamada do 'Delete'");

                return BadRequest(new Resultado()
                {
                    Acao            = "Criação de Evento",
                    Inconsistencias = { "Exceção ao efetuar a deleção do evento" }
                });
            }
        }

    }
}
