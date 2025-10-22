using Microsoft.AspNetCore.Builder;

public static class GetChatExtension
{
    public static WebApplication AddGetChat(this WebApplication app)
    {
        app.MapGet("/api/chats/{id}",
            (IChatRepository repository, int id) => repository.GetByIdAsync(id));

        return app;
    }
}