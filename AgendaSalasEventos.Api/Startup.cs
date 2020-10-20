using System;
using AgendaSalasEventos.Api.Business;
using AgendaSalasEventos.Api.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace AgendaSalasEventos.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddCors();

            services.AddDbContext<AplicacaoContext>(options =>
                                                    options.UseInMemoryDatabase("DBMemory")); //UseSQlServer


            services.AddMvc(options =>         { options.Filters.Add(new UnhandledExceptionLoggerFilter()); })
                    .AddJsonOptions(options => { options.JsonSerializerOptions.IgnoreNullValues = true; });

            services.AddResponseCompression();

            services.AddScoped<AplicacaoContext>();
            services.AddTransient<EventosService>();
            services.AddTransient<SalasService>();

            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "Agenda de Eventos",
                        Version = "v1",
                        Description = "API REST para manipulação de eventos",
                        Contact = new OpenApiContact
                        {
                            Name = "Daniel Ralfes",
                            Url = new Uri("https://github.com/danielralfes")
                        }
                    });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "[Ralfes v1] Api de Agendamento de Eventos - Salas");
            });

            app.UseHttpsRedirection();

            // Shows UseCors with CorsPolicyBuilder.
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
                        //Usado para definir quais rotas podem acessar) - Deixei comentando para edeito de testes
                        //.WithOrigins(
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
