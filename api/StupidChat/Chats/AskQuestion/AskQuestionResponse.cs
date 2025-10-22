public readonly record struct AskQuestionResponse
{
    public long ChatId { get; init; }
    public Message Message { get; init; }
}
