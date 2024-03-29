﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using MongoDB.Driver;

namespace TaskTracker_WPF
{
    public partial class MainWindow : Window
    {
        private readonly IMongoCollection<Task> collection;
        private readonly TaskManager taskManager;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "mongodb://localhost:27017";
            string databaseName = "task_tracker";
            string collectionName = "tasks";

            var client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(databaseName);
            collection = database.GetCollection<Task>(collectionName);

            taskManager = new TaskManager();

            dataGridTasks.ItemsSource = taskManager.RefreshTasks();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            ((HwndSource)PresentationSource.FromVisual(this)).AddHook(HookProc);
        }

        private IntPtr HookProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            if (msg == 0x0084 /*WM_NCHITTEST*/ )
            {
                // This prevents a crash in WindowChromeWorker._HandleNCHitTest
                try
                {
                    lParam.ToInt32();
                }
                catch (OverflowException)
                {
                    handled = true;
                }
            }
            return IntPtr.Zero;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Don't add a new task if the text box is empty.
            // if (string.IsNullOrEmpty(textBoxTask.Text)) { return; }

            var newTaskDialog = new NewTaskDialog
            {
                Owner = this
            };

            var dlgResult = newTaskDialog.ShowDialog();
            if (dlgResult == true)
            {
                // If we got here, the text box contains text so we can
                // create a new task.
                var newTask = newTaskDialog.NewTask;
                collection.InsertOne(newTask);
                dataGridTasks.ItemsSource = taskManager.RefreshTasks();
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTasks.SelectedItem is Task selectedTask)
            {
                taskManager.DeleteTask(selectedTask);
                dataGridTasks.ItemsSource = taskManager.RefreshTasks();
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTasks.SelectedItem is Task selectedTask)
            {
                string newDescription = textBoxTask.Text;
                selectedTask.Description = newDescription;

                taskManager.UpdateTask(selectedTask);
                dataGridTasks.ItemsSource = taskManager.RefreshTasks();

                // Clear the contents of the text box.
                textBoxTask.Text = string.Empty;
            }
        }

        private void ButtonMarkDone_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTasks.SelectedItem is Task selectedTask)
            {
                selectedTask.IsDone = true;
                taskManager.UpdateTask(selectedTask);
                dataGridTasks.ItemsSource = taskManager.RefreshTasks();
            }
        }

        private void DataGridTasks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dataGridTasks.SelectedItem is Task selectedTask)
            {
                textBoxTask.Text = selectedTask.Description;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.IsLoaded && checkBox.DataContext is Task task)
            {
                task.IsDone = checkBox.IsChecked ?? false;

                // Simulated method to update the task in the database
                taskManager.UpdateTask(task);
                dataGridTasks.ItemsSource = taskManager.RefreshTasks();
            }
        }
    }
}
