namespace TailsAndClaws.Common.Options;

public class PingEndpointOptions
{
    public const string SectionName = "PingEndpoint";
    public const string DefaultMessage = "Dogshouseservice.Version1.0.1";

    public string ReturnMessage { get; set; } = DefaultMessage;
}
