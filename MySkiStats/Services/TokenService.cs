namespace MySkiStats.Services;

using System.Text.Json.Serialization;

public class TokenService
{
    private const string AccessTokenKey = "strava_access_token";
    private const string RefreshTokenKey = "strava_refresh_token";
    private const string ExpiresAtKey = "strava_expires_at";

    private readonly Blazored.LocalStorage.ILocalStorageService _localStorage;

    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public long ExpiresAt { get; set; }

    public TokenService(Blazored.LocalStorage.ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task InitializeAsync()
    {
        AccessToken = await _localStorage.GetItemAsStringAsync(AccessTokenKey);
        RefreshToken = await _localStorage.GetItemAsStringAsync(RefreshTokenKey);
        ExpiresAt = await _localStorage.GetItemAsync<long>(ExpiresAtKey);
    }

    public async Task SaveTokenAsync(TokenResponse token)
    {
        AccessToken = token.AccessToken;
        RefreshToken = token.RefreshToken;
        ExpiresAt = token.ExpiresAt;

        await _localStorage.SetItemAsStringAsync(AccessTokenKey, token.AccessToken);
        await _localStorage.SetItemAsStringAsync(RefreshTokenKey, token.RefreshToken);
        await _localStorage.SetItemAsync(ExpiresAtKey, token.ExpiresAt);
    }

    public async Task ClearTokenAsync()
    {
        AccessToken = null;
        RefreshToken = null;
        ExpiresAt = 0;

        await _localStorage.RemoveItemAsync(AccessTokenKey);
        await _localStorage.RemoveItemAsync(RefreshTokenKey);
        await _localStorage.RemoveItemAsync(ExpiresAtKey);
    }

    public bool IsTokenExpired()
    {
        if (ExpiresAt == 0)
            return true;

        return DateTimeOffset.UtcNow.ToUnixTimeSeconds() >= ExpiresAt;
    }

    public class TokenResponse
    {
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = default!;

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default!;

        [JsonPropertyName("expires_at")]
        public long ExpiresAt { get; set; }

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = default!;
    }
}
