namespace Twitch.Requests;

/// <summary>
/// A Send Announcement request.
/// </summary>
public sealed record SendAnnouncementRequest
{
    /// <summary>
    /// Get the announcement to make in the broadcaster’s chat room.
    /// </summary>
    /// <returns>The announcement to make in the broadcaster’s chat room.</returns>
    [System.Text.Json.Serialization.JsonPropertyName("message")]
    public required string Message { get; init; }
}