using SendAnnouncementAsyncParams = Twitch.Params.SendAnnouncementAsyncParams;

namespace Twitch.Interfaces;

/// <summary>
/// A Twitch bot instance.
/// </summary>
public interface ITwitch
{
    /// <summary>
    /// Get the stream if available.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task<Responses.Stream?> GetStreamAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Sends an announcement to the broadcaster’s chat room.
    /// </summary>
    /// <param name="sendAnnouncementAsyncParams">The <see cref="SendAnnouncementAsyncParams"/></param>
    /// <param name="cancellationToken">The cancellation token to cancel operation.</param>
    /// <returns>The task object representing the asynchronous operation.</returns>
    Task SendAnnouncementAsync(
        SendAnnouncementAsyncParams sendAnnouncementAsyncParams,
        CancellationToken cancellationToken);
}