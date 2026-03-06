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
    private static readonly Color LightBackground = Color.FromArgb(235, 240, 245);
    private static readonly Color LightSurface = Color.FromArgb(250, 251, 253);
    private static readonly Color LightForeground = Color.FromArgb(22, 38, 55);
    private static readonly Color LightSecondary = Color.FromArgb(85, 100, 120);

    // ── Dark palette ─────────────────────────────────────────────────────────
    private static readonly Color DarkBackground   = Color.FromArgb(32,  32,  38);
    private static readonly Color DarkSurface      = Color.FromArgb(42,  42,  48);
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
            ? (Color.FromArgb(13, 43, 84), Color.FromArgb(100, 181, 246))
            : (Color.FromArgb(227, 242, 253), Color.FromArgb(21, 101, 192));

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
        IsDarkMode ? Color.FromArgb(60, 60, 60) : Color.FromArgb(250, 251, 253);

    // Standard (non-Flat) button background
    public static Color ButtonBackground =>
        IsDarkMode ? Color.FromArgb(50, 58, 78) : Color.FromArgb(242, 245, 248);

    public static Color DisabledButtonBackground =>
        IsDarkMode ? Color.FromArgb(28, 32, 42) : Color.FromArgb(218, 225, 233);

    public static Color DisabledForeground =>
        IsDarkMode ? Color.FromArgb(80, 82, 90) : Color.FromArgb(118, 130, 145);

    // Status bar background — distinct from form background for visual separation
    public static Color StatusBarBackground =>
        IsDarkMode ? Color.FromArgb(22, 22, 28) : Color.FromArgb(205, 215, 225);

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
                ss.BackColor = StatusBarBackground;
                ss.ForeColor = Foreground;
                ss.SizingGrip = false;
                ss.Renderer = IsDarkMode
                    ? new DarkStatusStripRenderer()
                    : new LightStatusStripRenderer();
                foreach (ToolStripItem item in ss.Items)
                {
                    item.BackColor = StatusBarBackground;
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

            case Button btn:
                if (IsOriginallyFlatButton(btn))
                {
                    btn.BackColor = FlatButtonBackground;
                    btn.ForeColor = Foreground;
                }
                else
                {
                    btn.UseVisualStyleBackColor = false;
                    if (IsDarkMode)
                    {
                        btn.FlatStyle = FlatStyle.Flat;
                        btn.FlatAppearance.BorderColor = Color.FromArgb(65, 75, 98);
                        btn.FlatAppearance.BorderSize = 1;
                    }
                    else
                    {
                        btn.FlatStyle = FlatStyle.Standard;
                    }

                    ApplyButtonColors(btn);
                    btn.EnabledChanged -= OnButtonEnabledChanged;
                    btn.EnabledChanged += OnButtonEnabledChanged;
                }

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

    private static void ApplyButtonColors(Button btn)
    {
        btn.BackColor = btn.Enabled ? ButtonBackground : DisabledButtonBackground;
        btn.ForeColor = btn.Enabled ? Foreground       : DisabledForeground;
    }

    private static bool IsOriginallyFlatButton(Button btn) =>
        btn.FlatAppearance.BorderSize == 0;

    private static void OnButtonEnabledChanged(object? sender, EventArgs e)
    {
        if (sender is Button btn && !IsOriginallyFlatButton(btn))
            ApplyButtonColors(btn);
    }

    private sealed class LightStatusStripRenderer : ToolStripProfessionalRenderer
    {
        public LightStatusStripRenderer()
            : base(new LightColorTable())
        {
        }

        private sealed class LightColorTable : ProfessionalColorTable
        {
            // Hover background — StatusBarBackground (205,215,225) + ~13 pts lighter
            public override Color ButtonSelectedHighlight => Color.FromArgb(218, 227, 236);
            public override Color ButtonSelectedHighlightBorder => Color.FromArgb(185, 200, 215);
            public override Color ButtonSelectedGradientBegin => Color.FromArgb(218, 227, 236);
            public override Color ButtonSelectedGradientMiddle => Color.FromArgb(218, 227, 236);

            public override Color ButtonSelectedGradientEnd => Color.FromArgb(218, 227, 236);

            // Pressed background — StatusBarBackground itself
            public override Color ButtonPressedHighlight => Color.FromArgb(205, 215, 225);
            public override Color ButtonPressedHighlightBorder => Color.FromArgb(165, 185, 205);
            public override Color ButtonPressedGradientBegin => Color.FromArgb(205, 215, 225);
            public override Color ButtonPressedGradientMiddle => Color.FromArgb(205, 215, 225);

            public override Color ButtonPressedGradientEnd => Color.FromArgb(205, 215, 225);

            // Status strip bar itself
            public override Color StatusStripGradientBegin => Color.FromArgb(205, 215, 225);
            public override Color StatusStripGradientEnd => Color.FromArgb(205, 215, 225);
        }
    }

    private sealed class DarkStatusStripRenderer : ToolStripProfessionalRenderer
    {
        public DarkStatusStripRenderer()
            : base(new DarkColorTable())
        {
        }

        private sealed class DarkColorTable : ProfessionalColorTable
        {
            // Hover background — slightly lighter than StatusBarBackground
            public override Color ButtonSelectedHighlight => Color.FromArgb(55, 65, 88);
            public override Color ButtonSelectedHighlightBorder => Color.FromArgb(65, 75, 98);
            public override Color ButtonSelectedGradientBegin => Color.FromArgb(55, 65, 88);
            public override Color ButtonSelectedGradientMiddle => Color.FromArgb(55, 65, 88);

            public override Color ButtonSelectedGradientEnd => Color.FromArgb(55, 65, 88);

            // Pressed background
            public override Color ButtonPressedHighlight => Color.FromArgb(42, 52, 72);
            public override Color ButtonPressedHighlightBorder => Color.FromArgb(65, 75, 98);
            public override Color ButtonPressedGradientBegin => Color.FromArgb(42, 52, 72);
            public override Color ButtonPressedGradientMiddle => Color.FromArgb(42, 52, 72);

            public override Color ButtonPressedGradientEnd => Color.FromArgb(42, 52, 72);

            // Status strip bar itself
            public override Color StatusStripGradientBegin => Color.FromArgb(22, 22, 28);
            public override Color StatusStripGradientEnd => Color.FromArgb(22, 22, 28);
        }
    }

    [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(
        IntPtr hwnd, int dwAttribute, ref int pvAttribute, int cbAttribute);

    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
    private const int DWMWA_CAPTION_COLOR           = 35;

    /// <summary>
    /// Applies the current theme to the Win32 title bar of <paramref name="form"/>.
    /// Uses DWMWA_USE_IMMERSIVE_DARK_MODE (Win10+) and DWMWA_CAPTION_COLOR (Win11+).
    /// Silently ignored on older Windows versions.
    /// </summary>
    public static void ApplyTitleBar(Form form)
    {
        if (!form.IsHandleCreated)
        {
            form.HandleCreated += (s, e) => ApplyTitleBar(form);
            return;
        }

        int dark = IsDarkMode ? 1 : 0;
        DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE,
            ref dark, sizeof(int));

        // COLORREF = 0x00BBGGRR
        Color c = Background;
        int colorRef = c.R | (c.G << 8) | (c.B << 16);
        DwmSetWindowAttribute(form.Handle, DWMWA_CAPTION_COLOR,
            ref colorRef, sizeof(int));
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
            IsDarkMode ? Color.FromArgb(50, 50, 55) : Color.FromArgb(242, 245, 249);

        dgv.ColumnHeadersDefaultCellStyle.BackColor =
            IsDarkMode ? Color.FromArgb(40, 40, 40) : Color.FromArgb(212, 222, 232);
        dgv.ColumnHeadersDefaultCellStyle.ForeColor = Foreground;

        dgv.GridColor =
            IsDarkMode ? Color.FromArgb(70, 70, 70) : Color.FromArgb(200, 210, 220);
    }
}
