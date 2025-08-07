using System.IO;
using System.Text.Json;
using ClipboardMasking.Win.Data;

namespace ClipboardMasking.Win.Services
{
    public class SettingsService
    {
        private readonly string _settingsPath;

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
        }
    }
} 