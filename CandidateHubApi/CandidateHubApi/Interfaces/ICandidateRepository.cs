using CandidateHubApi.Models;

namespace CandidateHubApi.Interfaces
{
    public interface ICandidateRepository
    {
        Task<Candidate> GetCandidateByEmailAsync(string email);
        Task AddCandidateAsync(Candidate candidate);
        Task UpdateCandidateAsync(Candidate candidate);
        Task SaveChangesAsync();
    }   
}
