using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;

public static class CreateChatExtension
{
    public static WebApplication AddCreateChat(this WebApplication app)
    {
        app.MapPost("/api/chats/create",
            (IChatRepository repository, [FromBody] CreateChatRequest createChatRequest) => 
            { 
                return repository.Create(createChatRequest.User); 
            });

        app.MapPost("/api/chats/import",
            (IChatRepository repository, [FromBody] Chat chat) => repository.AddAsync(chat));

        return app;
    }
}