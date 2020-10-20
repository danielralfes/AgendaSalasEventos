using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AgendaSalasEventos.Api.Business;
using AgendaSalasEventos.Api.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AgendaSalasEventos.Api.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class SalasController : ControllerBase
    {
        private readonly ILogger<SalasController> _logger;
        private readonly SalasService _service;

        public SalasController(ILogger<SalasController> logger, SalasService service)
        {
            _logger  = logger;
            _service = service;
        }

        [HttpGet("combo")]
        [ProducesResponseType(typeof(ResponseSalasCombo), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<ResponseSalasCombo>>> ListarTodasSalasCombo()
        {
            _logger.LogInformation($"Listagem de todos as salas para o combo");

            try
            {
                var resultado = await _service.ListarTodasSalasCombo();

                if (!resultado.Any())
                    return NotFound("Não existe registros de salas");

                return Ok(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na chamada do 'Get'");

                return BadRequest("Erro ao executar chamada");
            }
        }

    }
}
