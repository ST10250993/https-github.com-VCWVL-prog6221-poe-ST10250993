using System.Collections.Generic;
using System.Linq;

namespace ProgPart3
{
    public class TaskManager
    {
        private readonly List<TaskItem> tasks = new();

        // Return all tasks ordered: incomplete first, then by reminder date
        public IEnumerable<TaskItem> GetAllTasks() =>
            tasks.OrderBy(t => t.IsComplete).ThenBy(t => t.Reminder);

        public void AddTask(string title, string description, DateTime reminder)
        {
            tasks.Add(new TaskItem
            {
                Title = title,
                Description = description,
                Reminder = reminder,
                IsComplete = false
            });
        }

        public void MarkComplete(TaskItem task)
        {
            var t = tasks.FirstOrDefault(x => x == task);
            if (t != null)
                t.IsComplete = true;
        }

        public void DeleteTask(TaskItem task)
        {
            tasks.Remove(task);
        }
    }
}
