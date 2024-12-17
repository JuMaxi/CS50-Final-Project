using System.Text.Json;
using Microsoft.Extensions.Options;
using PropagatingKindness.Configuration;

namespace PropagatingKindness.Services;

public record ReCaptchaResponse(bool Success, string ErrorMessage);

public interface IReCaptchaService
{
    Task<ReCaptchaResponse> ValidateRecaptcha(string recaptchaResponse);
}

public class ReCaptchaService : IReCaptchaService
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<ReCaptchaConfiguration> _configuration;

    public ReCaptchaService(HttpClient httpClient, IOptions<ReCaptchaConfiguration> configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<ReCaptchaResponse> ValidateRecaptcha(string recaptchaResponse)
    {
        if (string.IsNullOrWhiteSpace(recaptchaResponse))
        {
            return new ReCaptchaResponse(false, "Please solve the captcha challenge.");
        }

        var values = new Dictionary<string, string>
        {
            { "secret", _configuration.Value.SecretKey },
            { "response", recaptchaResponse }
        };

        var content = new FormUrlEncodedContent(values);
        var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", content);
        var responseString = response.Content.ReadAsStringAsync().Result;
        if (!string.IsNullOrWhiteSpace(responseString))
        {
            var reCaptcha = JsonSerializer.Deserialize<ReCaptchaWebserviceResponse>(responseString);
            if(reCaptcha.success)
                return new ReCaptchaResponse(true, string.Empty);
            else
                return new ReCaptchaResponse(false, "Error validating captcha, please try again.");
        }
        else
        {
            return new ReCaptchaResponse(false, "Error validating captcha, please try again.");
        }
    }
}

internal class ReCaptchaWebserviceResponse
{
    public bool success { get; set; }
    public string challenge_ts { get; set; }
    public string hostname { get; set; }
}