using System.Collections.Generic;

namespace ClipboardMasking.Win.Data
{
    public class AppSettings
    {
        public bool MaskIPAddresses { get; set; } = true;
        public bool MaskEmails { get; set; } = true;
        public bool MaskPhoneNumbers { get; set; } = true;
        public bool MaskCreditCards { get; set; } = true;
        public bool MaskSSN { get; set; } = true;
        public bool MaskNames { get; set; } = true;
        public bool MaskURLs { get; set; } = false;
        public List<string> CustomNames { get; set; } = new List<string>();
        public List<CustomPattern> CustomPatterns { get; set; } = new List<CustomPattern>();
        public bool StartOnLaunch { get; set; } = true;
        public bool RunOnStartup { get; set; } = false;
    }
} 