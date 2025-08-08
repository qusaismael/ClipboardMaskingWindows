using System.IO;
using System.Text.Json;
using ClipboardMasking.Win.Data;
using Microsoft.Win32;
using System.Diagnostics;

namespace ClipboardMasking.Win.Services
{
    public class SettingsService
    {
        private readonly string _settingsPath;
        private const string RunKeyPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string AppRunValueName = "ClipboardMaskingWin";

        public SettingsService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var settingsDir = Path.Combine(appDataPath, "ClipboardMasking");
            Directory.CreateDirectory(settingsDir);
            _settingsPath = Path.Combine(settingsDir, "settings.json");
        }

        public AppSettings LoadSettings()
        {
            if (!File.Exists(_settingsPath))
            {
                return new AppSettings();
            }

            try
            {
                var json = File.ReadAllText(_settingsPath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch
            {
                // In case of corruption, return default settings
                return new AppSettings();
            }
        }

        public void SaveSettings(AppSettings settings)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(settings, options);
            File.WriteAllText(_settingsPath, json);

            // Apply autostart setting on save
            try
            {
                ApplyRunOnStartup(settings.RunOnStartup);
            }
            catch
            {
                // Swallow to avoid breaking settings save; UI could surface errors in future
            }
        }

        public void ApplyRunOnStartup(bool enable)
        {
            // Only meaningful on Windows
            if (!OperatingSystem.IsWindows()) return;

            using var key = Registry.CurrentUser.OpenSubKey(RunKeyPath, writable: true) ??
                            Registry.CurrentUser.CreateSubKey(RunKeyPath, writable: true);
            if (key == null) return;

            if (enable)
            {
                var exePath = Environment.ProcessPath ?? Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
                var quoted = $"\"{exePath}\"";
                key.SetValue(AppRunValueName, quoted);
            }
            else
            {
                if (key.GetValue(AppRunValueName) != null)
                {
                    key.DeleteValue(AppRunValueName, throwOnMissingValue: false);
                }
            }
        }
    }
} 