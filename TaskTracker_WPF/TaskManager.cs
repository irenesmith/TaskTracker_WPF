using MongoDB.Bson;
using MongoDB.Driver;

namespace TaskTracker_WPF;

public class TaskManager
{
    private readonly IMongoCollection<Task> collection;

    readonly string connectionString = "mongodb://localhost:27017";
    readonly string databaseName = "task_tracker";
    readonly string collectionName = "tasks";

    public TaskManager()
    {
        var client = new MongoClient(connectionString);
        IMongoDatabase database = client.GetDatabase(databaseName);
        collection = database.GetCollection<Task>(collectionName);
    }

    public List<Task> RefreshTasks()
    {
        var filter = Builders<Task>.Filter.Empty;
        var tasks = collection.Find(filter).ToList();

        return (List<Task>)tasks.OrderByDescending(t => t.Timestamp).ToList();
    }

    public void InsertTask(Task newTask)
    {
        collection.InsertOne(newTask);
    }

    public void DeleteTask(Task selectedTask)
    {
        collection.DeleteOne(t => t.Id == selectedTask.Id);
    }

    public void UpdateTask(Task selectedTask)
    {
        collection.ReplaceOne(t => t.Id == selectedTask.Id, selectedTask);
    }
}
