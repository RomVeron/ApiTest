using System.Text.Json.Serialization;

namespace Api.Spi.Reportero.Domain.Application.Models
{
    public class ErrorSpi
    {
        [JsonPropertyName("errorCode")]
        public string ErrorCode { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
    }
}
