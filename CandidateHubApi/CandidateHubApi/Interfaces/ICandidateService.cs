using CandidateHubApi.Dtos;

namespace CandidateHubApi.Interfaces
{
    public interface ICandidateService
    {
        Task AddOrUpdateCandidateAsync(CandidateDto candidateDto);
        Task<CandidateDto> GetCandidateByEmailAsync(string email);
    }
}
