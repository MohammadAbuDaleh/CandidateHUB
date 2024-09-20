using CandidateHubApi.Data;
using CandidateHubApi.Dtos;
using CandidateHubApi.Interfaces;
using CandidateHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CandidateHubApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatesController : ControllerBase
    {
        private readonly ICandidateService _candidateService;

        public CandidatesController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        /// <summary>
        /// Adds a new candidate or updates an existing candidate based on email.
        /// </summary>
        /// <param name="candidateDto">Candidate information.</param>
        /// <returns>Action result.</returns>
        [HttpPost]
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] CanididateDto candidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _candidateService.AddOrUpdateCandidateAsync(candidateDto);

            return Ok(new { Message = "Candidate information has been successfully added or updated." });
        }
    }
}
