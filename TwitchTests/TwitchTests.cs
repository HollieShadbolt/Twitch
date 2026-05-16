using System.Net.Http.Json;
using Moq;

namespace TwitchTests;

[TestFixture]
public static class TwitchTests
{
    #region GetStreamAsync

    [Test]
    public static async Task GetStreamAsync_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var cancellationTokenSource = new CancellationTokenSource();

        var expectedStream = new Twitch.Responses.Stream();

        Twitch.Responses.Stream[] data =
        [
            expectedStream
        ];

        var streams = new Twitch.Responses.Streams()
        {
            Data = data
        };

        mockHttpRequestMessageFactoryHandler
            .Setup(httpRequestMessageFactoryHandler =>
                httpRequestMessageFactoryHandler.SendAsync<Twitch.Responses.Streams>(
                    It.IsAny<Func<HttpRequestMessage>>(),
                    cancellationTokenSource.Token))
            .ReturnsAsync(streams);

        var parameter = Guid.NewGuid().ToString();

        var clientId = Guid.NewGuid().ToString();

        var broadcasterId = Guid.NewGuid().ToString();

        var config = new Twitch.Config
        {
            Parameter = parameter,
            ClientId = clientId,
            BroadcasterId = broadcasterId
        };

        var twitch = new Twitch.Twitch(mockHttpRequestMessageFactoryHandler.Object, config);

        // Act
        var actualStream = await twitch.GetStreamAsync(cancellationTokenSource.Token);

        // Assert
        Assert.That(actualStream, Is.EqualTo(expectedStream));

        mockHttpRequestMessageFactoryHandler
            .Verify(
                httpRequestMessageFactoryHandler =>
                    httpRequestMessageFactoryHandler.SendAsync<Twitch.Responses.Streams>(
                        It.Is<Func<HttpRequestMessage>>(httpRequestMessageFactory =>
                            VerifyGetStreamAsyncHttpRequestMessageFactory(httpRequestMessageFactory, broadcasterId,
                                parameter, clientId)),
                        cancellationTokenSource.Token),
                Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifyGetStreamAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        string broadcasterId,
        string parameter,
        string clientId)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        var keyValuePair = httpRequestMessage.Headers.Single(keyValuePair => keyValuePair.Key == "Client-Id");

        return httpRequestMessage.Method == HttpMethod.Get &&
               keyValuePair.Value.SingleOrDefault() == clientId &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bearer" &&
               httpRequestMessage.Headers.Authorization.Parameter == parameter &&
               httpRequestMessage.RequestUri?.ToString() == $"https://api.twitch.tv/helix/streams?user_id=" +
               $"{broadcasterId}";
    }

    #endregion

    #region SendAnnouncementAsync

    [Test]
    public static async Task SendAnnouncementAsync_TestAsync()
    {
        // Arrange
        var mockHttpRequestMessageFactoryHandler =
            new Mock<HttpRequestMessageHandler.Interfaces.IHttpRequestMessageFactoryHandler>();

        var message = Guid.NewGuid().ToString();

        var parameter = Guid.NewGuid().ToString();

        var clientId = Guid.NewGuid().ToString();

        var broadcasterId = Guid.NewGuid().ToString();

        var config = new Twitch.Config
        {
            Parameter = parameter,
            ClientId = clientId,
            BroadcasterId = broadcasterId
        };

        var twitch = new Twitch.Twitch(mockHttpRequestMessageFactoryHandler.Object, config);

        var moderatorId = Guid.NewGuid().ToString();

        var sendAnnouncementAsyncParams = new Twitch.Params.SendAnnouncementAsyncParams
        {
            ModeratorId = moderatorId,
            Message = message
        };

        var cancellationTokenSource = new CancellationTokenSource();

        // Act
        await twitch.SendAnnouncementAsync(sendAnnouncementAsyncParams, cancellationTokenSource.Token);

        // Assert
        mockHttpRequestMessageFactoryHandler
            .Verify(
                httpRequestMessageFactoryHandler => httpRequestMessageFactoryHandler.SendAsync(
                    It.Is<Func<HttpRequestMessage>>(httpRequestMessageFactory =>
                        VerifySendAnnouncementAsyncHttpRequestMessageFactory(httpRequestMessageFactory,
                            sendAnnouncementAsyncParams, broadcasterId, parameter, clientId)),
                    cancellationTokenSource.Token),
                Times.Exactly(1));

        mockHttpRequestMessageFactoryHandler.VerifyNoOtherCalls();
    }

    private static bool VerifySendAnnouncementAsyncHttpRequestMessageFactory(
        Func<HttpRequestMessage> httpRequestMessageFactory,
        Twitch.Params.SendAnnouncementAsyncParams sendAnnouncementAsyncParams,
        string broadcasterId,
        string parameter,
        string clientId)
    {
        var httpRequestMessage = httpRequestMessageFactory();

        var keyValuePair = httpRequestMessage.Headers.Single(keyValuePair => keyValuePair.Key == "Client-Id");

        var task = httpRequestMessage.Content?.ReadFromJsonAsync<Twitch.Requests.SendAnnouncementRequest>() ??
            throw new InvalidOperationException();

        return httpRequestMessage.Method == HttpMethod.Post &&
               keyValuePair.Value.SingleOrDefault() == clientId &&
               httpRequestMessage.Headers.Authorization?.Scheme == "Bearer" &&
               httpRequestMessage.Headers.Authorization?.Parameter == parameter &&
               httpRequestMessage.RequestUri?.ToString() ==
               $"https://api.twitch.tv/helix/chat/announcements?broadcaster_id={broadcasterId}&moderator_id=" +
               $"{sendAnnouncementAsyncParams.ModeratorId}" &&
               task.Result?.Message == sendAnnouncementAsyncParams.Message;
    }

    #endregion
}