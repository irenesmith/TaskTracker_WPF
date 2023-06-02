using System.Windows;
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

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            // Don't add a new task if the text box is empty.
            if (string.IsNullOrEmpty(textBoxTask.Text)) { return; }

            // If we got here, the text box contains text so we can
            // create a new task.
            string description = textBoxTask.Text;
            var newTask = new Task
            {
                Description = description,
                Timestamp = DateTime.Now,
                IsDone = false
            };
            collection.InsertOne(newTask);
            dataGridTasks.ItemsSource = taskManager.RefreshTasks();

            // Clear the contents of the text box.
            textBoxTask.Text = string.Empty;
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

        private void dataGridTasks_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dataGridTasks.SelectedItem is Task selectedTask)
            {
                textBoxTask.Text = selectedTask.Description;
            }
        }
    }
}
