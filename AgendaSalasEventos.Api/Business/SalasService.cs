﻿using AgendaSalasEventos.Api.Controllers;
using AgendaSalasEventos.Api.Data;
using AgendaSalasEventos.Api.DI;
using AgendaSalasEventos.Api.Models.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaSalasEventos.Api.Business
{
    public class SalasService : ISalasService
    {
        private readonly ILogger<SalasController> _logger;
        private readonly AplicacaoContext _db;

        public SalasService(ILogger<SalasController> logger, AplicacaoContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<List<ResponseSalasCombo>> ListarTodasSalasCombo()
        {
            _logger.LogInformation("[SalasService] Listar todos os registros");

            var retornaLista = await _db.Salas
                                        .Select(s => new ResponseSalasCombo()
                                        {
                                            Key    = s.SalaId,
                                            Value  = s.Nome,
                                        })
                                        .OrderBy(t => t.Value)
                                        .AsNoTracking()
                                        .ToListAsync();

            return retornaLista;
        }

    }
}
