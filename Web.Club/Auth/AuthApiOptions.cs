namespace Web.Club.Auth;

public sealed class AuthApiOptions
{
    public const string SectionName = "AuthApi";

    public string BaseUrl { get; init; } = "https://localhost:7255";
}
