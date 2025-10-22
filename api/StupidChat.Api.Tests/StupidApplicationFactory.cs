using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace StupidChat.Api.Tests;

public class StupidApplicationFactory : WebApplicationFactory<StupidChat.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //var sp = services.BuildServiceProvider();
            //using var scope = sp.CreateScope();
        });
    }
}