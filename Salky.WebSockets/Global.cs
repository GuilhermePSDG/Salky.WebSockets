global using static Salky.WebSockets.Global;
global using System.Text.Json;
using System.Text.Json.Serialization;

namespace Salky.WebSockets
{
    internal static class Global
    {
        public static JsonSerializerOptions DefaultJsonSerializerOptions = CreateOptions();

        private static JsonSerializerOptions CreateOptions()
        {
            var opt = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            opt.Converters.Add(new JsonStringEnumConverter());
            return opt;
        }
    }
}
