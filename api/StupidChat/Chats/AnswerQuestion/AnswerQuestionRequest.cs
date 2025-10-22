public readonly record struct AnswerQuestionRequest
{
    public long QuestionId { get; init; }
    public string Author { get; init; }
    public string Text { get; init; }
}
