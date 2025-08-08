using System.Text.RegularExpressions;
using ClipboardMasking.Win.Data;
using System;
using System.Collections.Generic;

namespace ClipboardMasking.Win.Clipboard
{
    public class ClipboardAnonymizer
    {
        // Precompiled regexes with timeouts to avoid excessive backtracking or ReDoS
        private static readonly Regex Ipv4Regex = new Regex(
            @"\b(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\b",
            RegexOptions.Compiled | RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(250));

        private static readonly Regex EmailRegex = new Regex(
            @"[A-Z0-9a-z._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250));

        private static readonly Regex PhoneRegex = new Regex(
            @"\b(?:\+?1[-.]?)?\(?([0-9]{3})\)?[-.]?([0-9]{3})[-.]?([0-9]{4})\b",
            RegexOptions.Compiled | RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(250));

        // Match candidate card numbers (13-19 digits allowing spaces or dashes). We'll validate via Luhn to reduce false positives
        private static readonly Regex CreditCardRegex = new Regex(
            @"\b(?:\d[ -]?){13,19}\b",
            RegexOptions.Compiled | RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(250));

        private static readonly Regex SsnRegex = new Regex(
            @"\b\d{3}-\d{2}-\d{4}\b",
            RegexOptions.Compiled | RegexOptions.CultureInvariant,
            TimeSpan.FromMilliseconds(250));

        private static readonly Regex UrlRegex = new Regex(
            @"https?://[^\s]+",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
            TimeSpan.FromMilliseconds(250));

        public string Anonymize(string text, AppSettings settings)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            string result = text;

            if (settings.MaskIPAddresses)
            {
                result = SafeReplace(Ipv4Regex, result, "[IP_ADDRESS]");
            }

            if (settings.MaskEmails)
            {
                result = SafeReplace(EmailRegex, result, "[EMAIL]");
            }

            if (settings.MaskPhoneNumbers)
            {
                result = SafeReplace(PhoneRegex, result, "[PHONE]");
            }

            if (settings.MaskCreditCards)
            {
                result = ReplaceCreditCards(result);
            }

            if (settings.MaskSSN)
            {
                result = SafeReplace(SsnRegex, result, "[SSN]");
            }

            if (settings.MaskURLs)
            {
                result = SafeReplace(UrlRegex, result, "[URL]");
            }

            if (settings.MaskNames && settings.CustomNames.Count > 0)
            {
                // Build a single combined regex for all names to reduce multiple passes
                // Use word boundaries and ignore case
                var escapedNames = new List<string>(capacity: settings.CustomNames.Count);
                foreach (var name in settings.CustomNames)
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        escapedNames.Add(Regex.Escape(name));
                    }
                }

                if (escapedNames.Count > 0)
                {
                    var combinedPattern = $"\\b(?:{string.Join("|", escapedNames)})\\b";
                    var namesRegex = new Regex(
                        combinedPattern,
                        RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
                        TimeSpan.FromMilliseconds(250));

                    result = SafeReplace(namesRegex, result, "[NAME]");
                }
            }

            if (settings.CustomPatterns.Count > 0)
            {
                foreach (var pattern in settings.CustomPatterns)
                {
                    if (pattern == null || !pattern.IsEnabled) continue;

                    try
                    {
                        // Compile user pattern with a timeout; ignore case as before
                        var userRegex = new Regex(
                            pattern.Pattern,
                            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase,
                            TimeSpan.FromMilliseconds(250));

                        result = SafeReplace(userRegex, result, pattern.Replacement);
                    }
                    catch (ArgumentException)
                    {
                        // Ignore invalid regex patterns
                    }
                }
            }

            return result;
        }

        private static string SafeReplace(Regex regex, string input, string replacement)
        {
            try
            {
                return regex.Replace(input, replacement);
            }
            catch (RegexMatchTimeoutException)
            {
                // If a timeout occurs, return the input unchanged to avoid blocking
                return input;
            }
        }

        private static string ReplaceCreditCards(string input)
        {
            try
            {
                return CreditCardRegex.Replace(input, match =>
                {
                    var digitsOnly = new string(match.Value.Where(char.IsDigit).ToArray());
                    if (digitsOnly.Length < 13 || digitsOnly.Length > 19)
                    {
                        return match.Value;
                    }

                    // Common test numbers often show up; still mask them
                    return IsLuhnValid(digitsOnly) ? "[CARD_NUMBER]" : match.Value;
                });
            }
            catch (RegexMatchTimeoutException)
            {
                return input;
            }
        }

        private static bool IsLuhnValid(string digits)
        {
            int sum = 0;
            bool doubleDigit = false;
            for (int i = digits.Length - 1; i >= 0; i--)
            {
                int d = digits[i] - '0';
                if (doubleDigit)
                {
                    d *= 2;
                    if (d > 9) d -= 9;
                }
                sum += d;
                doubleDigit = !doubleDigit;
            }
            return sum % 10 == 0;
        }
    }
} 