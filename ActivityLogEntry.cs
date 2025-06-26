using System;

namespace ProgPart3
{
    public class ActivityLogEntry
    {
        public DateTime Timestamp { get; set; }
        public string ActionType { get; set; }
        public string Description { get; set; }

        public ActivityLogEntry(string actionType, string description)
        {
            Timestamp = DateTime.Now;
            ActionType = actionType ?? "Unknown";
            Description = description ?? "No description";
        }

        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] {ActionType}: {Description}";
        }
    }
}
