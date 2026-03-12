using System;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Feedback_Flow.Models;

namespace FeedbackFlow.Avalonia.Views;

public partial class StudentDialog : Window
{
    public string StudentFullName => TxtFullName.Text?.Trim() ?? string.Empty;
    public string StudentEmail => TxtEmail.Text?.Trim() ?? string.Empty;
    public string ClassDays
    {
        get
        {
            var days = new System.Collections.Generic.List<string>();
            if (ChkMonday.IsChecked == true) days.Add("Monday");
            if (ChkTuesday.IsChecked == true) days.Add("Tuesday");
            if (ChkWednesday.IsChecked == true) days.Add("Wednesday");
            if (ChkThursday.IsChecked == true) days.Add("Thursday");
            if (ChkFriday.IsChecked == true) days.Add("Friday");
            if (ChkSaturday.IsChecked == true) days.Add("Saturday");
            if (ChkSunday.IsChecked == true) days.Add("Sunday");
            return string.Join(",", days);
        }
    }

    private readonly Student? _existingStudent;

    public StudentDialog()
    {
        InitializeComponent();
    }

    public StudentDialog(Student student) : this()
    {
        _existingStudent = student;
        TxtFullName.Text = student.FullName;
        TxtEmail.Text = student.Email;

        var studentDays = student.GetClassDays();
        ChkMonday.IsChecked = studentDays.Contains("Monday");
        ChkTuesday.IsChecked = studentDays.Contains("Tuesday");
        ChkWednesday.IsChecked = studentDays.Contains("Wednesday");
        ChkThursday.IsChecked = studentDays.Contains("Thursday");
        ChkFriday.IsChecked = studentDays.Contains("Friday");
        ChkSaturday.IsChecked = studentDays.Contains("Saturday");
        ChkSunday.IsChecked = studentDays.Contains("Sunday");
    }

    private void BtnSave_Click(object? sender, RoutedEventArgs e)
    {
        if (!ValidateInput())
        {
            return;
        }

        Close(true);
    }

    private void BtnCancel_Click(object? sender, RoutedEventArgs e)
    {
        Close(false);
    }

    private bool ValidateInput()
    {
        TxtError.Text = string.Empty;

        var name = TxtFullName.Text?.Trim() ?? string.Empty;
        var email = TxtEmail.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name))
        {
            TxtError.Text = "Name cannot be empty.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(email) || !Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
        {
            TxtError.Text = "Invalid email format.";
            return false;
        }

        return true;
    }
}
