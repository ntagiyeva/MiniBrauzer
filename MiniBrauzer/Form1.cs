using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;

namespace MiniBrauzer;

public partial class Form1 : Form
{
    private const string HomePage = "https://mini-brauzer.test/";
    private const string HomeAddress = "https://www.example.com";
    private static readonly string DataFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MiniBrauzer");
    private static readonly string StateFile = Path.Combine(DataFolder, "browser.json");
    private const int HistoryLimit = 100;

    private BrowserState state = new();
    private int nextTabNumber;
    private Panel? activeTab;
    private Button? activeTabButton;
    private CoreWebView2? ActiveBrowser => (activeTab?.Tag as WebView2)?.CoreWebView2;

    public Form1()
    {
        InitializeComponent();
    }

    protected override async void OnShown(EventArgs e)
    {
        base.OnShown(e);
        ResizeListRows();
        LoadState();
        addressBar.Text = HomeAddress;
        await Task.WhenAll(AddTab(activate: true), AddTab());
    }

    private async Task AddTab(Uri? address = null, bool activate = false)
    {
        var view = new WebView2
        {
            Dock = DockStyle.Fill,
            CreationProperties = new CoreWebView2CreationProperties
            {
                UserDataFolder = Path.Combine(DataFolder, "WebView2")
            }
        };

        var tab = new Panel { Tag = view, Dock = DockStyle.Fill, Visible = false };
        tab.Controls.Add(view);
        webPanel.Controls.Add(tab);

        var tabButton = new Button
        {
            Text = $"Tab {++nextTabNumber}",
            FlatStyle = FlatStyle.Flat,
            BackColor = Color.FromArgb(221, 225, 230),
            Size = LogicalToDeviceUnits(new Size(76, 23)),
            Margin = new Padding(0, 0, LogicalToDeviceUnits(4), 0),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(LogicalToDeviceUnits(5), 0, 0, 0),
            UseVisualStyleBackColor = false
        };
        tabButton.FlatAppearance.BorderColor = Color.FromArgb(157, 164, 172);
        tabButton.Click += (_, _) => SelectTab(tab, tabButton);
        tabStrip.Controls.Add(tabButton);
        tabStrip.Controls.SetChildIndex(plusButton, tabStrip.Controls.Count - 1);
        if (activate)
            SelectTab(tab, tabButton);

        try
        {
            await view.EnsureCoreWebView2Async();
            if (view.IsDisposed)
                return;

            CoreWebView2 browser = view.CoreWebView2;
            browser.Settings.IsWebMessageEnabled = false;
            browser.Settings.AreHostObjectsAllowed = false;
            browser.AddWebResourceRequestedFilter(HomePage, CoreWebView2WebResourceContext.Document);
            browser.WebResourceRequested += (_, args) =>
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(StartPageHtml));
                args.Response = browser.Environment.CreateWebResourceResponse(
                    stream, 200, "OK", "Content-Type: text/html; charset=utf-8");
            };
            browser.SourceChanged += (_, _) => UpdateIfActive(tab);
            browser.HistoryChanged += (_, _) => UpdateIfActive(tab);
            browser.NavigationStarting += (_, _) =>
            {
                if (activeTab == tab)
                    statusLabel.Text = "";
            };
            browser.NavigationCompleted += (_, args) =>
            {
                if (activeTab == tab)
                    statusLabel.Text = args.IsSuccess ? "" : $"Səhifə açılmadı ({args.WebErrorStatus}).";
                if (args.IsSuccess && browser.Source != HomePage && TryWebAddress(browser.Source, out var visited))
                    RecordVisit(visited);
                UpdateIfActive(tab);
            };
            browser.NewWindowRequested += async (_, args) =>
            {
                args.Handled = true;
                if (TryWebAddress(args.Uri, out var target))
                    await AddTab(target, activate: true);
            };

            browser.Navigate(address?.AbsoluteUri ?? HomePage);
            UpdateIfActive(tab);
        }
        catch (Exception ex) when (ex is WebView2RuntimeNotFoundException or COMException
                                   or InvalidOperationException or UnauthorizedAccessException or IOException)
        {
            if (tab.IsDisposed)
                return;

            tab.Tag = null;
            view.Dispose();
            tab.Controls.Add(new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = ex is WebView2RuntimeNotFoundException
                    ? "Microsoft Edge WebView2 Runtime quraşdırılmalıdır."
                    : $"WebView2 başladılmadı. {ex.Message}"
            });
            UpdateIfActive(tab);
        }
    }

    private async void PlusClicked(object? sender, EventArgs e) => await AddTab(activate: true);

    private void SelectTab(Panel tab, Button button)
    {
        if (activeTab is not null)
            activeTab.Hide();
        if (activeTabButton is not null)
            activeTabButton.BackColor = Color.FromArgb(221, 225, 230);

        activeTab = tab;
        activeTabButton = button;
        button.BackColor = Color.FromArgb(249, 249, 249);
        tab.Show();
        tab.BringToFront();
        RefreshNavigation();
        statusLabel.Text = "";
    }

    private void UpdateIfActive(Panel tab)
    {
        if (activeTab == tab)
            RefreshNavigation();
    }

    private void RefreshNavigation()
    {
        var browser = ActiveBrowser;
        var source = browser?.Source;
        addressBar.Text = source != HomePage && TryWebAddress(source, out var address)
            ? address.AbsoluteUri
            : HomeAddress;
    }

    private static bool TryWebAddress(string? value, out Uri address)
    {
        if (Uri.TryCreate(value, UriKind.Absolute, out var url)
            && (url.Scheme == Uri.UriSchemeHttps || url.Scheme == Uri.UriSchemeHttp)
            && url.Host.Length > 0)
        {
            address = url;
            return true;
        }

        address = null!;
        return false;
    }

    private void Go()
    {
        var input = addressBar.Text.Trim();
        if (!input.Contains("://", StringComparison.Ordinal))
            input = "https://" + input;

        if (TryWebAddress(input, out var address))
        {
            ActiveBrowser?.Navigate(address.AbsoluteUri);
            (activeTab?.Tag as WebView2)?.Focus();
        }
        else
        {
            statusLabel.Text = "Düzgün veb ünvanı daxil edin.";
            addressBar.Focus();
        }
    }

    private void GoClicked(object? sender, EventArgs e) => Go();
    private void BackClicked(object? sender, EventArgs e)
    {
        if (ActiveBrowser is { CanGoBack: true } browser)
            browser.GoBack();
    }

    private void ForwardClicked(object? sender, EventArgs e)
    {
        if (ActiveBrowser is { CanGoForward: true } browser)
            browser.GoForward();
    }

    private void ReloadClicked(object? sender, EventArgs e) => ActiveBrowser?.Reload();
    private void HomeClicked(object? sender, EventArgs e) => ActiveBrowser?.Navigate(HomePage);

    private void AddressKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            Go();
        }
    }

    private void AddFavoriteClicked(object? sender, EventArgs e)
    {
        var source = ActiveBrowser?.Source;
        if (!TryWebAddress(source == HomePage ? HomeAddress : source, out var address))
            return;

        var entry = new PageEntry(address.AbsoluteUri);
        if (state.Favorites.Contains(entry))
        {
            statusLabel.Text = "Bu səhifə artıq seçilmişlərdədir.";
            return;
        }

        state.Favorites.Add(entry);
        favoritesList.Items.Add(entry);
        statusLabel.Text = "Səhifə seçilmişlərə əlavə olundu.";
        SaveState();
    }

    private void RecordVisit(Uri address)
    {
        var entry = new PageEntry(address.AbsoluteUri);
        if (state.History.Count > 0 && state.History[0] == entry)
            return;

        state.History.Remove(entry);
        state.History.Insert(0, entry);
        if (state.History.Count > HistoryLimit)
            state.History.RemoveAt(HistoryLimit);

        historyList.Items.Clear();
        historyList.Items.AddRange(state.History.ToArray());
        SaveState();
    }

    private void OpenListAddress(object? sender, EventArgs e)
    {
        if (sender is ListBox { SelectedItem: PageEntry entry }
            && TryWebAddress(entry.Url, out var address))
        {
            ActiveBrowser?.Navigate(address.AbsoluteUri);
        }
    }

    private void ListKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.SuppressKeyPress = true;
            OpenListAddress(sender, e);
        }
    }

    private void LoadState()
    {
        try
        {
            if (File.Exists(StateFile))
                state = JsonSerializer.Deserialize<BrowserState>(File.ReadAllText(StateFile)) ?? new BrowserState();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            statusLabel.Text = "Saxlanmış siyahılar oxunmadı.";
        }

        favoritesList.Items.AddRange(state.Favorites.ToArray());
        historyList.Items.AddRange(state.History.ToArray());
    }

    private void SaveState()
    {
        var temporaryFile = Path.Combine(DataFolder, $"{Guid.NewGuid():N}.tmp");
        try
        {
            Directory.CreateDirectory(DataFolder);
            File.WriteAllText(temporaryFile, JsonSerializer.Serialize(state));
            File.Move(temporaryFile, StateFile, overwrite: true);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            statusLabel.Text = "Siyahılar yadda saxlanmadı.";
            try { File.Delete(temporaryFile); }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }

    private sealed record PageEntry(string Url)
    {
        public override string ToString()
        {
            if (!TryWebAddress(Url, out var address))
                return Url;

            var host = address.Authority;
            if (host.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
                host = host[4..];

            return address.PathAndQuery == "/" ? host + address.Fragment : host + address.PathAndQuery + address.Fragment;
        }
    }

    private sealed class BrowserState
    {
        public List<PageEntry> Favorites { get; set; } =
        [
            new("https://www.google.com/"),
            new("https://www.wikipedia.org/"),
            new("https://www.youtube.com/"),
            new("https://azertag.az/")
        ];

        public List<PageEntry> History { get; set; } =
        [
            new("https://www.example.com/"),
            new("https://www.bing.com/"),
            new("https://github.com/")
        ];
    }

    private void PaintPanelBorder(object? sender, PaintEventArgs e) =>
        PaintBorder((Control)sender!, e.Graphics, Color.FromArgb(91, 103, 112));

    private void PaintAddressBorder(object? sender, PaintEventArgs e) =>
        PaintBorder((Control)sender!, e.Graphics, Color.FromArgb(157, 164, 172));

    private static void PaintBorder(Control control, Graphics graphics, Color color)
    {
        using var pen = new Pen(color);
        var bounds = control.ClientRectangle;
        graphics.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
    }

    private void DrawListEntry(object? sender, DrawItemEventArgs e)
    {
        if (sender is not ListBox list || e.Index < 0)
            return;

        var row = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width - 1, LogicalToDeviceUnits(22));
        bool selected = (e.State & DrawItemState.Selected) != 0;
        using var background = new SolidBrush(selected ? Color.FromArgb(222, 235, 250) : Color.White);
        using var border = new Pen(Color.FromArgb(203, 213, 224));
        e.Graphics.FillRectangle(background, row);
        e.Graphics.DrawRectangle(border, row);
        row.Inflate(-LogicalToDeviceUnits(6), 0);
        TextRenderer.DrawText(e.Graphics, list.Items[e.Index]?.ToString(), list.Font, row,
            Color.FromArgb(45, 55, 72), TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
    }

    protected override void OnDpiChanged(DpiChangedEventArgs e)
    {
        base.OnDpiChanged(e);
        ResizeListRows();
    }

    private void ResizeListRows()
    {
        favoritesList.ItemHeight = historyList.ItemHeight = LogicalToDeviceUnits(27);
    }

    private void MinimizeClicked(object? sender, EventArgs e) => WindowState = FormWindowState.Minimized;

    private void MaximizeClicked(object? sender, EventArgs e)
    {
        if (WindowState == FormWindowState.Maximized)
            WindowState = FormWindowState.Normal;
        else
        {
            MaximizedBounds = Screen.FromControl(this).WorkingArea;
            WindowState = FormWindowState.Maximized;
        }
    }

    private void CloseClicked(object? sender, EventArgs e)
    {
        var result = MessageBox.Show(this, "Çıxmaq istədiyinizə əminsiniz?", "Çıxış",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (result == DialogResult.Yes)
            Close();
    }

    private void TitleMouseDown(object? sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Left && e.Clicks == 1 && WindowState == FormWindowState.Normal)
        {
            ReleaseCapture();
            SendMessage(Handle, 0x00A1, 2, 0);
        }
    }

    [DllImport("user32.dll")]
    private static extern bool ReleaseCapture();

    [DllImport("user32.dll", EntryPoint = "SendMessageW")]
    private static extern nint SendMessage(nint window, uint message, nint wParam, nint lParam);

    private const string StartPageHtml = """
        <!doctype html>
        <html lang="az">
        <meta charset="utf-8">
        <meta name="viewport" content="width=device-width,initial-scale=1">
        <title>WebView2</title>
        <style>
          body { margin: 0; padding: 9px 12px; background: white; color: #4a5568;
            font: 15px 'Segoe UI', sans-serif; }
          h1 { margin: 0 0 3px; color: #2b6cb0; font-size: 24px; }
          p { margin: 0; }
          section { display: grid; grid-template-columns: 1fr 1fr; gap: 24px; margin: 19px 8px 0 0; }
          .line { display: block; width: 92%; height: 11px; margin-bottom: 9px; background: #e2e8f0; }
          .line:nth-child(2) { width: 75%; }
          .line:nth-child(3) { width: 84%; }
          .media { display: grid; place-items: center; height: 110px; background: #bee3f8;
            color: #2a527e; font-size: 14px; }
        </style>
        <h1>WebView2</h1>
        <p>Burada veb-səhifə göstərilir</p>
        <section>
          <div aria-hidden="true"><span class="line"></span><span class="line"></span><span class="line"></span></div>
          <div class="media">şəkil / video</div>
        </section>
        </html>
        """;
}
