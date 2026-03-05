namespace Feedback_Flow.UI.Theme;

/// <summary>
/// Centralised theme management.  Defines light and dark colour palettes and
/// applies them recursively to a WinForms control tree.
/// </summary>
internal static class ThemeManager
{
    // ── State ────────────────────────────────────────────────────────────────
    public static bool IsDarkMode { get; private set; }

    /// <summary>Raised after <see cref="IsDarkMode"/> changes.</summary>
    public static event EventHandler ThemeChanged = delegate { };

    // ── Light palette ────────────────────────────────────────────────────────
    private static readonly Color LightBackground  = Color.FromArgb(240, 240, 240);
    private static readonly Color LightSurface     = Color.White;
    private static readonly Color LightForeground  = Color.FromArgb(33,  33,  33);
    private static readonly Color LightSecondary   = Color.FromArgb(117, 117, 117);

    // ── Dark palette ─────────────────────────────────────────────────────────
    private static readonly Color DarkBackground   = Color.FromArgb(30,  30,  30);
    private static readonly Color DarkSurface      = Color.FromArgb(45,  45,  48);
    private static readonly Color DarkForeground   = Color.FromArgb(240, 240, 240);
    private static readonly Color DarkSecondary    = Color.FromArgb(180, 180, 180);

    // ── Active palette (resolves to light or dark) ───────────────────────────
    public static Color Background  => IsDarkMode ? DarkBackground  : LightBackground;
    public static Color Surface     => IsDarkMode ? DarkSurface     : LightSurface;
    public static Color Foreground  => IsDarkMode ? DarkForeground  : LightForeground;
    public static Color Secondary   => IsDarkMode ? DarkSecondary   : LightSecondary;

    // ── Semantic colours for the mode-indicator banner ───────────────────────
    public static (Color Back, Color Fore) AllStudentsColors =>
        IsDarkMode
            ? (Color.FromArgb(13,  43,  84), Color.FromArgb(100, 181, 246))
            : (Color.FromArgb(227, 242, 253), Color.FromArgb(21,  101, 192));

    public static (Color Back, Color Fore) TodayColors =>
        IsDarkMode
            ? (Color.FromArgb(15,  50,  18), Color.FromArgb(165, 214, 167))
            : (Color.FromArgb(232, 245, 233), Color.FromArgb(46,  125,  50));

    public static (Color Back, Color Fore) HistoricalColors =>
        IsDarkMode
            ? (Color.FromArgb(80,  40,   0), Color.FromArgb(255, 193,  99))
            : (Color.FromArgb(255, 253, 231), Color.FromArgb(245, 124,   0));

    // ── Semantic colours for the class-description panel ────────────────────
    public static (Color Back, Color Fore) DescriptionColors =>
        IsDarkMode
            ? (Color.FromArgb(20,  50,  90), Color.FromArgb(187, 222, 251))
            : (Color.FromArgb(232, 244, 253), Color.FromArgb(13,   71, 161));

    // Flat/borderless button background (e.g. the inline clear-search "✕" button)
    public static Color FlatButtonBackground =>
        IsDarkMode ? Color.FromArgb(60, 60, 60) : Color.White;

    // ── API ──────────────────────────────────────────────────────────────────
    public static void SetDarkMode(bool isDark)
    {
        IsDarkMode = isDark;
        ThemeChanged?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    /// Recursively applies the current theme colours to <paramref name="root"/>
    /// and every descendant.  Panels with semantic colours (mode indicator,
    /// description panel) should be repainted by the caller afterwards.
    /// </summary>
    public static void Apply(Control root)
    {
        ApplySingle(root);
        foreach (Control child in root.Controls)
            Apply(child);
    }

    // ── Internal helpers ─────────────────────────────────────────────────────
    private static void ApplySingle(Control c)
    {
        switch (c)
        {
            case DataGridView dgv:
                ApplyToGrid(dgv);
                break;

            case StatusStrip ss:
                ss.BackColor = Background;
                ss.ForeColor = Foreground;
                foreach (ToolStripItem item in ss.Items)
                {
                    item.BackColor = Background;
                    item.ForeColor = Secondary;
                }
                break;

            case TextBox tb:
                tb.BackColor = Surface;
                tb.ForeColor = Foreground;
                break;

            case RichTextBox rtb:
                rtb.BackColor = Surface;
                rtb.ForeColor = Foreground;
                break;

            case CheckedListBox clb:
                clb.BackColor = Surface;
                clb.ForeColor = Foreground;
                break;

            case Label lbl:
                lbl.ForeColor = Foreground;
                lbl.BackColor = Color.Transparent;
                break;

            // Flat buttons (e.g. the inline "✕" clear-search button)
            case Button btn when btn.FlatStyle == FlatStyle.Flat:
                btn.BackColor = FlatButtonBackground;
                btn.ForeColor = Foreground;
                break;

            // Generic containers
            case Form _:
            case TableLayoutPanel _:
            case FlowLayoutPanel _:
            case Panel _:
                c.BackColor = Background;
                c.ForeColor = Foreground;
                break;
        }
    }

    private static void ApplyToGrid(DataGridView dgv)
    {
        dgv.BackgroundColor = Surface;
        dgv.EnableHeadersVisualStyles = false;

        dgv.DefaultCellStyle.BackColor = Surface;
        dgv.DefaultCellStyle.ForeColor = Foreground;
        dgv.DefaultCellStyle.SelectionBackColor =
            IsDarkMode ? Color.FromArgb(0, 80, 160) : Color.FromArgb(0, 120, 215);
        dgv.DefaultCellStyle.SelectionForeColor = Color.White;

        dgv.AlternatingRowsDefaultCellStyle.BackColor =
            IsDarkMode ? Color.FromArgb(50, 50, 55) : Color.FromArgb(245, 245, 250);

        dgv.ColumnHeadersDefaultCellStyle.BackColor =
            IsDarkMode ? Color.FromArgb(40, 40, 40) : Color.FromArgb(230, 230, 230);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Foreground;

        dgv.GridColor =
            IsDarkMode ? Color.FromArgb(70, 70, 70) : Color.FromArgb(210, 210, 210);
    }
}
