using AgendaSalasEventos.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AgendaSalasEventos.Api.Data
{
    public interface IAplicacaoContext : IDisposable
    {
        DbSet<Evento> Eventos { get; set; }
        DbSet<Sala> Salas { get; set; }
    }
}
