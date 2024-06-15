using Newtonsoft.Json;

namespace Shared.Models
{
    public class Comunidad
    {
        [JsonProperty(propertyName: "parent_code")]
        public string? IdPais { get; set; }

        [JsonProperty(propertyName: "label")]
        public string? Nombre { get; set; }

        [JsonProperty(propertyName: "code")]
        public string? Id { get; set; }

        [JsonProperty(propertyName: "provinces")]
        public IEnumerable<Provincia>? Provincias { get; set; }
    }

    public class Provincia
    {
        [JsonProperty(propertyName: "parent_code")]
        public string? IdComunidad { get; set; }

        [JsonProperty(propertyName: "code")]
        public string? Id { get; set; }

        [JsonProperty(propertyName: "label")]
        public string? Nombre { get; set; }

        [JsonProperty(propertyName: "towns")]
        public IEnumerable<Ciudad>? Ciudades { get; set; }
    }

    public class Ciudad
    {
        [JsonProperty(propertyName: "parent_code")]
        public string? IdProvincia { get; set; }

        [JsonProperty(propertyName: "code")]
        public string? Id { get; set; }

        [JsonProperty(propertyName: "label")]
        public string? Nombre { get; set; }
    }
}
