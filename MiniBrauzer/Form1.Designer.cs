namespace MiniBrauzer;

partial class Form1
{
    private Label heading = null!;
    private Panel browserPanel = null!;
    private Panel titlePanel = null!;
    private Label windowTitle = null!;
    private Button minimizeButton = null!;
    private Button maximizeButton = null!;
    private Button closeButton = null!;
    private Button backButton = null!;
    private Button forwardButton = null!;
    private Button reloadButton = null!;
    private Button homeButton = null!;
    private Button goButton = null!;
    private Panel addressFrame = null!;
    private TextBox addressBar = null!;
    private FlowLayoutPanel tabStrip = null!;
    private Button plusButton = null!;
    private Panel webPanel = null!;
    private Panel sidePanel = null!;
    private Label favoritesLabel = null!;
    private Label historyLabel = null!;
    private ListBox favoritesList = null!;
    private ListBox historyList = null!;
    private Button addFavoriteButton = null!;
    private Label statusLabel = null!;

    private void InitializeComponent()
    {
        heading = new Label();
        browserPanel = new Panel();
        titlePanel = new Panel();
        windowTitle = new Label();
        minimizeButton = new Button();
        maximizeButton = new Button();
        closeButton = new Button();
        backButton = new Button();
        forwardButton = new Button();
        reloadButton = new Button();
        homeButton = new Button();
        goButton = new Button();
        addressFrame = new Panel();
        addressBar = new TextBox();
        tabStrip = new FlowLayoutPanel();
        plusButton = new Button();
        webPanel = new Panel();
        sidePanel = new Panel();
        favoritesLabel = new Label();
        historyLabel = new Label();
        favoritesList = new ListBox();
        historyList = new ListBox();
        addFavoriteButton = new Button();
        statusLabel = new Label();

        SuspendLayout();
        browserPanel.SuspendLayout();
        titlePanel.SuspendLayout();
        addressFrame.SuspendLayout();
        tabStrip.SuspendLayout();
        webPanel.SuspendLayout();
        sidePanel.SuspendLayout();

        heading.Name = "heading";
        heading.Text = "C# WinForms + WebView2: mini brauzer layihəsinin təsviri";
        heading.Location = new Point(33, 18);
        heading.Size = new Size(900, 33);
        heading.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        heading.TextAlign = ContentAlignment.MiddleLeft;

        browserPanel.Name = "browserPanel";
        browserPanel.Location = new Point(33, 59);
        browserPanel.Size = new Size(691, 565);
        browserPanel.BackColor = Color.FromArgb(240, 240, 240);
        browserPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        browserPanel.Paint += PaintPanelBorder;

        titlePanel.Name = "titlePanel";
        titlePanel.Location = new Point(0, 0);
        titlePanel.Size = new Size(691, 30);
        titlePanel.BackColor = Color.FromArgb(43, 108, 176);
        titlePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        titlePanel.MouseDown += TitleMouseDown;
        titlePanel.DoubleClick += MaximizeClicked;
        browserPanel.Controls.Add(titlePanel);

        windowTitle.Name = "windowTitle";
        windowTitle.Location = new Point(10, 0);
        windowTitle.Size = new Size(465, 27);
        windowTitle.ForeColor = Color.White;
        windowTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        windowTitle.TextAlign = ContentAlignment.MiddleLeft;
        windowTitle.Text = "Form1 - Mini Brauzer (WinForms + WebView2)";
        windowTitle.MouseDown += TitleMouseDown;
        windowTitle.DoubleClick += MaximizeClicked;
        titlePanel.Controls.Add(windowTitle);

        minimizeButton.Name = "minimizeButton";
        minimizeButton.Location = new Point(605, 1);
        minimizeButton.Size = new Size(25, 24);
        minimizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        minimizeButton.BackColor = Color.FromArgb(43, 108, 176);
        minimizeButton.ForeColor = Color.White;
        minimizeButton.FlatStyle = FlatStyle.Flat;
        minimizeButton.FlatAppearance.BorderSize = 0;
        minimizeButton.Text = "-";
        minimizeButton.AccessibleName = "Kiçilt";
        minimizeButton.UseVisualStyleBackColor = false;
        minimizeButton.Click += MinimizeClicked;
        titlePanel.Controls.Add(minimizeButton);

        maximizeButton.Name = "maximizeButton";
        maximizeButton.Location = new Point(630, 1);
        maximizeButton.Size = new Size(25, 24);
        maximizeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        maximizeButton.BackColor = Color.FromArgb(43, 108, 176);
        maximizeButton.ForeColor = Color.White;
        maximizeButton.FlatStyle = FlatStyle.Flat;
        maximizeButton.FlatAppearance.BorderSize = 0;
        maximizeButton.Text = "□";
        maximizeButton.AccessibleName = "Böyüt və ya bərpa et";
        maximizeButton.UseVisualStyleBackColor = false;
        maximizeButton.Click += MaximizeClicked;
        titlePanel.Controls.Add(maximizeButton);

        closeButton.Name = "closeButton";
        closeButton.Location = new Point(654, 1);
        closeButton.Size = new Size(25, 24);
        closeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        closeButton.BackColor = Color.FromArgb(43, 108, 176);
        closeButton.ForeColor = Color.White;
        closeButton.FlatStyle = FlatStyle.Flat;
        closeButton.FlatAppearance.BorderSize = 0;
        closeButton.Text = "×";
        closeButton.AccessibleName = "Bağla";
        closeButton.UseVisualStyleBackColor = false;
        closeButton.Click += CloseClicked;
        titlePanel.Controls.Add(closeButton);

        backButton.Name = "backButton";
        backButton.Location = new Point(10, 40);
        backButton.Size = new Size(35, 28);
        backButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        backButton.BackColor = Color.White;
        backButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        backButton.FlatStyle = FlatStyle.Flat;
        backButton.FlatAppearance.BorderColor = Color.FromArgb(157, 164, 172);
        backButton.Text = "←";
        backButton.AccessibleName = "Geri";
        backButton.UseVisualStyleBackColor = false;
        backButton.Click += BackClicked;
        browserPanel.Controls.Add(backButton);

        forwardButton.Name = "forwardButton";
        forwardButton.Location = new Point(49, 40);
        forwardButton.Size = new Size(35, 28);
        forwardButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        forwardButton.BackColor = Color.White;
        forwardButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        forwardButton.FlatStyle = FlatStyle.Flat;
        forwardButton.FlatAppearance.BorderColor = Color.FromArgb(157, 164, 172);
        forwardButton.Text = "→";
        forwardButton.AccessibleName = "İrəli";
        forwardButton.UseVisualStyleBackColor = false;
        forwardButton.Click += ForwardClicked;
        browserPanel.Controls.Add(forwardButton);

        reloadButton.Name = "reloadButton";
        reloadButton.Location = new Point(88, 40);
        reloadButton.Size = new Size(35, 28);
        reloadButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        reloadButton.BackColor = Color.White;
        reloadButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        reloadButton.FlatStyle = FlatStyle.Flat;
        reloadButton.FlatAppearance.BorderColor = Color.FromArgb(157, 164, 172);
        reloadButton.Text = "⟳";
        reloadButton.AccessibleName = "Yenilə";
        reloadButton.UseVisualStyleBackColor = false;
        reloadButton.Click += ReloadClicked;
        browserPanel.Controls.Add(reloadButton);

        homeButton.Name = "homeButton";
        homeButton.Location = new Point(127, 40);
        homeButton.Size = new Size(35, 28);
        homeButton.Anchor = AnchorStyles.Top | AnchorStyles.Left;
        homeButton.BackColor = Color.White;
        homeButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        homeButton.FlatStyle = FlatStyle.Flat;
        homeButton.FlatAppearance.BorderColor = Color.FromArgb(157, 164, 172);
        homeButton.Text = "⌂";
        homeButton.AccessibleName = "Ana səhifə";
        homeButton.UseVisualStyleBackColor = false;
        homeButton.Click += HomeClicked;
        browserPanel.Controls.Add(homeButton);

        goButton.Name = "goButton";
        goButton.Location = new Point(605, 40);
        goButton.Size = new Size(75, 28);
        goButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        goButton.BackColor = Color.FromArgb(198, 246, 213);
        goButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        goButton.FlatStyle = FlatStyle.Flat;
        goButton.FlatAppearance.BorderColor = Color.FromArgb(157, 164, 172);
        goButton.Text = "Get";
        goButton.AccessibleName = "Get";
        goButton.UseVisualStyleBackColor = false;
        goButton.Click += GoClicked;
        browserPanel.Controls.Add(goButton);

        addressFrame.Name = "addressFrame";
        addressFrame.Location = new Point(168, 40);
        addressFrame.Size = new Size(430, 28);
        addressFrame.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        addressFrame.BackColor = Color.White;
        addressFrame.Paint += PaintAddressBorder;
        browserPanel.Controls.Add(addressFrame);

        addressBar.Name = "addressBar";
        addressBar.Location = new Point(8, 5);
        addressBar.Size = new Size(414, 19);
        addressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        addressBar.BorderStyle = BorderStyle.None;
        addressBar.ForeColor = Color.FromArgb(74, 85, 104);
        addressBar.AccessibleName = "Ünvan sətri";
        addressBar.KeyDown += AddressKeyDown;
        addressFrame.Controls.Add(addressBar);

        tabStrip.Name = "tabStrip";
        tabStrip.Location = new Point(10, 76);
        tabStrip.Size = new Size(670, 23);
        tabStrip.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        tabStrip.AutoScroll = false;
        tabStrip.WrapContents = false;
        browserPanel.Controls.Add(tabStrip);

        plusButton.Name = "plusButton";
        plusButton.Size = new Size(32, 23);
        plusButton.Margin = Padding.Empty;
        plusButton.BackColor = Color.FromArgb(221, 225, 230);
        plusButton.FlatStyle = FlatStyle.Flat;
        plusButton.FlatAppearance.BorderColor = Color.FromArgb(157, 164, 172);
        plusButton.Text = "+";
        plusButton.AccessibleName = "Yeni vərəq";
        plusButton.UseVisualStyleBackColor = false;
        plusButton.Click += PlusClicked;
        tabStrip.Controls.Add(plusButton);

        webPanel.Name = "webPanel";
        webPanel.Location = new Point(10, 99);
        webPanel.Size = new Size(670, 455);
        webPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        webPanel.BackColor = Color.FromArgb(43, 108, 176);
        webPanel.Padding = new Padding(3);
        browserPanel.Controls.Add(webPanel);

        sidePanel.Name = "sidePanel";
        sidePanel.Location = new Point(739, 59);
        sidePanel.Size = new Size(236, 565);
        sidePanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        sidePanel.BackColor = Color.FromArgb(247, 250, 252);
        sidePanel.Paint += PaintPanelBorder;

        favoritesLabel.Name = "favoritesLabel";
        favoritesLabel.Location = new Point(12, 8);
        favoritesLabel.Size = new Size(212, 20);
        favoritesLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        favoritesLabel.Text = "Seçilmişlər (ListBox)";
        sidePanel.Controls.Add(favoritesLabel);

        historyLabel.Name = "historyLabel";
        historyLabel.Location = new Point(12, 166);
        historyLabel.Size = new Size(212, 20);
        historyLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        historyLabel.Text = "Tarixçə (ListBox)";
        sidePanel.Controls.Add(historyLabel);

        favoritesList.Name = "favoritesList";
        favoritesList.Location = new Point(12, 38);
        favoritesList.Size = new Size(212, 108);
        favoritesList.AccessibleName = "Seçilmişlər";
        favoritesList.BackColor = Color.FromArgb(247, 250, 252);
        favoritesList.BorderStyle = BorderStyle.None;
        favoritesList.DrawMode = DrawMode.OwnerDrawFixed;
        favoritesList.IntegralHeight = false;
        favoritesList.ItemHeight = 27;
        favoritesList.DrawItem += DrawListEntry;
        favoritesList.DoubleClick += OpenListAddress;
        favoritesList.KeyDown += ListKeyDown;
        sidePanel.Controls.Add(favoritesList);

        historyList.Name = "historyList";
        historyList.Location = new Point(12, 197);
        historyList.Size = new Size(212, 81);
        historyList.AccessibleName = "Tarixçə";
        historyList.BackColor = Color.FromArgb(247, 250, 252);
        historyList.BorderStyle = BorderStyle.None;
        historyList.DrawMode = DrawMode.OwnerDrawFixed;
        historyList.IntegralHeight = false;
        historyList.ItemHeight = 27;
        historyList.DrawItem += DrawListEntry;
        historyList.DoubleClick += OpenListAddress;
        historyList.KeyDown += ListKeyDown;
        sidePanel.Controls.Add(historyList);

        addFavoriteButton.Name = "addFavoriteButton";
        addFavoriteButton.Location = new Point(12, 294);
        addFavoriteButton.Size = new Size(213, 30);
        addFavoriteButton.BackColor = Color.FromArgb(254, 252, 191);
        addFavoriteButton.FlatStyle = FlatStyle.Flat;
        addFavoriteButton.FlatAppearance.BorderColor = Color.FromArgb(157, 164, 172);
        addFavoriteButton.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        addFavoriteButton.Text = "Seçilmişlərə əlavə et";
        addFavoriteButton.UseVisualStyleBackColor = false;
        addFavoriteButton.Click += AddFavoriteClicked;
        sidePanel.Controls.Add(addFavoriteButton);

        statusLabel.Name = "statusLabel";
        statusLabel.Location = new Point(33, 635);
        statusLabel.Size = new Size(942, 24);
        statusLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        statusLabel.AutoEllipsis = true;
        statusLabel.ForeColor = Color.FromArgb(74, 85, 104);

        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(233, 237, 242);
        ClientSize = new Size(1005, 673);
        Font = new Font("Segoe UI", 9F);
        ForeColor = Color.FromArgb(26, 32, 44);
        FormBorderStyle = FormBorderStyle.None;
        MinimumSize = new Size(750, 470);
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Mini Brauzer (WinForms + WebView2)";
        Controls.Add(heading);
        Controls.Add(browserPanel);
        Controls.Add(sidePanel);
        Controls.Add(statusLabel);
        sidePanel.ResumeLayout(false);
        webPanel.ResumeLayout(false);
        tabStrip.ResumeLayout(false);
        addressFrame.ResumeLayout(false);
        titlePanel.ResumeLayout(false);
        browserPanel.ResumeLayout(false);
        addressFrame.PerformLayout();
        ResumeLayout(false);
    }
}
