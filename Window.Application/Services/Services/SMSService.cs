using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Kavenegar.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Window.Application.Services.Interfaces;

namespace Window.Application.Services.Implementation
{
    public class SMSService : ISMSService
    {
        private readonly HttpClient _client;
        private readonly IConfiguration _configuration;
        private readonly ILogger<SMSService> _logger;

        public SMSService(HttpClient client, IConfiguration configuration, ILogger<SMSService> logger)
        {
            _client = client;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<SendResult?> SendLookupSMS(string receptor, string token, string? token2, string? token3, string template)
        {
            var apiKey = _configuration["kavenegar:apikey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                _logger.LogWarning("Kavenegar lookup API key is missing.");
                return null;
            }

            using var form = new FormUrlEncodedContent(new Dictionary<string, string?>
            {
                ["receptor"] = receptor,
                ["token"] = token,
                ["token2"] = token2,
                ["token3"] = token3,
                ["template"] = template,
                ["type"] = "sms"
            });

            try
            {
                using var response = await _client.PostAsync($"{Uri.EscapeDataString(apiKey.Trim())}/verify/lookup.json", form);
                var result = await response.Content.ReadFromJsonAsync<LookupResponse>();
                var message = result?.Entries?.FirstOrDefault();
                if (response.IsSuccessStatusCode && result?.Return?.Status == 200 &&
                    message is { Messageid: > 0, Status: 1 or 2 or 4 or 5 or 10 })
                {
                    return message;
                }

                _logger.LogWarning(
                    "Kavenegar lookup was not accepted. HTTP {HttpStatus}, API {ApiStatus}, message {MessageStatus}.",
                    (int)response.StatusCode, result?.Return?.Status, message?.Status);
            }
            catch (HttpRequestException)
            {
                _logger.LogWarning("Kavenegar lookup request failed.");
            }
            catch (OperationCanceledException)
            {
                _logger.LogWarning("Kavenegar lookup request timed out.");
            }
            catch (JsonException)
            {
                _logger.LogWarning("Kavenegar lookup returned an invalid response.");
            }

            return null;
        }

        public async Task<SendResult?> SendSimpleSMS(string receptor, string message)
        {
            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(_configuration["kavenegar:apikey"]);
            var result = await api.Send("10008663", receptor, message);
            return result;
        }

        private sealed record LookupResponse(
            [property: JsonPropertyName("return")] LookupStatus? Return,
            SendResult[]? Entries);

        private sealed record LookupStatus(int Status);
    }
}
