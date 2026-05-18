using System.Text.Json.Serialization;

namespace AIHospitalManagementSys.DTOs.Ai
{
    public class AiRequestDto
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("context_type")]
        public string? ContextType { get; set; } = "General";

        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("user_id")]
        public string? UserId { get; set; }

        [JsonPropertyName("patient_id")]
        public string? PatientId { get; set; }
    }
}
