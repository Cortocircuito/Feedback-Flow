using Feedback_Flow.Services;
using Feedback_Flow.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Feedback_Flow;

internal static class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Setup Dependency Injection
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        // Resolve Form1
        var mainForm = ServiceProvider.GetRequiredService<Form1>();
        Application.Run(mainForm);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Register Services
        services.AddSingleton<IFileSystemService, FileSystemService>();
        services.AddSingleton<IDataService, CsvDataService>();
        services.AddSingleton<IPdfService, PdfGenerationService>();
        services.AddSingleton<IEmailService, OutlookEmailService>();
        services.AddSingleton<IStudentService, StudentService>();

        // Register Forms
        services.AddTransient<Form1>();
    }
}