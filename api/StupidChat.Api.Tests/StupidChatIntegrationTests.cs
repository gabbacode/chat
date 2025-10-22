using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace StupidChat.Api.Tests;

public class StupidChatIntegrationTests : IClassFixture<StupidApplicationFactory>
{
    private readonly HttpClient _client;

    public StupidChatIntegrationTests(StupidApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1000)]
    [InlineData(100000)]
    [InlineData(1000000)]
    public async Task CreateChat(int chatCount)
    {
        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = 16
        };

        await Parallel.ForEachAsync(
            Enumerable.Range(0, chatCount),
            options,
            async (id, cancellationToken) =>
            {
                var createChatRequest = new
                {
                    user = "Test User"
                };

                var response = await _client.PostAsJsonAsync("/api/chats/create", createChatRequest);

                response.StatusCode.Should().Be(HttpStatusCode.OK);

                var chat = await response.Content.ReadFromJsonAsync<Chat>();

                chat.Should().NotBeNull();
                chat.Id.Should().BeInRange(0, long.MaxValue);
            });
    }

    [Fact]
    public async Task ViewChat()
    {
        var createChatRequest = new { user = "Test User" };
        var createChatresponse = await _client.PostAsJsonAsync("/api/chats/create", createChatRequest);
        var chatData1 = await createChatresponse.Content.ReadFromJsonAsync<Chat>();

        chatData1.Should().NotBeNull();

        var getChatResponse = await _client.GetAsync($"/api/chats/{chatData1.Id}");
        getChatResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var chatData2 = await getChatResponse.Content.ReadFromJsonAsync<Chat>();
        chatData2.Should().NotBeNull();
        chatData2.User.Should().BeEquivalentTo(createChatRequest.user);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(1000)]
    [InlineData(100000)]
    public async Task AskAndAnswerQuestion(int questionsCount)
    {
        var createChatRequest = new { user = "Test User" };
        var createChatresponse = await _client.PostAsJsonAsync("/api/chats/create", createChatRequest);
        var chatData = await createChatresponse.Content.ReadFromJsonAsync<Chat>();
        chatData.Should().NotBeNull();

        var questionIds = new List<long>();
        for (int i = 0; i < questionsCount; i++)
        {
            var askRequest = new { Value = $"Question {i}" };
            var askResponse = await _client.PostAsJsonAsync($"/api/chats/{chatData.Id}/ask", askRequest);
            askResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var askQuestionResponse = await askResponse.Content.ReadFromJsonAsync<AskQuestionResponse>();
            questionIds.Add(askQuestionResponse.Message.Id);
        }

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = 8
        };

        await Parallel.ForEachAsync(
            questionIds,
            options,
            async (id, cancellationToken) =>
            {
                var answerRequest = new
                {
                    ChatId = chatData.Id,
                    QuestionId = id,
                    Author = "someone",
                    Text = $"answer {id}"
                };
                var answerResponse = await _client.PostAsJsonAsync($"/api/chats/{chatData.Id}/answer", answerRequest);
                answerResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            });

        await Parallel.ForEachAsync(
            questionIds,
            options,
            async (id, cancellationToken) =>
        {
            var answerLoadResponse = await _client.GetAsync($"/api/chats/{chatData.Id}/answer/{id}");
            answerLoadResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var answer = await answerLoadResponse.Content.ReadFromJsonAsync<LoadAnswersResponse>();
        });

        var getChatResponse = await _client.GetAsync($"/api/chats/{chatData.Id}");
        getChatResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var chat = await getChatResponse.Content.ReadFromJsonAsync<Chat>();

        chat.Should().NotBeNull();
        chat.User.Should().BeEquivalentTo(createChatRequest.user);
        chat.Messages.Count.Should().Be(questionsCount);
    }
}