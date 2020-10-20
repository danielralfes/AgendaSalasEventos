using AgendaSalasEventos.Api.Business;
using AgendaSalasEventos.Api.Controllers;
using AgendaSalasEventos.Api.Data;
using AgendaSalasEventos.Api.DI;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AgendaSalasEventos.ApiTest
{
    [TestClass]
    public class TestSalasController
    {

        private readonly ServiceProvider serviceProvider;

        public TestSalasController()
        {
            var services = new ServiceCollection();

            services.AddLogging()
                    .AddTransient<SalasController>()
                    .AddTransient<SalasService>()
                    .AddSingleton<IAplicacaoContext, AplicacaoContext>()
                    .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);

            serviceProvider = services.BuildServiceProvider();
        }

        [TestMethod]
        public void TestMethodGetSala()
        {
            ////var testSala = GetTest
            var sala = new SalasController(serviceProvider.GetService<ILogger<SalasController>>(), serviceProvider.GetService<SalasService>());

            var resp = Task.Run(async () =>
            {
                return await sala.ListarTodasSalasCombo();

            }).GetAwaiter().GetResult().Value;

            Assert.IsNotNull(resp);
            Assert.IsTrue(resp.Any());
        }
    }
}
