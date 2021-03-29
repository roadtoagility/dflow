namespace DFlow.Persistence.EventStore.Model
{
    public class EventData<TKey>
    {
        public EventData(TKey aggregateId, string aggregateType, long version, byte[] data)
        {
            AggregateId = aggregateId;
            AggregateType = aggregateType;
            Version = version;
            Data = data;
        }

        public TKey AggregateId { get; set; }
        public string AggregateType { get; set; }
        public long Version { get; set; }
        public byte[] Data { get; set; }
    }
}