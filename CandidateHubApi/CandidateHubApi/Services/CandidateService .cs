using CandidateHubApi.Dtos;
using CandidateHubApi.Interfaces;
using CandidateHubApi.Models;

namespace CandidateHubApi.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _repository;

        public CandidateService(ICandidateRepository repository)
        {
            _repository = repository;
        }

        public async Task AddOrUpdateCandidateAsync(CanididateDto candidateDto)
        {
            var existingCandidate = await _repository.GetCandidateByEmailAsync(candidateDto.Email);

            if (existingCandidate != null)
            {
                // Update existing candidate
                existingCandidate.FirstName = candidateDto.FirstName;
                existingCandidate.LastName = candidateDto.LastName;
                existingCandidate.PhoneNumber = candidateDto.PhoneNumber;
                existingCandidate.CallInterval = candidateDto.CallInterval;
                existingCandidate.LinkedInProfileUrl = candidateDto.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidateDto.GitHubProfileUrl;
                existingCandidate.FreeTextComment = candidateDto.FreeTextComment;

                await _repository.UpdateCandidateAsync(existingCandidate);
            }
            else
            {
                // Create new candidate
                var newCandidate = new Candidate
                {
                    FirstName = candidateDto.FirstName,
                    LastName = candidateDto.LastName,
                    Email = candidateDto.Email,
                    PhoneNumber = candidateDto.PhoneNumber,
                    CallInterval = candidateDto.CallInterval,
                    LinkedInProfileUrl = candidateDto.LinkedInProfileUrl,
                    GitHubProfileUrl = candidateDto.GitHubProfileUrl,
                    FreeTextComment = candidateDto.FreeTextComment
                };

                await _repository.AddCandidateAsync(newCandidate);
            }

            await _repository.SaveChangesAsync();
        }
    }
}
