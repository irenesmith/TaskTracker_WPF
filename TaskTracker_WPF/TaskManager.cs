using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_WPF;

public class TaskManager
{
    readonly string connectionString = "mongodb://localhost:27017";
    readonly string databaseName = "task_tracker";
    readonly string collectionName = "tasks";

}
