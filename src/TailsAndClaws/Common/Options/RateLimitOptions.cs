namespace TailsAndClaws.Common.Options;

public class RateLimitOptions
{
    public const string SectionName = "RateLimiting";

    public int WindowLengthInSeconds { get; set; } = 1;

    public int RequestsPerWindowAmount { get; set; } = 10;
}
