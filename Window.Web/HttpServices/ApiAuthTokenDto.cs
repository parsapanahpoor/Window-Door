namespace Window.Web.HttpServices
{
    public class ApiAuthTokenDto
    {
        public string FullName { get; set; }

        public string Email { get; set; }

        public string UniqueKey { get; set; }

        public long UserId { get; set; }

        public bool IsValid { get; set; } = true;

        public bool IsAllDataFilled()
        {
            return Email != null && FullName != null && UserId != 0 && UniqueKey != null;
        }
    }
}
