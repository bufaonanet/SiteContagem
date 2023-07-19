using SiteContagem.WebApp.HealthChecks;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHealthChecks();

var appInsightsKey = builder.Configuration.GetConnectionString("ApplicationInsights");
if (!string.IsNullOrWhiteSpace(appInsightsKey))
{
    builder.Services.AddApplicationInsightsTelemetry(options =>
    {
        options.ConnectionString = appInsightsKey;
    });
}

var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
builder.Services.AddSingleton(ConnectionMultiplexer.Connect(redisConnectionString));

if (builder.Configuration["Session:Type"]?.ToUpper() == "REDIS")
    builder.Services.AddStackExchangeRedisCache(redisCacheConfig =>
    {
        redisCacheConfig.Configuration = redisConnectionString;
        redisCacheConfig.InstanceName = "bufa-redis";
    });
else
    builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(
        Convert.ToInt32(builder.Configuration["Session:TimeoutInSeconds"]));
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHealthChecks("/status", HealthChecksExtensions.GetJsonReturn());

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.Run();