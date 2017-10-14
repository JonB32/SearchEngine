using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SearchEngineService.Models
{
    public class Nerd
    {
        // ObjectId needed to query MongoDB collections
        private ObjectId id;
        private string name;

        public Nerd()
        {
        }

        [BsonElement("name")]
        public string Name { get => name; set => name = value; }
        public ObjectId Id { get => id; set => id = value; }
    }
}
