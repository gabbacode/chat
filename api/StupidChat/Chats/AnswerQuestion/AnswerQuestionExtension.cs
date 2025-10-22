using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

public static class AnswerQuestionExtension
{
    public static WebApplication AddAnswerQuestion(this WebApplication app)
    {
        app.MapPost("/api/chats/{id}/answer",
            async (IChatRepository repository, long id, [FromBody] AnswerQuestionRequest answerQuestionRequest) =>
            {
                var answer = await repository.AddAnswerAsync(
                    id,
                    answerQuestionRequest.QuestionId,
                    new ReplyMessage
                    {
                        Author = answerQuestionRequest.Author,
                        Text = answerQuestionRequest.Text
                    });

                return new AnswerQuestionResponse
                {
                    ChatId = id,
                    QuestionId = answerQuestionRequest.QuestionId,
                    Answer = answer
                };
            });

        return app;
    }
}