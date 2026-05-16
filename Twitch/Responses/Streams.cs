namespace Twitch.Responses;

/// <summary>
/// A list of streams.
/// </summary>
public sealed record Streams
{
    /// <summary>
    /// Get the list of streams.
    /// </summary>
    /// <returns>The list of streams.</returns>
    [System.Text.Json.Serialization.JsonPropertyName("data")]
    public required IEnumerable<Stream> Data { get; init; }
}