using System;
using System.Media;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace ProgPart3
{
    public partial class MainWindow : Window
    {
        private ChatBot bot;
        private SoundPlayer player;

        // Assuming you have a TaskManager instance somewhere to pass to TaskWindow
        private TaskManager taskManager = new TaskManager();

        public MainWindow()
        {
            InitializeComponent();
            bot = new ChatBot();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Display ASCII art
            AsciiArtBlock.Text =
@"══════════════════════════════════════════════════════════════╗
║                    🤖 CYBERSECURITY BOT 🤖                   |
╚══════════════════════════════════════════════════════════════=╝
                  ╔═[:::: SYSTEM ONLINE ::::]═╗
               ...................................
               .............-^[{{{[>-.............
               .........~{{{{{)*=*){{{{{~.........
               .......]{{(....=@@@=....){{[.......
               .....^{{{{:...@@@@@@@.....*{{>.....
               ....]{{.>{{{:{@@@@@@@{......{{[....
               ...^{]....]{{%@@@@@@@@]......({^...
               ..-{{:.....:{{{)^+^={@@......:{{-..
               ..>{(........*#{{+.>@>........){<..
               ..){*.....~@@@@@{{{@@@@@~.....*{(..
               ..>{(....+@@@@@@@@{{{@@@@*....){<..
               ..-{{:..~@@@@@@@@@@@{{#@@@=..:{{-..
               ...^{]..^@@@@@@@([)@@%{{%@<..({^...
               ....]{{..#@@@@@@##{@@@@#{{<.{{[....
               .....^{{*..:@@@@@@@@@@@=:{{{{>.....
               ~......]{{).............){{[.......
               -........~{{{{{)*=*){{{{{~.........
               =............-^[{{{[>-.............
~..................................
                 ╚════════ CHAT READY ════════╝
";

            ResponseBlock.Document.Blocks.Clear();
            AppendColoredText("Bot: Hello! Ask me anything about cybersecurity.\n", Colors.LightGreen);

            PlayVoiceGreeting();

            // Placeholder visibility on load
            PlaceholderText.Visibility = string.IsNullOrEmpty(InputBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string audioPath = System.IO.Path.Combine(baseDir, "progpart1.wav");

                if (!System.IO.File.Exists(audioPath))
                {
                    MessageBox.Show($"Audio file not found at: {audioPath}", "File Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                player = new SoundPlayer(audioPath);
                player.Load();
                player.Play();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Audio playback failed: {ex.Message}", "Audio Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string userInput = InputBox.Text.Trim();
            if (!string.IsNullOrWhiteSpace(userInput))
            {
                AppendColoredText($"You: {userInput}\n", Colors.LightBlue);

                string response = bot.GetResponseAsText(userInput);

                AppendColoredText($"Bot: {response}\n\n", Colors.LightGreen);

                InputBox.Text = "";
                ResponseBlock.ScrollToEnd();
            }
        }

        private void AppendColoredText(string text, Color color)
        {
            var para = new Paragraph();
            var run = new Run(text) { Foreground = new SolidColorBrush(color) };
            para.Inlines.Add(run);
            ResponseBlock.Document.Blocks.Add(para);
        }

        private void InputBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            PlaceholderText.Visibility = string.IsNullOrEmpty(InputBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OpenQuiz_Click(object sender, RoutedEventArgs e)
        {
            var quizWindow = new QuizWindow
            {
                Owner = this
            };
            quizWindow.ShowDialog();
        }

        private void OpenTasks_Click(object sender, RoutedEventArgs e)
        {
            var taskWindow = new TaskWindow(taskManager)
            {
                Owner = this
            };
            taskWindow.ShowDialog();
        }
    }
}
