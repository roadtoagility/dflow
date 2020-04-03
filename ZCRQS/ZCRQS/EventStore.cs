using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Core.Shared;
using Core.Shared.Base;
using Core.Shared.Interfaces;

namespace Program
{
    public class EventStore : IEventStore<Guid>
    {
        readonly BinaryFormatter _formatter = new BinaryFormatter();
        readonly IAppendOnlyStore<Guid> _appendOnlyStore;
        readonly IQueueService _queueService;
        
        public EventStore(IAppendOnlyStore<Guid> appendOnlyStore, IQueueService queueService)
        {
            _appendOnlyStore = appendOnlyStore;
            _queueService = queueService;
        }
        
        
        
        public EventStream LoadEventStream(Guid id) => LoadEventStream(id, 0, Int32.MaxValue);
        
        public EventStream LoadEventStream(Guid id, int skipEvents, int maxCount)
        {
            var records = _appendOnlyStore.ReadRecords(id, skipEvents, maxCount).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {
                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                stream.Version = tapeRecord.Version;
            }

            return stream;
        }

        public EventStream LoadEventStreamAfterVersion(Guid id, long afterVersion)
        {
            //TODO: acabei zoando muito essa questão de long e int, tudo precisa ser long
            var records = _appendOnlyStore.ReadRecords(id, afterVersion, Int32.MaxValue).ToList();

            var stream = new EventStream();

            foreach (var tapeRecord in records)
            {
                stream.Events.AddRange(DeserializeEvent(tapeRecord.Data));
                stream.Version = tapeRecord.Version;
            }

            return stream;
        }

        //TODO: preciso ver o controle de versão melhor, ta muito manual ainda
        public void AppendToStream<TType>(Guid id, long version, ICollection<IEvent> events)
        {
            if (events.Count == 0)
                return;

            var data = SerializeEvent(events.ToArray());
            var aggregateType = typeof(TType).Name;
            
            //TODO: Ainda não entendi como se controla a versao corretamente
            version++;
            
            try
            {
                _appendOnlyStore.Append(id, aggregateType, data, version);
                
                //TODO: entender MUITO BEM questões de rollback, garantias, oq acontece se da erro no appendOnly?
                _queueService.Publish(events.ToArray());
            }
            //TODO: ainda preciso entender como vai ser o tratamento de exceptions
            catch(Exception ex)
            {
                var server = LoadEventStream(id, 0, int.MaxValue);

                throw;

            }
        }
        
        byte[] SerializeEvent(IEvent[] e)
        {
            using (var mem = new MemoryStream())
            {
                _formatter.Serialize(mem, e);
                return mem.ToArray();
            }
        }
        
        IEvent[] DeserializeEvent(byte[] data)
        {
            using (var mem = new MemoryStream(data))
            {
                return (IEvent[])_formatter.Deserialize(mem);
            }
        }
    }
}