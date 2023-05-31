using System;
using System.Windows;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace TaskTracker_WPF
{
    public partial class MainWindow : Window
    {
        private readonly IMongoCollection<Task> collection;

        public MainWindow()
        {
            InitializeComponent();

            string connectionString = "mongodb://localhost:27017";
            string databaseName = "task_tracker";
            string collectionName = "tasks";

            var client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(databaseName);
            collection = database.GetCollection<Task>(collectionName);

            RefreshTasks();
        }

        private void RefreshTasks()
        {
            var filter = Builders<Task>.Filter.Empty;
            var tasks = collection.Find(filter).ToList();

            dataGridTasks.ItemsSource = tasks;
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
            RefreshTasks();

            textBoxTask.Text = string.Empty;
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTasks.SelectedItem is Task selectedTask)
            {
                collection.DeleteOne(t => t.Id == selectedTask.Id);
                RefreshTasks();
            }
        }

        private void ButtonUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTasks.SelectedItem is Task selectedTask)
            {
                string newDescription = textBoxTask.Text;
                selectedTask.Description = newDescription;
                collection.ReplaceOne(t => t.Id == selectedTask.Id, selectedTask);
                RefreshTasks();
                textBoxTask.Text = string.Empty;
            }
        }

        private void ButtonMarkDone_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridTasks.SelectedItem is Task selectedTask)
            {
                selectedTask.IsDone = true;
                collection.ReplaceOne(t => t.Id == selectedTask.Id, selectedTask);
                RefreshTasks();
            }
        }
    }
}
