using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
using Avalonia.Markup.Xaml;
using FeedbackFlow.Avalonia.ViewModels;
using FeedbackFlow.Avalonia.Views;
using Feedback_Flow.Services;
using Feedback_Flow.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FeedbackFlow.Avalonia;

public partial class App : Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            DisableAvaloniaDataAnnotationValidation();
            
            var services = new ServiceCollection();
            ConfigureServices(services);
            Services = services.BuildServiceProvider();
            
            var mainWindow = new MainWindow();
            var viewModel = Services.GetRequiredService<MainWindowViewModel>();
            viewModel.SetMainWindow(mainWindow);
            mainWindow.DataContext = viewModel;
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IDatabaseService, SqliteDatabaseService>();
        services.AddSingleton<IStudentService, StudentService>();
        services.AddSingleton<IFileSystemService, FileSystemService>();
        services.AddSingleton<INoteService, NoteService>();
        services.AddSingleton<IPdfService, PdfGenerationService>();
        services.AddSingleton<IEmailService, OutlookEmailService>();
        
        services.AddTransient<MainWindowViewModel>();
    }

    private void DisableAvaloniaDataAnnotationValidation()
    {
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
}
