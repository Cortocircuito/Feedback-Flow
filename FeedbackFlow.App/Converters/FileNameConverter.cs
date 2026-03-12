using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;

namespace FeedbackFlow.App.Converters;

public class FileNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is string path && !string.IsNullOrEmpty(path)
            ? Path.GetFileName(path) : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value;
}
