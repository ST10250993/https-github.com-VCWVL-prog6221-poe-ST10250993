using System.Collections.Generic;
using System.Windows;

namespace ProgPart3
{
    public partial class ActivityLogWindow : Window
    {
        private readonly ActivityLogManager _logManager;
        private int _currentIndex = 0;
        private const int PageSize = 10;

        public ActivityLogWindow(ActivityLogManager logManager)
        {
            InitializeComponent();
            _logManager = logManager;
            LoadNextEntries();
        }

        private void LoadNextEntries()
        {
            List<ActivityLogEntry> entries = _logManager.GetEntries(_currentIndex, PageSize);

            foreach (var entry in entries)
            {
                LogListBox.Items.Add(entry.ToString());
            }

            _currentIndex += entries.Count;

            // Hide Load More if no more entries
            LoadMoreButton.Visibility = (_currentIndex >= _logManager.Count) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void LoadMoreButton_Click(object sender, RoutedEventArgs e)
        {
            LoadNextEntries();
        }
    }
}
