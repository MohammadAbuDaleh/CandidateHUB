using CandidateHubApi.Data;
using CandidateHubApi.Dtos;
using CandidateHubApi.Interfaces;
using CandidateHubApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> AddOrUpdateCandidate([FromBody] CandidateDto candidateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _candidateService.AddOrUpdateCandidateAsync(candidateDto);

            return Ok(new { Message = "Candidate information has been successfully added or updated." });
        }

        /// <summary>
        /// Retrieves candidate information by email.
        /// </summary>
        /// <param name="email">Candidate's email.</param>
        /// <returns>Candidate information.</returns>
        [HttpGet("{email}")]
        public async Task<IActionResult> GetCandidateByEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || !new EmailAddressAttribute().IsValid(email))
            {
                return BadRequest("A valid email must be provided.");
            }

            var candidate = await _candidateService.GetCandidateByEmailAsync(email);
            if (candidate == null)
            {
                return NotFound("Candidate not found.");
            }

            return Ok(candidate);
        }
    }
}
