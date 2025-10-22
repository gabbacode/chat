using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolver = AppJsonContext.Default;
    options.SerializerOptions.Encoder = null;
});
builder.Services.AddSingleton<IChatRepository, MemoryChatRepository>();

//builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {  // только для dev
            policy.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});
//builder.Logging.ClearProviders();
builder.WebHost.ConfigureKestrel(options =>
{
    options.AllowSynchronousIO = true;
});

var app = builder.Build();

app.UseOpenApi().UseSwaggerUi();
app.UseHttpsRedirection();
//app.MapControllers();

app
    .AddCreateChat()
    .AddGetChat()
    .AddAnswerQuestion()
    .AddAskQuestion()
    .AddLoadAnswer();

app.UseCors();
app.Run();