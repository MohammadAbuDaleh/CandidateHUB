using CandidateHubApi.Dtos;
using CandidateHubApi.Interfaces;
using CandidateHubApi.Models;
using Microsoft.Extensions.Caching.Memory;

namespace CandidateHubApi.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _repository;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

        public CandidateService(ICandidateRepository repository, IMemoryCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task AddOrUpdateCandidateAsync(CandidateDto candidateDto)
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

            // Invalidate cache if a candidate is added or updated
            _cache.Remove(candidateDto.Email.ToLower());
        }

        public async Task<CandidateDto> GetCandidateByEmailAsync(string email)
        {
            if (_cache.TryGetValue(email.ToLower(), out CandidateDto cachedCandidate))
            {
                return cachedCandidate;
            }

            var candidate = await _repository.GetCandidateByEmailAsync(email);
            if (candidate == null)
            {
                return null;
            }

            var candidateDto = new CandidateDto
            {
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                Email = candidate.Email,
                PhoneNumber = candidate.PhoneNumber,
                CallInterval = candidate.CallInterval,
                LinkedInProfileUrl = candidate.LinkedInProfileUrl,
                GitHubProfileUrl = candidate.GitHubProfileUrl,
                FreeTextComment = candidate.FreeTextComment
            };

            // Set cache options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(_cacheExpiration);

            // Save data in cache
            _cache.Set(email.ToLower(), candidateDto, cacheEntryOptions);

            return candidateDto;
        }
    }
}
