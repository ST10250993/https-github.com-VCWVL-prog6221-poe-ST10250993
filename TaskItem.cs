namespace ProgPart3
{
    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Reminder { get; set; }
        public bool IsComplete { get; set; }

        // Display string for ListBox
        public string DisplayText => $"{(IsComplete ? "[✔]" : "[ ]")} {Title} - {Reminder:g}";
    }
}
