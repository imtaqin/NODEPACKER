using System.Text.Json.Serialization;

namespace NodeJsPacker.Models
{
    public class NodeJsRelease
    {
        [JsonPropertyName("version")]
        public string? Version { get; set; }

        [JsonPropertyName("lts")]
        public object? Lts { get; set; }
    }
}