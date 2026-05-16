namespace Twitch.Params;

/// <summary>
/// Params for <see cref="Interfaces.ITwitch.SendAnnouncementAsync"/>.
/// </summary>
public sealed record SendAnnouncementAsyncParams
{
    /// <summary>
    /// Get the ID of a user who has permission to moderate the broadcaster’s chat room.
    /// </summary>
    /// <returns>The ID of a user who has permission to moderate the broadcaster’s chat room.</returns>
    public required string ModeratorId { get; init; }

    /// <summary>
    /// Get the announcement to make in the broadcaster’s chat room.
    /// </summary>
    /// <returns>The announcement to make in the broadcaster’s chat room.</returns>
    public required string Message { get; init; }
}