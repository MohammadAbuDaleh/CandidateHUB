using System.ComponentModel.DataAnnotations;

namespace CandidateHubApi.Dtos
{
    public class CandidateDto
    {
        public long? Id { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } // Unique identifier
        public string? CallInterval { get; set; }
        [Url]
        public string? LinkedInProfileUrl { get; set; }
        [Url]
        public string? GitHubProfileUrl { get; set; }
        [Required]
        public string FreeTextComment { get; set; }

    }
}
