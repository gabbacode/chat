public readonly record struct AnswerQuestionResponse
{
    public long ChatId { get; init; }
    public long QuestionId { get; init; }
    public ReplyMessage Answer { get; init; }
}