using System.Text.Json.Serialization;

namespace Api.Test.Domain.Application.Validations
{
    public class ErrorResponse
    {
        [JsonPropertyName("codigo")]
        public string Codigo { get; init; }
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; init; }
    }
}
