using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AIHospitalManagementSys.DTOs.Ai;
using AIHospitalManagementSys.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIHospitalManagementSys.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AiController : ControllerBase
    {
        private readonly IAiService _aiService;

        public AiController(IAiService aiService)
        {
            _aiService = aiService;
        }

        [HttpPost("query")]
        public async Task<IActionResult> Query([FromBody] AiRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Question))
            {
                return BadRequest(new { Message = "Question cannot be empty." });
            }

            // Securely extract the user identity
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            request.UserId = userId;
            
            // Assign role securely from claims (ignoring whatever the client sent)
            if (User.IsInRole("Admin"))
            {
                request.Role = "Admin";
                // Admin can query any context, defaulting to Admin if not specified
                request.ContextType = string.IsNullOrEmpty(request.ContextType) ? "Admin" : request.ContextType;
            }
            else if (User.IsInRole("Doctor"))
            {
                request.Role = "Doctor";
                // Doctors usually query Clinical or specific patient data
                if (request.ContextType == "Admin") request.ContextType = "Clinical"; // Restrict admin context
            }
            else if (User.IsInRole("Receptionist"))
            {
                request.Role = "Receptionist";
                // Receptionists usually query Admissions or Billing
                if (request.ContextType == "Admin" || request.ContextType == "Clinical") 
                    request.ContextType = "Admissions"; 
            }
            else if (User.IsInRole("Patient"))
            {
                request.Role = "Patient";
                request.ContextType = "Patient"; // Force context
                
                // Ensure patients can only retrieve their own data
                if (!string.IsNullOrEmpty(request.PatientId) && request.PatientId != userId)
                {
                    return Forbid("Patients cannot query other patient records.");
                }
                request.PatientId = userId; 
            }
            else
            {
                // Fallback for general authenticated users
                request.Role = "General";
                request.ContextType = "General";
            }

            // Ensure a session exists
            if (string.IsNullOrWhiteSpace(request.SessionId))
            {
                request.SessionId = Guid.NewGuid().ToString();
            }

            var response = await _aiService.QueryAsync(request);
            return Ok(response);
        }
    }
}
