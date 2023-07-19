using System.Runtime.InteropServices;

namespace SiteContagem.WebApp;

public class ApplicationStatus
{
    public static string Local { get; } = Environment.MachineName;
    public static string Kernel { get; } = Environment.OSVersion.VersionString;
    public static string Framework { get; } = RuntimeInformation.FrameworkDescription;
    public static bool Healthy { get; set; } = true;

    /*static ApplicationStatus()
    {
        //Local = Environment.MachineName;
        Kernel = Environment.OSVersion.VersionString;
        Framework = RuntimeInformation.FrameworkDescription;
    }*/
}