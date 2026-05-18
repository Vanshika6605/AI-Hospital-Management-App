using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AIHospitalManagementSys.DTOs.Ai;
using AIHospitalManagementSys.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace AIHospitalManagementSys.Services.Implementations
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AiService> _logger;

        public AiService(HttpClient httpClient, ILogger<AiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<AiResponseDto> QueryAsync(AiRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("query", request);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"FastAPI backend returned status code {response.StatusCode}");
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error Details: {errorContent}");
                    return GetFallbackResponse(request.Question, "The MediAI Assistant is currently experiencing connection issues.");
                }

                var aiResponse = await response.Content.ReadFromJsonAsync<AiResponseDto>();
                if (aiResponse == null)
                {
                    _logger.LogError("FastAPI response could not be deserialized.");
                    return GetFallbackResponse(request.Question, "The MediAI Assistant returned an invalid format.");
                }

                return aiResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Exception while contacting FastAPI RAG backend: {ex.Message}");
                return GetFallbackResponse(request.Question, "The MediAI Assistant is currently offline or unreachable.");
            }
        }

        private AiResponseDto GetFallbackResponse(string query, string message)
        {
            return new AiResponseDto
            {
                Query = query,
                Answer = message,
                Abstained = true,
                Confidence = 0.0,
                GroundingWarnings = new System.Collections.Generic.List<string> { "backend_error" }
            };
        }
    }
}
