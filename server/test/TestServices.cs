using database;
using database.Triggers;
using lib.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace test;

public static class TestServices
{
    public static IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();

        services.AddMemoryCache();
        services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase("local-test-db");
            options.UseTriggers(o => o.AddTrigger<EntityBaseTrigger<Guid>>());
            options.UseTriggers(o => o.AddTrigger<EntityBaseTrigger<int>>());
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        services.AddTransient<ContactService>();

        return services.BuildServiceProvider();
    }
}