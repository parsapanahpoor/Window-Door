using Newtonsoft.Json;
using System.Text;

namespace Window.Web.HttpServices
{
    public static class ApiExtensions
    {
        public static async Task<T> GetMapedData<T>(this HttpResponseMessage response)
        {
            var stringData = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringData);
        }

        public static StringContent GetJsonStringContent(this object? value)
        {
            string json = JsonConvert.SerializeObject(value);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
