using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using FeedbackFlow.Core.Models;

namespace FeedbackFlow.App.Views;

public partial class PrepareClassDialog : Window
{
    public string? SelectedMaterial { get; private set; }
    public string? ClassDescription
    {
        get
        {
            var text = TextDescription?.Text?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
    }

    public PrepareClassDialog(string studentName, DateTime nextClassDate, ClassSession? existingSession)
    {
        InitializeComponent();

        TxtStudentName.Text = $"Preparing next class for: {studentName}";
        TxtNextDate.Text = $"Next class date: {nextClassDate:dddd, dd MMM yyyy}";

        if (existingSession != null)
        {
            SelectedMaterial = existingSession.AssignedMaterial;
            TextDescription.Text = existingSession.ClassDescription ?? string.Empty;
        }

        UpdateMaterialLabel();
    }

    private async void BtnBrowse_Click(object? sender, RoutedEventArgs e)
    {
        var options = new FilePickerOpenOptions
        {
            Title = "Select Learning Material",
            AllowMultiple = false,
            FileTypeFilter = new[]
            {
                new FilePickerFileType("All Supported Files")
                {
                    Patterns = new[] { "*.pdf", "*.docx", "*.doc", "*.odt", "*.pptx", "*.ppt", "*.odp" }
                },
                new FilePickerFileType("PDF Files") { Patterns = new[] { "*.pdf" } },
                new FilePickerFileType("Word Documents") { Patterns = new[] { "*.docx", "*.doc" } },
                new FilePickerFileType("LibreOffice Writer") { Patterns = new[] { "*.odt" } },
                new FilePickerFileType("PowerPoint") { Patterns = new[] { "*.pptx", "*.ppt" } },
                new FilePickerFileType("LibreOffice Impress") { Patterns = new[] { "*.odp" } }
            }
        };

        var result = await StorageProvider.OpenFilePickerAsync(options);
        if (result.Count > 0)
        {
            SelectedMaterial = result[0].Path.LocalPath;
            UpdateMaterialLabel();
        }
    }

    private void BtnClearMaterial_Click(object? sender, RoutedEventArgs e)
    {
        SelectedMaterial = null;
        UpdateMaterialLabel();
    }

    private void UpdateMaterialLabel()
    {
        if (string.IsNullOrEmpty(SelectedMaterial))
        {
            TxtMaterialPath.Text = "No material assigned";
            TxtMaterialPath.Opacity = 0.6;
            BtnClearMaterial.IsEnabled = false;
        }
        else
        {
            TxtMaterialPath.Text = System.IO.Path.GetFileName(SelectedMaterial);
            TxtMaterialPath.Opacity = 1;
            BtnClearMaterial.IsEnabled = true;
        }
    }

    private void BtnSave_Click(object? sender, RoutedEventArgs e)
    {
        Close(true);
    }

    private void BtnCancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }
}
