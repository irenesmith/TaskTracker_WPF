using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace TaskTracker_WPF
{
    public class Task
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsDone { get; set; }
    }
}
