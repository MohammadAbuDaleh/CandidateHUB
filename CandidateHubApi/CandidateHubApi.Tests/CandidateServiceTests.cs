using System.Threading.Tasks;
using Xunit;
using Moq;
using CandidateHubApi.Repositories;
using CandidateHubApi.Services;
using CandidateHubApi.Models;
using CandidateHubApi.Dtos;
using CandidateHubApi.Interfaces;

namespace CandidateHubApi.Tests
{
    public class CandidateServiceTests
    {
        private readonly Mock<ICandidateRepository> _mockRepo;
        private readonly CandidateService _service;

        public CandidateServiceTests()
        {
            _mockRepo = new Mock<ICandidateRepository>();
            _service = new CandidateService(_mockRepo.Object);
        }

        [Fact]
        public async Task AddOrUpdateCandidateAsync_AddsNewCandidate_WhenEmailDoesNotExist()
        {
            // Arrange
            var candidateDto = new CanididateDto
            {
                FirstName = "Jane",
                LastName = "Doe",
                Email = "jane.doe@example.com",
                FreeTextComment = "Interested in backend roles."
            };

            _mockRepo.Setup(repo => repo.GetCandidateByEmailAsync(candidateDto.Email))
                     .ReturnsAsync((Candidate)null);

            // Act
            await _service.AddOrUpdateCandidateAsync(candidateDto);

            // Assert
            _mockRepo.Verify(repo => repo.AddCandidateAsync(It.Is<Candidate>(c =>
                c.FirstName == candidateDto.FirstName &&
                c.LastName == candidateDto.LastName &&
                c.Email == candidateDto.Email &&
                c.FreeTextComment == candidateDto.FreeTextComment
            )), Times.Once);

            _mockRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task AddOrUpdateCandidateAsync_UpdatesExistingCandidate_WhenEmailExists()
        {
            // Arrange
            var candidateDto = new CanididateDto
            {
                FirstName = "John",
                LastName = "Smith",
                Email = "john.smith@example.com",
                FreeTextComment = "Updated comments."
            };

            var existingCandidate = new Candidate
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "john.smith@example.com",
                FreeTextComment = "Initial comments."
            };

            _mockRepo.Setup(repo => repo.GetCandidateByEmailAsync(candidateDto.Email))
                     .ReturnsAsync(existingCandidate);

            // Act
            await _service.AddOrUpdateCandidateAsync(candidateDto);

            // Assert
            Assert.Equal(candidateDto.FirstName, existingCandidate.FirstName);
            Assert.Equal(candidateDto.LastName, existingCandidate.LastName);
            Assert.Equal(candidateDto.FreeTextComment, existingCandidate.FreeTextComment);

            _mockRepo.Verify(repo => repo.UpdateCandidateAsync(existingCandidate), Times.Once);
            _mockRepo.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }
    }
}
