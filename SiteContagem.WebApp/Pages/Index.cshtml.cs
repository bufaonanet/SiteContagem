using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SiteContagem.WebApp.Logging;
using StackExchange.Redis;

namespace SiteContagem.WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IConfiguration _configuration;
    private readonly ConnectionMultiplexer _connectionRedis;
    private readonly TelemetryConfiguration _telemetryConfig;

    public IndexModel(
        ILogger<IndexModel> logger,
        IConfiguration configuration,
        ConnectionMultiplexer connectionRedis,
        TelemetryConfiguration telemetryConfig
        )
    {
        _logger = logger;
        _configuration = configuration;
        _connectionRedis = connectionRedis;
        _telemetryConfig = telemetryConfig;
    }

    public void OnGet()
    {
        DateTimeOffset inicio = DateTime.Now;

        var watch = new Stopwatch();
        watch.Start();

        var valorAtual = (int)_connectionRedis.GetDatabase()
            .StringIncrement("APIContagem");
        ;

        _logger.LogValorAtual(valorAtual);

        watch.Stop();

        TelemetryClient client = new(_telemetryConfig);
        client.TrackDependency
        (
            "Redis",
            "Get",
            $"valor = {valorAtual}",
            inicio,
            watch.Elapsed, 
            true
        );

        if (string.IsNullOrWhiteSpace(HttpContext.Session.GetString("UserSession")))
            HttpContext.Session.SetString("UserSession", Guid.NewGuid().ToString());

        TempData["Contador"] = valorAtual;
        TempData["UserSession"] = HttpContext.Session.GetString("UserSession");
        TempData["SessionType"] = _configuration["Session:Type"];
        TempData["Local"] = ApplicationStatus.Local;
        TempData["Kernel"] = ApplicationStatus.Kernel;
        TempData["Framework"] = ApplicationStatus.Framework;
        TempData["Saudavel"] = ApplicationStatus.Healthy ? "Sim" : "Não";
        TempData["MensagemFixa"] = "Teste";
        TempData["MensagemVariavel"] = _configuration["MensagemVariavel"];
    }
}