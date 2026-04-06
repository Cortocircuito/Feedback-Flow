using PdfSharp.Fonts;

namespace FeedbackFlow.Core.Helpers;

/// <summary>
/// Custom IFontResolver for PDFSharp on .NET Core+.
/// Reads fonts directly from the Windows Fonts directory since PDFSharp
/// cannot access system fonts automatically outside of .NET Framework.
/// </summary>
public class WindowsFontResolver : IFontResolver
{
    private static readonly string FontsFolder =
        Environment.GetFolderPath(Environment.SpecialFolder.Fonts);

    // Map logical family+style combos to actual .ttf file names
    private static readonly Dictionary<string, string> FontMap =
        new(StringComparer.OrdinalIgnoreCase)
        {
            // Arial
            { "Arial#400",  "arial.ttf"    },  // Regular
            { "Arial#700",  "arialbd.ttf"  },  // Bold
            { "Arial#401",  "ariali.ttf"   },  // Italic
            { "Arial#701",  "arialbi.ttf"  },  // BoldItalic

            // Fallback if Arial is not available (Segoe UI ships with Win 10/11)
            { "Segoe UI#400", "segoeui.ttf"  },
            { "Segoe UI#700", "segoeuib.ttf" },
        };

    public FontResolverInfo ResolveTypeface(string familyName, bool bold, bool italic)
    {
        int style = (bold ? 700 : 400) + (italic ? 1 : 0);
        string key = $"{familyName}#{style}";

        if (FontMap.TryGetValue(key, out var fileName))
            return new FontResolverInfo(key);

        // Try plain regular as final fallback
        string regularKey = $"{familyName}#400";
        if (FontMap.ContainsKey(regularKey))
            return new FontResolverInfo(regularKey);

        // If still not found, fall back to Arial Regular
        return new FontResolverInfo("Arial#400");
    }

    public byte[] GetFont(string faceName)
    {
        if (!FontMap.TryGetValue(faceName, out var fileName))
            fileName = "arial.ttf";  // hard fallback

        var path = Path.Combine(FontsFolder, fileName);

        if (!File.Exists(path))
            throw new FileNotFoundException(
                $"Font file '{fileName}' not found in the Windows Fonts directory. " +
                $"Looked at: {path}");

        return File.ReadAllBytes(path);
    }
}
