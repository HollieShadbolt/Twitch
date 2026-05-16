using JsonPropertyNameAttribute = System.Text.Json.Serialization.JsonPropertyNameAttribute;

namespace Twitch;

/// <summary>
/// A <see cref="Twitch"/> config.
/// </summary>
public class Config
{
    /// <summary>
    /// Get the credentials containing the authentication information of the user agent.
    /// </summary>
    /// <returns>The credentials containing the authentication information of the user agent.</returns>
    [JsonPropertyName("parameter")]
    public required string Parameter { get; init; }

    /// <summary>
    /// Get the client ID.
    /// </summary>
    /// <returns>The client ID.</returns>
    [JsonPropertyName("client_id")]
    public required string ClientId { get; init; }

    /// <summary>
    /// Get the broadcaster ID.
    /// </summary>
    /// <returns>The broadcaster ID.</returns>
    [JsonPropertyName("broadcaster_id")]
    public required string BroadcasterId { get; init; }
}