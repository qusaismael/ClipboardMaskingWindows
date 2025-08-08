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
            try
            {
                InitializeComponent();
                _settingsService = new SettingsService();
                _anonymizer = new ClipboardAnonymizer();
                _settings = _settingsService.LoadSettings();

                // Setup NotifyIcon (System Tray Icon) with more visible icon
                _notifyIcon = new NotifyIcon
                {
                    Icon = SystemIcons.Information, // Use Information icon which is more visible
                    Visible = true,
                    Text = "Clipboard Masking - Double click to toggle"
                };
                
                // Add double-click handler to toggle monitoring
                _notifyIcon.DoubleClick += ToggleMonitoring_Click;

                var contextMenu = new ContextMenuStrip();
                contextMenu.Items.Add("▶ Start Monitoring", null, ToggleMonitoring_Click);
                contextMenu.Items.Add("Settings...", null, Settings_Click);
                contextMenu.Items.Add("-");
                contextMenu.Items.Add("Show Status", null, ShowStatus_Click);
                contextMenu.Items.Add("Test Functionality", null, TestFunctionality_Click);
                contextMenu.Items.Add("Quit", null, Quit_Click);
                _notifyIcon.ContextMenuStrip = contextMenu;
                
                UpdateMenuState();

                // Show initial notification to confirm app is running
                _notifyIcon.ShowBalloonTip(5000, "Clipboard Masking", "Application started! Look for the info icon in system tray.", ToolTipIcon.Info);
                
                // Start monitoring honoring user preference
                if (_settings.StartOnLaunch)
                {
                    StartMonitoring();
                }

                // Apply RunOnStartup preference
                try
                {
                    _settingsService.ApplyRunOnStartup(_settings.RunOnStartup);
                }
                catch { }
                
                // Show a message box to confirm the app is running
                MessageBox.Show("Clipboard Masking Application Started!\n\nLook for the system tray icon.\nCopy 'test@example.com' and paste it to test.", "Application Started", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting application: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TestFunctionality_Click(object? sender, EventArgs e)
        {
            try
            {
                // Test the anonymizer directly
                string testEmail = "test@example.com";
                string result = _anonymizer.Anonymize(testEmail, _settings);
                
                string message = $"Test Results:\nOriginal: {testEmail}\nResult: {result}\nMonitoring: {_isMonitoring}\nMask Count: {_maskCount}";
                MessageBox.Show(message, "Functionality Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Test failed: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowStatus_Click(object? sender, EventArgs e)
        {
            string status = _isMonitoring ? "ACTIVE" : "INACTIVE";
            MessageBox.Show($"Clipboard Masking Status: {status}\nMasked items: {_maskCount}", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void StartMonitoring()
        {
            try
            {
                if (_isMonitoring) return;
                _clipboardMonitor = new ClipboardMonitor();
                _clipboardMonitor.ClipboardUpdate += OnClipboardUpdate;
                _isMonitoring = true;
                UpdateMenuState();
                
                // Show notification that monitoring has started
                _notifyIcon.ShowBalloonTip(3000, "Clipboard Masking", "Monitoring started! Copy sensitive data to test.", ToolTipIcon.Info);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting monitoring: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StopMonitoring()
        {
            try
            {
                if (!_isMonitoring) return;
                _clipboardMonitor?.StopMonitoring();
                _clipboardMonitor = null;
                _isMonitoring = false;
                UpdateMenuState();
                
                // Show notification that monitoring has stopped
                _notifyIcon.ShowBalloonTip(2000, "Clipboard Masking", "Monitoring paused.", ToolTipIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping monitoring: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OnClipboardUpdate(object? sender, EventArgs e)
        {
            try
            {
                if (!System.Windows.Forms.Clipboard.ContainsText()) return;

                // Guard against re-entrancy by avoiding processing when we just set the clipboard
                string clipboardText = System.Windows.Forms.Clipboard.GetText();
                string anonymizedText = _anonymizer.Anonymize(clipboardText, _settings);

                if (!string.Equals(clipboardText, anonymizedText, StringComparison.Ordinal))
                {
                    _lastMaskedContent = clipboardText;
                    _maskCount++;
                    System.Windows.Forms.Clipboard.SetText(anonymizedText);
                    UpdateMenuState(true);
                }
            }
            catch (Exception ex)
            {
                // Log error but don't show message box to avoid spam
                Debug.WriteLine($"Clipboard update error: {ex.Message}");
            }
        }
        
        private void UpdateMenuState(bool justMasked = false)
        {
            try
            {
                if (_notifyIcon.ContextMenuStrip == null) return;
                
                var toggleItem = _notifyIcon.ContextMenuStrip.Items[0];
                toggleItem.Text = _isMonitoring ? "⏸ Pause Monitoring" : "▶ Start Monitoring";
                
                if (_isMonitoring)
                {
                    _notifyIcon.Icon = SystemIcons.Asterisk; // Use asterisk icon which is very visible
                    _notifyIcon.Text = $"Clipboard Masking (ACTIVE) - Masked: {_maskCount}";
                }
                else
                {
                     _notifyIcon.Icon = SystemIcons.Information; // Use information icon for inactive
                    _notifyIcon.Text = "Clipboard Masking (INACTIVE)";
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
            catch (Exception ex)
            {
                Debug.WriteLine($"Update menu state error: {ex.Message}");
            }
        }

        private void Restore_Click(object? sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error restoring content: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleMonitoring_Click(object? sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error toggling monitoring: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Settings_Click(object? sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Quit_Click(object? sender, EventArgs e)
        {
            try
            {
                StopMonitoring();
                _notifyIcon.Dispose();
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error quitting: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                base.OnLoad(e);
                // Make the form hidden
                Visible = false;
                ShowInTaskbar = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                StopMonitoring();
                base.OnFormClosing(e);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Form closing error: {ex.Message}");
            }
        }
    }
}