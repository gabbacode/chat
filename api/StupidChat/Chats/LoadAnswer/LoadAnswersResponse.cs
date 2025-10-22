public readonly record struct LoadAnswersResponse
{
    public long ChatId { get; init; }
    public long QuestionId { get; init; }
    public ReplyMessage[] Answers { get; init; }
}