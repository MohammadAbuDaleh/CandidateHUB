using CandidateHubApi.Data;
using CandidateHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CandidateHubApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CandidatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> UpsertCandidate(Candidate candidate)
        {
            var existingCandidate = await _context.Candidates
                .FirstOrDefaultAsync(c => c.Email == candidate.Email);

            if (existingCandidate == null)
            {
                _context.Candidates.Add(candidate);
            }
            else
            {
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.CallInterval = candidate.CallInterval;
                existingCandidate.LinkedInProfileUrl = candidate.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidate.GitHubProfileUrl;
                existingCandidate.FreeTextComment = candidate.FreeTextComment;
            }

            await _context.SaveChangesAsync();
            return Ok(candidate);
        }
    }
}
