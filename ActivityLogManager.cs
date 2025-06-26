using System.Collections.Generic;
using System.Linq;

namespace ProgPart3
{
    public class ActivityLogManager
    {
        private readonly List<ActivityLogEntry> _entries = new();

        // Add new log entry
        public void AddEntry(string actionType, string description)
        {
            var entry = new ActivityLogEntry(actionType, description);
            _entries.Add(entry);
        }

        // Get latest entries in descending order (newest first)
        public List<ActivityLogEntry> GetEntries(int skip = 0, int take = 10)
        {
            return _entries
                .OrderByDescending(e => e.Timestamp)
                .Skip(skip)
                .Take(take)
                .ToList();
        }

        // Total number of log entries
        public int Count => _entries.Count;
    }
}
