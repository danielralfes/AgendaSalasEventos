using System.Text.Json;

namespace AgendaSalasEventos.Api
{
    public static class Util
    {
        public static string GetJSONResultado(object objToConverter)
        {
            return JsonSerializer.Serialize(objToConverter);
        }
    }
}
