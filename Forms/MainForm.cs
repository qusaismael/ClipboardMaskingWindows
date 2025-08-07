using ClipboardMasking.Win.Clipboard;
using ClipboardMasking.Win.Data;
using ClipboardMasking.Win.Services;
using System.Diagnostics;

namespace ClipboardMasking.Win.Forms
{
    public partial class MainForm : Form
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly SettingsService _settingsService;
        private readonly ClipboardAnonymizer _anonymizer;
        private AppSettings _settings;
        private ClipboardMonitor? _clipboardMonitor;

        private bool _isMonitoring = false;
        private int _maskCount = 0;
        private string _lastMaskedContent = string.Empty;

        public MainForm()
        {
            InitializeComponent();
            _settingsService = new SettingsService();
            _anonymizer = new ClipboardAnonymizer();
            _settings = _settingsService.LoadSettings();

            // Setup NotifyIcon (System Tray Icon)
            _notifyIcon = new NotifyIcon
            {
                Icon = SystemIcons.Shield, // Replace with your icon
                Visible = true,
                Text = "Clipboard Masking"
            };

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Start Monitoring", null, ToggleMonitoring_Click);
            contextMenu.Items.Add("Settings...", null, Settings_Click);
            contextMenu.Items.Add("-");
            contextMenu.Items.Add("Quit", null, Quit_Click);
            _notifyIcon.ContextMenuStrip = contextMenu;
            
            UpdateMenuState();

            if (_settings.StartOnLaunch)
            {
                StartMonitoring();
            }
        }

        private void StartMonitoring()
        {
            if (_isMonitoring) return;
            _clipboardMonitor = new ClipboardMonitor();
            _clipboardMonitor.ClipboardUpdate += OnClipboardUpdate;
            _isMonitoring = true;
            UpdateMenuState();
        }

        private void StopMonitoring()
        {
            if (!_isMonitoring) return;
            _clipboardMonitor?.StopMonitoring();
            _clipboardMonitor = null;
            _isMonitoring = false;
            UpdateMenuState();
        }

        private void OnClipboardUpdate(object? sender, EventArgs e)
        {
            if (!System.Windows.Forms.Clipboard.ContainsText()) return;

            string clipboardText = System.Windows.Forms.Clipboard.GetText();
            string anonymizedText = _anonymizer.Anonymize(clipboardText, _settings);

            if (clipboardText != anonymizedText)
            {
                _lastMaskedContent = clipboardText;
                _maskCount++;
                System.Windows.Forms.Clipboard.SetText(anonymizedText);
                UpdateMenuState(true);
            }
        }
        
        private void UpdateMenuState(bool justMasked = false)
        {
            if (_notifyIcon.ContextMenuStrip == null) return;
            
            var toggleItem = _notifyIcon.ContextMenuStrip.Items[0];
            toggleItem.Text = _isMonitoring ? "Pause Monitoring" : "Start Monitoring";
            
            if (_isMonitoring)
            {
                _notifyIcon.Icon = new Icon(SystemIcons.Shield, 48, 48); // A green icon would be better
                _notifyIcon.Text = $"Clipboard Masking (Active) - Masked: {_maskCount}";
            }
            else
            {
                 _notifyIcon.Icon = SystemIcons.Warning; // A gray icon would be better
                _notifyIcon.Text = "Clipboard Masking (Inactive)";
            }

            // Add/Remove Restore Item
            var restoreItem = _notifyIcon.ContextMenuStrip.Items.Cast<ToolStripItem>().FirstOrDefault(i => i.Name == "RestoreItem");
            if (!string.IsNullOrEmpty(_lastMaskedContent))
            {
                if (restoreItem == null)
                {
                    restoreItem = new ToolStripMenuItem("Restore Last Content", null, Restore_Click) { Name = "RestoreItem" };
                    _notifyIcon.ContextMenuStrip.Items.Insert(1, restoreItem);
                }
            }
            else
            {
                if (restoreItem != null)
                {
                    _notifyIcon.ContextMenuStrip.Items.Remove(restoreItem);
                }
            }

            if (justMasked)
            {
                _notifyIcon.ShowBalloonTip(1000, "Clipboard Masked", $"Sensitive data was masked. Total: {_maskCount}", ToolTipIcon.Info);
            }
        }

        private void Restore_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_lastMaskedContent))
            {
                // Temporarily stop monitoring to prevent re-masking
                bool wasMonitoring = _isMonitoring;
                if (wasMonitoring) StopMonitoring();
                
                System.Windows.Forms.Clipboard.SetText(_lastMaskedContent);
                _lastMaskedContent = string.Empty;
                UpdateMenuState();

                if(wasMonitoring) StartMonitoring();
            }
        }

        private void ToggleMonitoring_Click(object? sender, EventArgs e)
        {
            if (_isMonitoring)
            {
                StopMonitoring();
            }
            else
            {
                StartMonitoring();
            }
        }

        private void Settings_Click(object? sender, EventArgs e)
        {
            // Pause monitoring while settings are open
            bool wasMonitoring = _isMonitoring;
            if (wasMonitoring) StopMonitoring();

            using (var settingsForm = new SettingsForm(_settings, _settingsService))
            {
                settingsForm.ShowDialog();
                // Settings are saved within the form, just reload them
                _settings = _settingsService.LoadSettings();
            }
            
            if (wasMonitoring) StartMonitoring();
        }

        private void Quit_Click(object? sender, EventArgs e)
        {
            StopMonitoring();
            _notifyIcon.Dispose();
            Application.Exit();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            // Make the form hidden
            Visible = false;
            ShowInTaskbar = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopMonitoring();
            base.OnFormClosing(e);
        }
    }
}