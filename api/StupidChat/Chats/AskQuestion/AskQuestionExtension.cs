using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public static class AskQuestionExtension
{
    public static WebApplication AddAskQuestion(this WebApplication app)
    {
        app.MapPost("/api/chats/{id}/ask",
            async (IChatRepository repository, long id, [FromBody] AskQuestionRequest question) =>
            {
                var newMessage = await repository.AddQuestionAsync(
                    id,
                    new Message
                    {
                        Author = question.Author,
                        Text = question.Text
                    });

                //if (chat == null)
                //{
                //    return NotFound("Chat not found");
                //}

                return new AskQuestionResponse
                {
                    ChatId = id,
                    Message = newMessage
                };
            });

        return app;
    }
}