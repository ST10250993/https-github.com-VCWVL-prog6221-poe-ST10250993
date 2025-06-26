using System;
using System.Windows;
using Microsoft.VisualBasic; // For InputBox

namespace ProgPart3
{
    public partial class TaskWindow : Window
    {
        private TaskManager taskManager;

        public TaskWindow(TaskManager manager)
        {
            InitializeComponent();
            taskManager = manager;
            RefreshTaskList();
        }

        private void RefreshTaskList()
        {
            TaskListBox.ItemsSource = null;
            TaskListBox.ItemsSource = taskManager.GetAllTasks();
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            string title = Interaction.InputBox("Enter task title:", "New Task");
            if (string.IsNullOrWhiteSpace(title)) return;

            string description = Interaction.InputBox("Enter task description:", "New Task");
            if (string.IsNullOrWhiteSpace(description)) return;

            string reminderInput = Interaction.InputBox("Enter reminder date and time (e.g. 2025-06-30 15:30):", "New Task");
            if (!DateTime.TryParse(reminderInput, out DateTime reminder))
            {
                MessageBox.Show("Invalid date/time format.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            taskManager.AddTask(title, description, reminder);
            RefreshTaskList();
        }

        private void MarkComplete_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem selectedTask)
            {
                taskManager.MarkComplete(selectedTask);
                RefreshTaskList();
            }
            else
            {
                MessageBox.Show("Please select a task to mark as complete.", "No Task Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem selectedTask)
            {
                var result = MessageBox.Show("Are you sure you want to delete this task?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    taskManager.DeleteTask(selectedTask);
                    RefreshTaskList();
                }
            }
            else
            {
                MessageBox.Show("Please select a task to delete.", "No Task Selected", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
