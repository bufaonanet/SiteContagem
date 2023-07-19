using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SiteContagem.WebApp.Pages;

public class ForceError : PageModel
{
    private readonly ILogger<ForceError> _logger;

    public ForceError(ILogger<ForceError> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        ApplicationStatus.Healthy = false;
        _logger.LogWarning("Status da aplicação configurado para simulação de erro do tipo 500!");
    }
}