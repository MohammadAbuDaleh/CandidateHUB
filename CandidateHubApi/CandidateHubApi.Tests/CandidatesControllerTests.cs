using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using CandidateHubApi.Controllers;
using CandidateHubApi.Services;
using CandidateHubApi.Models;
using CandidateHubApi.Dtos;
using CandidateHubApi.Interfaces;

namespace CandidateHubApi.Tests
{
    public class CandidatesControllerTests
    {
        private readonly Mock<ICandidateService> _mockService;
        private readonly CandidatesController _controller;

        public CandidatesControllerTests()
        {
            _mockService = new Mock<ICandidateService>();
            _controller = new CandidatesController(_mockService.Object);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_ReturnsOk_WhenModelIsValid()
        {
            // Arrange
            var candidateDto = new CandidateDto
            {
                FirstName = "Alice",
                LastName = "Johnson",
                Email = "alice.johnson@example.com",
                FreeTextComment = "Looking for frontend positions."
            };

            // Act
            var result = await _controller.AddOrUpdateCandidate(candidateDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);
            Assert.Equal("Candidate information has been successfully added or updated.",
                         ((dynamic)okResult.Value).Message);
            _mockService.Verify(s => s.AddOrUpdateCandidateAsync(candidateDto), Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            var candidateDto = new CandidateDto
            {
                // Missing required fields
                Email = "invalid-email",
                FreeTextComment = ""
            };

            _controller.ModelState.AddModelError("FirstName", "Required");
            _controller.ModelState.AddModelError("LastName", "Required");
            _controller.ModelState.AddModelError("Comments", "Required");

            // Act
            var result = await _controller.AddOrUpdateCandidate(candidateDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(400, badRequestResult.StatusCode);
            _mockService.Verify(s => s.AddOrUpdateCandidateAsync(It.IsAny<CandidateDto>()), Times.Never);
        }
    }
}
