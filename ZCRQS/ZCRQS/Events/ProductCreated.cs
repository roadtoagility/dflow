using System;
using Core.Shared;
using Program.Commands;

namespace Program.Events
{
    public class ProductCreated : IEvent
    {
        public Guid RootId { get; private set; }
        public Guid Id { get; private set; }
        public DateTime Date { get; private set; }

        public CreateProductCommand PayLoad { get; private set; }


        public ProductCreated(Guid rootId, CreateProductCommand cmd)
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;;
            RootId = rootId;
            PayLoad = cmd;
        }
        
        public string GetEventName()
        {
            return "ProductCreated";
        }

        public string GetEntityType()
        {
            return "Product";
        }

        public Guid GetEventId()
        {
            return Id;
        }

        public string GetEventType()
        {
            return "ProductCreated";
        }

        public string GetEventDate()
        {
            return Date.ToString();
        }

        public Guid GetRoot()
        {
            return RootId;
        }

        public string GetEventData()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(PayLoad);
        }
    }
}