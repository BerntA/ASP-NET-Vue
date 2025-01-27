using database;
using database.Triggers;
using lib.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();
builder.Services.AddMemoryCache();
builder.Services
    .AddRazorPages()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.AllowReadingFromString | JsonNumberHandling.AllowNamedFloatingPointLiterals;
        options.JsonSerializerOptions.IncludeFields = true;
    });
builder.Services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration["Database"]);
    options.UseTriggers(o => o.AddTrigger<EntityBaseTrigger<Guid>>());
    options.UseTriggers(o => o.AddTrigger<EntityBaseTrigger<int>>());

    var isDevBuild = builder.Environment.IsDevelopment();
    options.EnableSensitiveDataLogging(isDevBuild);
    options.EnableDetailedErrors(isDevBuild);
});

builder.Services.AddTransient<ContactService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.MapStaticAssets();

app.UseAuthorization();

app.MapControllers();

app.Run();