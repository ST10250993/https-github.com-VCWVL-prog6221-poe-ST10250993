using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProgPart3
{
    public partial class QuizWindow : Window
    {
        private readonly List<Question> questions;
        private int currentQuestionIndex = 0;
        private int score = 0;

        public QuizWindow()
        {
            InitializeComponent();

            // Initialize questions
            questions = GetQuizQuestions();

            DisplayCurrentQuestion();
        }

        private void DisplayCurrentQuestion()
        {
            FeedbackTextBlock.Text = "";
            SubmitButton.IsEnabled = true;
            NextButton.Visibility = Visibility.Collapsed;
            OptionsPanel.Children.Clear();

            if (currentQuestionIndex >= questions.Count)
            {
                ShowFinalScore();
                return;
            }

            var question = questions[currentQuestionIndex];
            QuestionTextBlock.Text = $"Q{currentQuestionIndex + 1}: {question.Text}";

            // Create radio buttons for options
            for (int i = 0; i < question.Options.Count; i++)
            {
                var rb = new RadioButton
                {
                    Content = question.Options[i],
                    Foreground = new SolidColorBrush(Colors.LightGreen),
                    FontFamily = new System.Windows.Media.FontFamily("Consolas"),
                    FontSize = 14,
                    Margin = new Thickness(5)
                };
                OptionsPanel.Children.Add(rb);
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = -1;

            // Find which radio button is selected
            for (int i = 0; i < OptionsPanel.Children.Count; i++)
            {
                if (OptionsPanel.Children[i] is RadioButton rb && rb.IsChecked == true)
                {
                    selectedIndex = i;
                    break;
                }
            }

            if (selectedIndex == -1)
            {
                MessageBox.Show("Please select an answer before submitting.", "No Answer Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SubmitButton.IsEnabled = false;

            var question = questions[currentQuestionIndex];

            bool isCorrect = selectedIndex == question.CorrectOptionIndex;

            if (isCorrect)
            {
                score++;
                FeedbackTextBlock.Foreground = new SolidColorBrush(Colors.LightGreen);
                FeedbackTextBlock.Text = $"Correct! 🎉\n{question.Explanation}";
            }
            else
            {
                FeedbackTextBlock.Foreground = new SolidColorBrush(Colors.OrangeRed);
                FeedbackTextBlock.Text = $"Incorrect. ❌\n{question.Explanation}";
            }

            NextButton.Visibility = Visibility.Visible;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            currentQuestionIndex++;
            if (currentQuestionIndex < questions.Count)
            {
                DisplayCurrentQuestion();
            }
            else
            {
                ShowFinalScore();
            }
        }

        private void ShowFinalScore()
        {
            QuestionTextBlock.Text = $"Quiz Completed! Your Score: {score} / {questions.Count}";

            string feedback = score switch
            {
                var s when s == questions.Count => "Excellent! You know your cybersecurity very well! 🏆",
                var s when s >= questions.Count * 0.7 => "Good job! You have a solid understanding of cybersecurity.",
                var s when s >= questions.Count * 0.4 => "Not bad! There's room for improvement.",
                _ => "Keep learning! Cybersecurity is important for everyone."
            };

            FeedbackTextBlock.Text = feedback;

            OptionsPanel.Children.Clear();
            SubmitButton.Visibility = Visibility.Collapsed;
            NextButton.Visibility = Visibility.Collapsed;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        // Hardcoded sample questions (minimum 10) for demo
        private List<Question> GetQuizQuestions()
        {
            return new List<Question>
            {
                new Question {
                    Text = "Phishing attacks are attempts to steal your personal information by pretending to be a trustworthy entity.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 0,
                    Explanation = "Phishing is a social engineering attack often via email pretending to be someone you trust.",
                    IsTrueFalse = true
                },
                new Question {
                    Text = "Which of these is NOT a strong password practice?",
                    Options = new List<string> {
                        "Using a mix of letters, numbers, and symbols",
                        "Using the same password for multiple accounts",
                        "Changing passwords regularly"
                    },
                    CorrectOptionIndex = 1,
                    Explanation = "Reusing passwords across accounts is risky because if one is compromised, all are at risk.",
                    IsTrueFalse = false
                },
                new Question {
                    Text = "Two-factor authentication (2FA) adds an extra layer of security beyond just a password.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 0,
                    Explanation = "2FA requires something you know (password) plus something you have or are (phone, fingerprint).",
                    IsTrueFalse = true
                },
                new Question {
                    Text = "Public Wi-Fi networks are always safe to use for banking or shopping online.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "Public Wi-Fi is often insecure and can be exploited by attackers.",
                    IsTrueFalse = true
                },
                new Question {
                    Text = "A firewall is used to:",
                    Options = new List<string> {
                        "Block unauthorized access to your network",
                        "Increase internet speed",
                        "Store backup data"
                    },
                    CorrectOptionIndex = 0,
                    Explanation = "Firewalls monitor and control incoming and outgoing network traffic based on security rules.",
                    IsTrueFalse = false
                },
                new Question {
                    Text = "Malware is:",
                    Options = new List<string> {
                        "A type of antivirus software",
                        "Malicious software designed to harm or exploit devices",
                        "A hardware device"
                    },
                    CorrectOptionIndex = 1,
                    Explanation = "Malware includes viruses, trojans, ransomware and other harmful software.",
                    IsTrueFalse = false
                },
                new Question {
                    Text = "You should always update your software because:",
                    Options = new List<string> {
                        "Updates often fix security vulnerabilities",
                        "Updates slow down your device",
                        "Updates delete your files"
                    },
                    CorrectOptionIndex = 0,
                    Explanation = "Keeping software up to date protects against known exploits and bugs.",
                    IsTrueFalse = false
                },
                new Question {
                    Text = "Encryption makes data unreadable to unauthorized users.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 0,
                    Explanation = "Encryption scrambles data so only authorized parties can read it.",
                    IsTrueFalse = true
                },
                new Question {
                    Text = "Ransomware demands payment to:",
                    Options = new List<string> {
                        "Restore access to your encrypted files",
                        "Install antivirus software",
                        "Improve system speed"
                    },
                    CorrectOptionIndex = 0,
                    Explanation = "Ransomware encrypts your data and demands money to decrypt it.",
                    IsTrueFalse = false
                },
                new Question {
                    Text = "Using the same password across multiple sites is a good security practice.",
                    Options = new List<string> { "True", "False" },
                    CorrectOptionIndex = 1,
                    Explanation = "Reusing passwords makes it easier for attackers to compromise multiple accounts.",
                    IsTrueFalse = true
                }
            };
        }
    }
}
