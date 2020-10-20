using AgendaSalasEventos.Api.Models.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AgendaSalasEventos.Api.DI
{
    public interface ISalasService
    {
        Task<List<ResponseSalasCombo>> ListarTodasSalasCombo();
    }
}
