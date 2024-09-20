using CandidateHubApi.Dtos;

namespace CandidateHubApi.Interfaces
{
    public interface ICandidateService
    {
        Task AddOrUpdateCandidateAsync(CanididateDto candidateDto);
    }
}
