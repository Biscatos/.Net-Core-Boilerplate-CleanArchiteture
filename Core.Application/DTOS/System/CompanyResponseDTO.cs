namespace Wizzzard.Application.DTOS
{
    public class CompanyResponseDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LogoFileName { get; set; }
        public string EmisNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string WebSite { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
