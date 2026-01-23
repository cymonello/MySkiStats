using System.Text.Json.Serialization;

namespace MySkiStats.Api.Services;

public class StravaService
{
    private readonly HttpClient _httpClient;
    private const string StravaApiBaseUrl = "https://www.strava.com/api/v3";

    public StravaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<StravaAthlete?> GetAthleteAsync(string accessToken)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{StravaApiBaseUrl}/athlete");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<StravaAthlete>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<List<StravaActivity>?> GetActivitiesAsync(string accessToken, int page = 1, int perPage = 200)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{StravaApiBaseUrl}/athlete/activities?page={page}&per_page={perPage}");
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
            return null;

        var json = await response.Content.ReadAsStringAsync();
        return System.Text.Json.JsonSerializer.Deserialize<List<StravaActivity>>(json, new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public class StravaAthlete
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("firstname")]
        public string FirstName { get; set; } = default!;

        [JsonPropertyName("lastname")]
        public string LastName { get; set; } = default!;
    }

    public class StravaActivity
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = default!;

        [JsonPropertyName("distance")]
        public float Distance { get; set; }

        [JsonPropertyName("total_elevation_gain")]
        public float Elevation { get; set; }

        [JsonPropertyName("moving_time")]
        public float MovingTime { get; set; }

        [JsonPropertyName("elapsed_time")]
        public float ElapsedTime { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = default!;
    }
}