using System;
using System.IO;
using System.Media;
using System.Windows;

namespace ProgPart3
{
    public class Design
    {
        private SoundPlayer player;

        public void PlayVoiceGreeting()
        {
            try
            {
                string baseDir = AppDomain.CurrentDomain.BaseDirectory;
                string audioPath = Path.Combine(baseDir, "progpart1.wav");

                if (!File.Exists(audioPath))
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
                MessageBox.Show($"Audio error: {ex.Message}", "Playback Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string GetAsciiArt()
        {
            return @"
@""══════════════════════════════════════════════════════════════╗
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
        }
    }
}
