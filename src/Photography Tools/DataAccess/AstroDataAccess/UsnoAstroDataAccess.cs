using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Photography_Tools.DataAccess.AstroDataAccess;

public class UsnoAstroDataAccess : IAstroDataAccess
{
    private static readonly TimeSpan MinRequestDiffTime = TimeSpan.FromSeconds(0.5), MinFailRequestDiffTime = TimeSpan.FromSeconds(20);
    private static readonly DateTime MinDateTime = new(1700, 1, 1), MaxDateTime = new(2099, 12, 31);

    private readonly HttpClient httpClient;

    private long lastApiRequest = 0, lastFailRequest = 0;

    public UsnoAstroDataAccess(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<ServiceResponse<AstroData?>> GetAstroDataAsync(DateTime date, double latitude, double longitude)
    {
        if (!(Connectivity.Current.NetworkAccess == NetworkAccess.Internet))
            return IAstroDataAccess.FailResultResponse with { Message = "No Internet access", Code = -2 };

        if (Stopwatch.GetElapsedTime(lastApiRequest) < MinRequestDiffTime)
            return IAstroDataAccess.FailResultResponse with { Message = "Too quick api call", Code = -3 };

        if (Stopwatch.GetElapsedTime(lastFailRequest) < MinFailRequestDiffTime)
            return IAstroDataAccess.FailResultResponse with { Message = "Source unavailable", Code = -4 };

        if (date < MinDateTime || date > MaxDateTime)
            return IAstroDataAccess.IncorrectInputResponse with { Code = -5 };

        DateTime universalDate = date.ToUniversalTime();
        lastApiRequest = Stopwatch.GetTimestamp();

        try
        {
#if DEBUG
            Debug.WriteLine($"USNO API CALL: rstt/oneday?date={universalDate:yyyy-MM-dd}&time={universalDate:HH:mm}&coords={latitude.ToString("0.000", CultureInfo.InvariantCulture)},{longitude.ToString("0.00", CultureInfo.InvariantCulture)}");
#endif
            using HttpResponseMessage response = await httpClient.GetAsync($"rstt/oneday?date={universalDate:yyyy-MM-dd}&time={universalDate:HH:mm}&coords={latitude.ToString("0.000", CultureInfo.InvariantCulture)},{longitude.ToString("0.00", CultureInfo.InvariantCulture)}");

            if (!response.IsSuccessStatusCode)
            {
                lastFailRequest = Stopwatch.GetTimestamp();
                return IAstroDataAccess.FailResultResponse with { Code = (int)response.StatusCode };
            }

            using Stream responseStream = await response.Content.ReadAsStreamAsync();
            UsnoSoonMoonResponse? result = await JsonSerializer.DeserializeAsync<UsnoSoonMoonResponse>(responseStream);

            if (result is null)
                return IAstroDataAccess.IncorrectInputResponse;

            double moonAge = await GetMoonAgeAsync(date);

            return new ServiceResponse<AstroData?>(ConvertUsnoResponseToAstroData(result, moonAge), true, 1);
        }
        catch (Exception)
        {
            lastFailRequest = Stopwatch.GetTimestamp();
            throw;
        }
    }

    private async Task<double> GetMoonAgeAsync(DateTime universalDate)
    {
#if DEBUG
        Debug.WriteLine($"USNO API CALL: moon/phases/date?date={universalDate:yyyy-MM-dd}&nump=4");
#endif
        using HttpResponseMessage response = await httpClient.GetAsync($"moon/phases/date?date={universalDate:yyyy-MM-dd}&nump=4");

        if (!response.IsSuccessStatusCode)
            return double.NaN;

        using Stream stream = await response.Content.ReadAsStreamAsync();
        UsnoPhaseOfTheMoonResponse? result = await JsonSerializer.DeserializeAsync<UsnoPhaseOfTheMoonResponse>(stream);

        if (result is null)
            return double.NaN;

        foreach (var item in result.PhaseData)
        {
            if (item.Name.Equals("New Moon"))
                return AstroConst.SynodicMonthLength - (item.Time - universalDate).TotalDays;
        }

        return double.NaN;
    }

    public static AstroData ConvertUsnoResponseToAstroData(UsnoSoonMoonResponse response, double moonAge)
    {
        TimeSpan rise, civilStart, civilEnd, transit, set;
        rise = civilEnd = civilStart = transit = set = TimeSpan.Zero;

        foreach (ShortPhaseData item in response.Properties.Data.SunData)
        {
            switch (item.Phen)
            {
                case "Set":
                    set = item.Time;
                    break;
                case "End Civil Twilight":
                    civilEnd = item.Time;
                    break;
                case "Begin Civil Twilight":
                    civilStart = item.Time;
                    break;
                case "Rise":
                    rise = item.Time;
                    break;
                case "Upper Transit":
                    transit = item.Time;
                    break;
                default:
                    break;
            }
        }

        SunData sundData = new(rise, civilStart, civilEnd, transit, set);

        rise = transit = set = TimeSpan.Zero;

        foreach (ShortPhaseData item in response.Properties.Data.MoonData)
        {
            switch (item.Phen)
            {
                case "Set":
                    set = item.Time;
                    break;
                case "Rise":
                    rise = item.Time;
                    break;
                case "Upper Transit":
                    transit = item.Time;
                    break;
                default:
                    break;
            }
        }

        MoonData moonData = new(rise, transit, set, response.Properties.Data.Fracillum, moonAge, response.Properties.Data.CurrentPhase);

        return new(sundData, moonData);
    }


    public class UsnoPhaseOfTheMoonResponse
    {
        [JsonPropertyName("phasedata")]
        public required PhaseData[] PhaseData { get; set; }
    }

    public class PhaseData
    {
        [JsonPropertyName("day")]
        public int Day { get; set; }
        [JsonPropertyName("month")]
        public int Month { get; set; }
        [JsonPropertyName("year")]
        public int Year { get; set; }
        [JsonPropertyName("phase")]
        public required string Name { get; set; }
        [JsonPropertyName("time")]
        public required string TimeString { get; set; }

        [JsonIgnore]
        public DateTime Time { get => new(Year, Month, Day, int.Parse(TimeString.AsSpan().Trim()[..2]), int.Parse(TimeString.AsSpan().Trim()[^2..]), 0); }
    }

    public class UsnoSoonMoonResponse
    {
        [JsonPropertyName("properties")]
        public required Properties Properties { get; set; }
    }

    public class Properties
    {
        [JsonPropertyName("data")]
        public required Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("closestphase")]
        public required ClosestPhase ClosestPhase { get; set; }
        [JsonPropertyName("moondata")]
        public required ShortPhaseData[] MoonData { get; set; }
        [JsonPropertyName("sundata")]
        public required ShortPhaseData[] SunData { get; set; }
        [JsonPropertyName("fracillum")]
        public required string FracillumString { get; set; }
        [JsonPropertyName("curphase")]
        public required string CurrentPhase { get; set; }

        [JsonIgnore]
        public double Fracillum { get => double.Parse(FracillumString.AsSpan()[..^1]); }
    }

    public class ShortPhaseData
    {
        [JsonPropertyName("phen")]
        public required string Phen { get; set; }
        [JsonPropertyName("time")]
        public required string TimeString { get; set; }

        [JsonIgnore]
        public TimeSpan Time => TimeSpan.ParseExact(TimeString, "g", CultureInfo.InvariantCulture);
    }

    public class ClosestPhase
    {
        [JsonPropertyName("phase")]
        public required string Phase { get; set; }
    }
}