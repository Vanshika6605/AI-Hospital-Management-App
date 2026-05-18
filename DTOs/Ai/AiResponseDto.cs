using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AIHospitalManagementSys.DTOs.Ai
{
    public class HallucinationCheckDto
    {
        [JsonPropertyName("hallucination_risk")]
        public double HallucinationRisk { get; set; }

        [JsonPropertyName("unsupported_claims")]
        public List<string> UnsupportedClaims { get; set; } = new List<string>();
    }

    public class AiResponseDto
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("answer")]
        public string Answer { get; set; }

        [JsonPropertyName("abstained")]
        public bool Abstained { get; set; }

        [JsonPropertyName("confidence")]
        public double Confidence { get; set; }

        [JsonPropertyName("grounding_warnings")]
        public List<string> GroundingWarnings { get; set; } = new List<string>();

        [JsonPropertyName("hallucination")]
        public HallucinationCheckDto Hallucination { get; set; } = new HallucinationCheckDto();

        [JsonPropertyName("used_chunks")]
        public List<string> UsedChunks { get; set; } = new List<string>();
    }
}
