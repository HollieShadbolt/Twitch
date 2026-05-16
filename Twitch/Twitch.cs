using HttpUtility = System.Web.HttpUtility;
using SendAnnouncementAsyncParams = Twitch.Params.SendAnnouncementAsyncParams;

namespace Twitch;

public sealed class Twitch(
    HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler httpRequestMessageFactoryHandler,
    Config config)
    : HttpRequestMessageHandler.HttpRequestMessageFactories(Scheme, config.Parameter, Uri), Interfaces.ITwitch
{
    private const string Scheme = "Bearer";
    private const string Uri = "https://api.twitch.tv/helix";

    public async Task<Responses.Stream?> GetStreamAsync(CancellationToken cancellationToken)
    {
        var httpRequestMessageFactory = GetGetStreamsHttpRequestMessage;

        var streams =
            await httpRequestMessageFactoryHandler.SendAsync<Responses.Streams>(
                httpRequestMessageFactory,
                cancellationToken);

        return streams.Data.SingleOrDefault();
    }

    public async Task SendAnnouncementAsync(
        SendAnnouncementAsyncParams sendAnnouncementAsyncParams,
        CancellationToken cancellationToken)
    {
        var httpRequestMessageFactory = GetSendAnnouncementHttpRequestMessageFactory(sendAnnouncementAsyncParams);

        await httpRequestMessageFactoryHandler.SendAsync(httpRequestMessageFactory, cancellationToken);
    }

    private HttpRequestMessage GetGetStreamsHttpRequestMessage()
    {
        const string path = "streams";

        var uriBuilder = UriBuilderFactory.GetUriBuilder(path);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["user_id"] = config.BroadcasterId;

        uriBuilder.Query = query.ToString();

        var httpRequestMessage = HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder, HttpMethod.Get);

        return GetHttpRequestMessageWithClientId(httpRequestMessage);
    }

    private Func<HttpRequestMessage> GetSendAnnouncementHttpRequestMessageFactory(
        SendAnnouncementAsyncParams sendAnnouncementAsyncParams) => () =>
    {
        string[] paths =
        [
            "chat",
            "announcements"
        ];

        var uriBuilder = UriBuilderFactory.GetUriBuilder(paths);

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["broadcaster_id"] = config.BroadcasterId;
        query["moderator_id"] = sendAnnouncementAsyncParams.ModeratorId;

        uriBuilder.Query = query.ToString();

        var inputValue = new Requests.SendAnnouncementRequest
        {
            Message = sendAnnouncementAsyncParams.Message
        };

        var httpRequestMessage =
            HttpRequestMessageFactory.GetHttpRequestMessage(uriBuilder, HttpMethod.Post, inputValue);

        return GetHttpRequestMessageWithClientId(httpRequestMessage);
    };

    private HttpRequestMessage GetHttpRequestMessageWithClientId(HttpRequestMessage httpRequestMessage)
    {
        const string name = "Client-Id";

        httpRequestMessage.Headers.Add(name, config.ClientId);

        return httpRequestMessage;
    }
}