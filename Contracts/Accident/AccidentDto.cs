using System.Text.Json.Serialization;

namespace Marc2.Contracts.Accident
{
    public class AccidentDto
    {
        [JsonPropertyName("accidentId")]
        public int Id { get; set; }
        public string GeneralDescription { get; set; } = null!;
        public string AccidentName { get; set; } = null!;
        public double Lattitude { get; set; }
        public double Longtitude { get; set; }
        public string Address { get; set; } =  null!;
        public int OrganizationId { get; set; }
        public DateTime AroseAt { get; set; }
    }
}
