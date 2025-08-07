using System.Text.RegularExpressions;
using ClipboardMasking.Win.Data;

namespace ClipboardMasking.Win.Clipboard
{
    public class ClipboardAnonymizer
    {
        public string Anonymize(string text, AppSettings settings)
        {
            var result = text;
            if (settings.MaskIPAddresses)
            {
                result = Regex.Replace(result, @"\b(?:\d{1,3}\.){3}\d{1,3}\b", "[IP_ADDRESS]");
            }
            if (settings.MaskEmails)
            {
                result = Regex.Replace(result, @"[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}", "[EMAIL]", RegexOptions.IgnoreCase);
            }
            if (settings.MaskPhoneNumbers)
            {
                result = Regex.Replace(result, @"\b(?:\+?1[-.]?)?\(?([0-9]{3})\)?[-.]?([0-9]{3})[-.]?([0-9]{4})\b", "[PHONE]");
            }
            if (settings.MaskCreditCards)
            {
                result = Regex.Replace(result, @"\b(?:\d[ -]?){13,16}\b", "[CARD_NUMBER]");
            }
            if (settings.MaskSSN)
            {
                result = Regex.Replace(result, @"\b\d{3}-\d{2}-\d{4}\b", "[SSN]");
            }
            if (settings.MaskURLs)
            {
                 result = Regex.Replace(result, @"https?://[^\s]+", "[URL]", RegexOptions.IgnoreCase);
            }
            if (settings.MaskNames)
            {
                foreach (var name in settings.CustomNames)
                {
                    result = Regex.Replace(result, $@"\b{Regex.Escape(name)}\b", "[NAME]", RegexOptions.IgnoreCase);
                }
            }
            foreach (var pattern in settings.CustomPatterns.Where(p => p.IsEnabled))
            {
                try
                {
                    result = Regex.Replace(result, pattern.Pattern, pattern.Replacement, RegexOptions.IgnoreCase);
                }
                catch (ArgumentException)
                {
                    // Ignore invalid regex patterns
                }
            }
            return result;
        }
    }
} 