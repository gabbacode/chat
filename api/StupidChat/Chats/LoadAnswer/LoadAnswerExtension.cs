using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public static class LoadAnswerExtension
{
    public static WebApplication AddLoadAnswer(this WebApplication app)
    {
        app.MapGet("/api/chats/{id}/answer/{questionId}",
            async (IChatRepository repository, long id, long questionId) =>
            {
                var answers = await repository.GetAnswerAsync(id, questionId);

                return new LoadAnswersResponse
                {
                    ChatId = id,
                    QuestionId = questionId,
                    Answers = answers
                };
            });

        return app;
    }
}