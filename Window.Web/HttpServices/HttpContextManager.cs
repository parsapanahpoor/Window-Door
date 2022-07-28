namespace Window.Web.HttpServices
{
    public static class HttpContextManager
    {
        public static string GetUserIP(this HttpContext context)
        {
            return context.Connection.RemoteIpAddress.ToString();
        }

        public static string GetUrlReferer(this HttpRequest request)
        {
            return request.Headers["Referer"].ToString();
        }

        public static ApiAuthTokenDto GetUserInfo(this HttpContext context)
        {
            return context.Items.SingleOrDefault(s => s.Key == "UserInfo").Value as ApiAuthTokenDto;
        }
    }
}
