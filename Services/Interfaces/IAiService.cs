using System.Threading.Tasks;
using AIHospitalManagementSys.DTOs.Ai;

namespace AIHospitalManagementSys.Services.Interfaces
{
    public interface IAiService
    {
        Task<AiResponseDto> QueryAsync(AiRequestDto request);
    }
}
