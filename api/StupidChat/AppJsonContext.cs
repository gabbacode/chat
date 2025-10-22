using System.Text.Json.Serialization;

[JsonSourceGenerationOptions(
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
[JsonSerializable(typeof(AnswerQuestionResponse))]
[JsonSerializable(typeof(AnswerQuestionRequest))]
[JsonSerializable(typeof(AskQuestionRequest))]
[JsonSerializable(typeof(AskQuestionResponse))]
[JsonSerializable(typeof(LoadAnswersResponse))]
[JsonSerializable(typeof(Chat))]
[JsonSerializable(typeof(CreateChatRequest))]

public partial class AppJsonContext : JsonSerializerContext
{
}