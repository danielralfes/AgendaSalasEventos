using AgendaSalasEventos.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AgendaSalasEventos.Api.Data
{
    public class AplicacaoContext : DbContext, IAplicacaoContext
    {
        public AplicacaoContext(DbContextOptions<AplicacaoContext> options) : base(options)
        {
            SeedData();
        }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Sala> Salas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Evento>()
                   .Property(p => p.EventoId)
                   .IsRequired();
            builder.Entity<Evento>()
                   .Property(p => p.Responsavel)
                   .HasMaxLength(100);

            builder.Entity<Sala>()
                   .Property(p => p.SalaId)
                   .IsRequired();
            builder.Entity<Sala>()
                   .Property(p => p.Nome)
                   .HasMaxLength(50);

            base.OnModelCreating(builder);
        }

        private void SeedData()
        {
            //Obs:
            //Seed apenas para testes em memória
            //Em ambiente normal não usar.
            //Seed usado apenas para inicializar os dados em quando é usado codefirst e apenas uma vez

            if (Salas.Count() == 0)
            {
                //Criação Inicial de Salas para realizar testes
                Salas.Add(new Sala { SalaId = 1, Nome = "C#" });
                Salas.Add(new Sala { SalaId = 2, Nome = "Javascript" });
                Salas.Add(new Sala { SalaId = 3, Nome = "Angular" });
                Salas.Add(new Sala { SalaId = 4, Nome = "React" });

                //Criação Inicial de Eventos para realizar testes
                Eventos.Add(new Evento { SalaId = 1, EventoId = Guid.NewGuid(), DataInicial = DateTime.Now.AddDays(1), DataFinal = DateTime.Now.AddDays(2), Responsavel = "José Resp1" });
                Eventos.Add(new Evento { SalaId = 1, EventoId = Guid.NewGuid(), DataInicial = DateTime.Now.AddDays(3), DataFinal = DateTime.Now.AddDays(4), Responsavel = "José Resp1" });
                Eventos.Add(new Evento { SalaId = 1, EventoId = Guid.NewGuid(), DataInicial = DateTime.Now.AddDays(4), DataFinal = DateTime.Now.AddDays(6), Responsavel = "José Resp2" });
                Eventos.Add(new Evento { SalaId = 2, EventoId = Guid.NewGuid(), DataInicial = DateTime.Now.AddDays(7), DataFinal = DateTime.Now.AddDays(8), Responsavel = "José Resp3" });
                Eventos.Add(new Evento { SalaId = 2, EventoId = Guid.NewGuid(), DataInicial = DateTime.Now.AddDays(9), DataFinal = DateTime.Now.AddDays(9), Responsavel = "José Resp333" });
                Eventos.Add(new Evento { SalaId = 3, EventoId = Guid.NewGuid(), DataInicial = DateTime.Now.AddDays(10), DataFinal = DateTime.Now.AddDays(11), Responsavel = "José Teste" });

                SaveChanges();
            }
        }
    }
}
