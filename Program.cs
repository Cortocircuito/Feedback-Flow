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

        // Run Database Migration
        var databaseService = ServiceProvider.GetRequiredService<IDatabaseService>();
        // Ensure DB exists first
        Task.Run(async () => await databaseService.InitializeDatabaseAsync()).Wait();

        var migrationService = ServiceProvider.GetRequiredService<IMigrationService>();
        Task.Run(async () => await migrationService.MigrateFromCsvAsync()).Wait();

        // Resolve MainDashboard
        var mainForm = ServiceProvider.GetRequiredService<MainDashboard>();
        Application.Run(mainForm);
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Register Services
        services.AddSingleton<IDatabaseService, SqliteDatabaseService>();
        services.AddSingleton<IMigrationService, MigrationService>();
        
        services.AddSingleton<IFileSystemService, FileSystemService>();
        // services.AddSingleton<IDataService, CsvDataService>(); // Removed
        services.AddSingleton<IPdfService, PdfGenerationService>();
        services.AddSingleton<IEmailService, OutlookEmailService>();
        services.AddSingleton<IStudentService, StudentService>();
        services.AddSingleton<INoteService, NoteService>();

        // Register Forms
        services.AddTransient<MainDashboard>();
    }
}