using Avalonia.Controls;
using Avalonia.Interactivity;

namespace FeedbackFlow.App.Views;

public partial class ConfirmDialog : Window
{
    public ConfirmDialog(string message)
    {
        InitializeComponent();
        TxtMessage.Text = message;
    }

    private void BtnConfirm_Click(object? sender, RoutedEventArgs e) => Close(true);
    private void BtnCancel_Click(object? sender, RoutedEventArgs e) => Close(false);
}
