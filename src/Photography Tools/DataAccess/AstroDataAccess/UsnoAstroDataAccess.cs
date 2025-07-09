using Photography_Tools.Services.ConfigService;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Photography_Tools.DataAccess.AstroDataAccess;

public class UsnoAstroDataAccess : IAstroDataAccess
{
    private static readonly TimeSpan MinRequestDiffTime = TimeSpan.FromMilliseconds(500), MinFailRequestDiffTime = TimeSpan.FromSeconds(20);
    private static readonly DateTime MinDateTime = new(1700, 1, 1), MaxDateTime = new(2099, 12, 31);

    private readonly IHttpClientFactory httpClientFactory;
    private readonly ISettingsService settingsService;

    private long lastApiRequest = 0, lastFailRequest = 0;

    public string DataSourceInfo { get; } = "U.S. Naval Observatory";

    public UsnoAstroDataAccess(IHttpClientFactory httpClientFactory, ISettingsService settingsService)
    {
        this.httpClientFactory = httpClientFactory;
        this.settingsService = settingsService;
    }

    public async Task<ServiceResponse<AstroData?>> GetAstroDataAsync(DateTime dateTimeUtc, double latitude, double longitude)
    {
        if (!(Connectivity.Current.NetworkAccess == NetworkAccess.Internet) || settingsService.GetSettings().IsUseOnlyOfflineDataSourceModeEnabled)
            return IAstroDataAccess.FailResultResponse with { Message = "No Internet access", Code = -2 };

        if (Stopwatch.GetElapsedTime(lastApiRequest) < MinRequestDiffTime)
            return IAstroDataAccess.FailResultResponse with { Message = "Too quick api call", Code = -3 };

        if (Stopwatch.GetElapsedTime(lastFailRequest) < MinFailRequestDiffTime)
            return IAstroDataAccess.FailResultResponse with { Message = "Source unavailable", Code = -4 };

        if (dateTimeUtc < MinDateTime || dateTimeUtc > MaxDateTime)
            return IAstroDataAccess.IncorrectInputResponse with { Code = -5 };

        lastApiRequest = Stopwatch.GetTimestamp();

        try
        {
#if DEBUG
            Debug.WriteLine($"USNO API CALL: rstt/oneday?date={dateTimeUtc:yyyy-MM-dd}&coords={latitude.ToString("0.000", CultureInfo.InvariantCulture)},{longitude.ToString("0.00", CultureInfo.InvariantCulture)}");
#endif
            HttpClient httpClient = httpClientFactory.CreateClient(HttpClientConst.UsnoHttpClientName);
            using HttpResponseMessage response = await httpClient.GetAsync($"rstt/oneday?date={dateTimeUtc:yyyy-MM-dd}&coords={latitude.ToString("0.000", CultureInfo.InvariantCulture)},{longitude.ToString("0.00", CultureInfo.InvariantCulture)}");

            if (!response.IsSuccessStatusCode)
            {
                lastFailRequest = Stopwatch.GetTimestamp();
                return IAstroDataAccess.FailResultResponse with { Code = (int)response.StatusCode };
            }

            using Stream responseStream = await response.Content.ReadAsStreamAsync();
            UsnoSoonMoonResponse? result = await JsonSerializer.DeserializeAsync<UsnoSoonMoonResponse>(responseStream);

            if (result is null)
                return IAstroDataAccess.IncorrectInputResponse;

            double moonAge = GetMoonAge(dateTimeUtc, result.Properties.Data.ClosestPhase);

            return new ServiceResponse<AstroData?>(ConvertUsnoResponseToAstroData(result, moonAge, dateTimeUtc), true, 1);
        }
        catch (Exception)
        {
            lastFailRequest = Stopwatch.GetTimestamp();
            throw;
        }
    }

    public static double GetMoonAge(DateTime selectedDateUTC, PhaseData closestPhase)
    {
        double diff = (closestPhase.DateTime - selectedDateUTC).Duration().TotalDays * (selectedDateUTC < closestPhase.DateTime ? -1 : 1);
        double result = closestPhase.Name switch
        {
            "New Moon" => AstroConst.SynodicMonthLength + diff,
            "Last Quarter" or AstroConst.ThirdQuarter => AstroConst.SynodicMonthLength * 0.75 + diff,
            "Full Moon" => AstroConst.SynodicMonthLength * 0.5 + diff,
            "First Quarter" => AstroConst.SynodicMonthLength * 0.25 + diff,
            _ => double.NaN
        };

        if (result > AstroConst.SynodicMonthLength)
            result -= AstroConst.SynodicMonthLength;

        return result;
    }

    public static AstroData ConvertUsnoResponseToAstroData(UsnoSoonMoonResponse response, double moonAge, DateTime selectedDateTimeUTC)
    {
        DateTime? rise, civilStart, civilEnd, transit, set;
        rise = civilEnd = civilStart = transit = set = null;

        foreach (ShortPhaseData item in response.Properties.Data.SunData)
        {
            switch (item.Phen)
            {
                case "Set":
                    set = response.Properties.Data.Date.Add(item.Time);
                    break;
                case "End Civil Twilight":
                    civilEnd = response.Properties.Data.Date.Add(item.Time);
                    break;
                case "Begin Civil Twilight":
                    civilStart = response.Properties.Data.Date.Add(item.Time);
                    break;
                case "Rise":
                    rise = response.Properties.Data.Date.Add(item.Time);
                    break;
                case "Upper Transit":
                    transit = response.Properties.Data.Date.Add(item.Time);
                    break;
                default:
                    break;
            }
        }

        SunData sundData = new(rise, civilStart, civilEnd, transit, set);

        rise = transit = set = null;

        foreach (ShortPhaseData item in response.Properties.Data.MoonData)
        {
            switch (item.Phen)
            {
                case "Set":
                    set = response.Properties.Data.Date.Add(item.Time);
                    break;
                case "Rise":
                    rise = response.Properties.Data.Date.Add(item.Time);
                    break;
                case "Upper Transit":
                    transit = response.Properties.Data.Date.Add(item.Time);
                    break;
                default:
                    break;
            }
        }

        string phaseName;
        if ((response.Properties.Data.ClosestPhase.DateTime - selectedDateTimeUTC).Duration().TotalDays < AstroConst.SynodicMonthLength / 60)
            phaseName = response.Properties.Data.ClosestPhase.Name;
        else
            phaseName = response.Properties.Data.CurrentPhase;

        if (phaseName.Equals("Last Quarter"))
            phaseName = AstroConst.ThirdQuarter;

        MoonData moonData = new(rise, transit, set, response.Properties.Data.Fracillum, moonAge, phaseName);

        return new(sundData, moonData);
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
        public DateTime DateTime { get => new(Year, Month, Day, int.Parse(TimeString.AsSpan().Trim()[..2]), int.Parse(TimeString.AsSpan().Trim()[^2..]), 0, DateTimeKind.Utc); }
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
        public required PhaseData ClosestPhase { get; set; }
        [JsonPropertyName("moondata")]
        public required ShortPhaseData[] MoonData { get; set; }
        [JsonPropertyName("sundata")]
        public required ShortPhaseData[] SunData { get; set; }
        [JsonPropertyName("fracillum")]
        public required string FracillumString { get; set; }
        [JsonPropertyName("curphase")]
        public required string CurrentPhase { get; set; }
        [JsonPropertyName("day")]
        public int Day { get; set; }
        [JsonPropertyName("month")]
        public int Month { get; set; }
        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonIgnore]
        public double Fracillum { get => double.Parse(FracillumString.AsSpan()[..^1]); }
        [JsonIgnore]
        public DateTime Date { get => new(Year, Month, Day, 0, 0, 0, DateTimeKind.Utc); }

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
}