namespace CRM.Domain.DTOs.StructuredApiDtos.Account
{
    public class ApiLoginDto
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public enum ApiLoginResult
    {
        Success,
        NotActive,
        NotFound
    }
}
