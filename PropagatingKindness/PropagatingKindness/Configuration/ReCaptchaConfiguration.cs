namespace PropagatingKindness.Configuration;

public class ReCaptchaConfiguration
{
    internal const string ConfigSection = "reCAPTCHA";

    public string SiteKey { get; set; }
    public string SecretKey { get; set; }
}
