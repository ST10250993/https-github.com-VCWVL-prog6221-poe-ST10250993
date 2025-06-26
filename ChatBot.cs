using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProgPart3
{
    public class ChatBot
    {
        private Dictionary<string, List<string>> responses;
        private Dictionary<string, string> sentimentResponses;
        private Dictionary<string, string[]> keywordSynonyms; // synonym -> canonical keyword map
        private Random random;

        private string lastTopicKey = null;   // remembers last topic
        private int lastResponseIndex = -1;   // remembers last response index for topic

        public ChatBot()
        {
            random = new Random();

            // Map synonyms and variants to canonical keywords
            keywordSynonyms = new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", new[] { "password", "passcode", "login key" } },
                { "phishing", new[] { "phishing", "scam", "fake email" } },
                { "safe browsing", new[] { "safe browsing", "secure browsing" } },
                { "antivirus", new[] { "antivirus", "anti-virus", "virus protection" } },
                { "firewall", new[] { "firewall", "fire wall" } },
                { "vpn", new[] { "vpn", "virtual private network" } },
                { "2fa", new[] { "2fa", "two-factor authentication", "two factor authentication" } },
                { "social media", new[] { "social media", "social networking" } },
                { "malware", new[] { "malware", "malicious software" } },
                { "ransomware", new[] { "ransomware" } },
                { "updates", new[] { "updates", "software update", "patch" } },
                { "public wifi", new[] { "public wifi", "public wireless", "public wifi network" } },
                { "backups", new[] { "backups", "data backup" } },
                { "email security", new[] { "email security", "email protection", "secure email" } },
                { "encryption", new[] { "encryption", "encrypt" } }
            };

            sentimentResponses = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "worried", "I understand you're feeling worried. Cybersecurity can be intimidating, but I'm here to guide you through it!" },
                { "curious", "Curiosity is great! Ask away — I love helping people learn about cybersecurity." },
                { "frustrated", "It's okay to feel frustrated. Let's take it step by step. What would you like help with?" },
                { "happy", "I'm glad you're feeling positive! Let's keep that energy while learning cybersecurity." },
                { "angry", "I understand your frustration. Let's work together to solve your concerns." },
                { "anxious", "It's okay to feel anxious. I'm here to support you through your cybersecurity questions." }
            };

            responses = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                { "password", new List<string> {
                    "Use long, complex passwords with a mix of letters, numbers, and symbols.",
                    "Avoid using the same password for multiple accounts and enable 2FA where possible.",
                    "Change your passwords regularly and don't share them with anyone."
                }},
                { "phishing", new List<string> {
                    "Phishing emails often pretend to be from trusted sources. Always verify sender details.",
                    "Never click on suspicious links. Check the email domain and watch for red flags.",
                    "If something feels off about an email, it’s best to ignore or report it."
                }},
                { "safe browsing", new List<string> {
                    "Use secure, HTTPS-enabled websites and avoid downloading unknown files.",
                    "Install browser extensions that block tracking and malicious scripts.",
                    "Keep your browser up to date to avoid vulnerabilities."
                }},
                { "antivirus", new List<string> {
                    "Install reputable antivirus software and keep it updated.",
                    "Run regular scans to detect and remove threats.",
                    "Antivirus software is your first line of defense. Use it wisely!"
                }},
                { "firewall", new List<string> {
                    "A firewall helps block unauthorized access. Keep it enabled at all times.",
                    "Configure your firewall to monitor both inbound and outbound traffic.",
                    "Firewalls act like gatekeepers — never disable them unless necessary."
                }},
                { "vpn", new List<string> {
                    "A VPN encrypts your internet connection, improving privacy.",
                    "Use a VPN on public Wi-Fi to protect your data from hackers.",
                    "A good VPN keeps your IP and identity safe from surveillance."
                }},
                { "2fa", new List<string> {
                    "Two-Factor Authentication adds a second layer of protection.",
                    "Use 2FA apps like Google Authenticator instead of SMS when possible.",
                    "Even if someone steals your password, 2FA keeps them out."
                }},
                { "social media", new List<string> {
                    "Limit personal info shared online. Set profiles to private.",
                    "Regularly review and update your social media privacy settings.",
                    "Avoid clicking links from unknown accounts or messages."
                }},
                { "malware", new List<string> {
                    "Avoid downloading attachments or clicking links from unknown sources.",
                    "Install anti-malware tools and keep your system updated.",
                    "Be cautious when installing free software — it may include malware."
                }},
                { "ransomware", new List<string> {
                    "Backup your data regularly to recover from ransomware attacks.",
                    "Avoid opening unexpected email attachments. Use up-to-date security tools.",
                    "Never pay the ransom — report the incident to authorities instead."
                }},
                { "updates", new List<string> {
                    "Keep your operating system and software up to date.",
                    "Install patches as soon as they are released to avoid exploits.",
                    "Updates often fix security flaws. Don’t delay them."
                }},
                { "public wifi", new List<string> {
                    "Avoid logging into sensitive accounts on public Wi-Fi.",
                    "If necessary, use a trusted VPN for a secure connection.",
                    "Public Wi-Fi can be a trap. Turn off auto-connect features."
                }},
                { "backups", new List<string> {
                    "Regular backups can save your data during a cyberattack.",
                    "Use both cloud and local backups for best protection.",
                    "Test your backups occasionally to make sure they work."
                }},
                { "email security", new List<string> {
                    "Don't open attachments from unknown senders.",
                    "Use spam filters and mark suspicious messages as junk.",
                    "Always double-check email addresses before responding."
                }},
                { "encryption", new List<string> {
                    "Encryption keeps your data unreadable to intruders.",
                    "Use end-to-end encrypted apps for secure communication.",
                    "Encrypt your devices and drives, especially laptops."
                }}
            };
        }

        public string GetResponseAsText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please enter a valid question or topic.";

            string userInput = Preprocess(input);

            // Phrases that mean "tell me more"
            var moreRequests = new HashSet<string> { "more", "tell me more", "another", "more info", "more information" };

            if (moreRequests.Contains(userInput))
            {
                // User wants more info about last topic
                if (lastTopicKey != null && responses.ContainsKey(lastTopicKey))
                {
                    var topicResponses = responses[lastTopicKey];
                    lastResponseIndex = (lastResponseIndex + 1) % topicResponses.Count;
                    return topicResponses[lastResponseIndex];
                }
                else
                {
                    return "I don't know which topic you'd like more information on. Please ask about a topic first.";
                }
            }

            // Check for hardcoded sentiment responses
            foreach (var sentiment in sentimentResponses)
            {
                if (ContainsWholeWord(userInput, sentiment.Key.ToLower()))
                {
                    lastTopicKey = null;
                    lastResponseIndex = -1;
                    return sentiment.Value;
                }
            }

            // Topic match using synonyms and whole word/phrase matching
            foreach (var pair in keywordSynonyms)
            {
                string canonicalKey = pair.Key;
                string[] synonyms = pair.Value;

                foreach (string synonym in synonyms)
                {
                    if (ContainsWholeWord(userInput, synonym.ToLower()))
                    {
                        if (responses.ContainsKey(canonicalKey))
                        {
                            var replyOptions = responses[canonicalKey];
                            int index;
                            if (replyOptions.Count == 1)
                            {
                                index = 0;
                            }
                            else
                            {
                                do
                                {
                                    index = random.Next(replyOptions.Count);
                                } while (index == lastResponseIndex);
                            }
                            lastTopicKey = canonicalKey;
                            lastResponseIndex = index;
                            return replyOptions[index];
                        }
                    }
                }
            }

            // Try detecting sentiment as fallback
            string detectedSentiment = DetectSentiment(userInput);
            if (sentimentResponses.TryGetValue(detectedSentiment, out string sentimentReply))
            {
                return sentimentReply;
            }

            // No matches found
            lastTopicKey = null;
            lastResponseIndex = -1;
            return "I'm sorry, I didn't quite understand that. Could you try asking in a different way or about another cybersecurity topic?";
        }

        private string Preprocess(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "";

            // Lowercase and remove punctuation (keep spaces)
            string lower = input.ToLowerInvariant();
            string cleaned = Regex.Replace(lower, @"[^\w\s]", " ");
            // Normalize multiple spaces to one
            cleaned = Regex.Replace(cleaned, @"\s+", " ").Trim();
            return cleaned;
        }

        // Checks if the text contains the whole word/phrase (with word boundaries)
        private bool ContainsWholeWord(string text, string word)
        {
            // \b works for word boundaries
            return Regex.IsMatch(text, $@"\b{Regex.Escape(word)}\b", RegexOptions.IgnoreCase);
        }

        private string DetectSentiment(string input)
        {
            input = input.ToLower();

            string[] positiveWords = { "good", "great", "excellent", "happy", "love", "awesome", "cool", "secure", "safe" };
            string[] negativeWords = { "bad", "terrible", "sad", "angry", "hate", "worried", "frustrated", "anxious", "scared" };

            int positiveScore = positiveWords.Count(word => input.Contains(word));
            int negativeScore = negativeWords.Count(word => input.Contains(word));

            if (positiveScore > negativeScore) return "happy";
            if (negativeScore > positiveScore)
            {
                if (input.Contains("worried")) return "worried";
                if (input.Contains("frustrated")) return "frustrated";
                if (input.Contains("anxious")) return "anxious";
                if (input.Contains("angry")) return "angry";
                return "worried"; // fallback negative tone
            }

            return "curious"; // neutral fallback
        }
    }
}
