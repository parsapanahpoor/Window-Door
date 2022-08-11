namespace CRM.Domain.DTOs.StructuredApiDtos.Account
{
    public class ApiLoginResponseDto
    {
        public string TokenCode { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? ExpiresAt { get; set; }
    }
}
