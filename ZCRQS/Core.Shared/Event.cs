using System;

namespace Core.Shared
{
    public class Event
    {
        public Guid IdEntity { get; private set; }
        public string Name { get; private set; }
        public object Entity { get; private set; }

        public Event(Guid id, string name, object entity)
        {
            Name = name;
            Entity = entity;
            IdEntity = id;
        }
    }
}