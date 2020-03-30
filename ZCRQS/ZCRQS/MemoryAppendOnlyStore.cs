using System;
using System.Collections.Generic;
using System.Linq;
using Core.Shared;

namespace Program
{
    public class MemoryAppendOnlyStore : IAppendOnlyStore
    {
        //Aqui seria a infra que insere no banco
        private ICollection<EventDTO> EventsStorage = new List<EventDTO>();
        
        public void Dispose()
        {
            EventsStorage = new List<EventDTO>();
        }

        public void Append(string name, byte[] data, int expectedVersion = -1)
        {
            EventsStorage.Add(new EventDTO(name, expectedVersion, data));
        }

        public IEnumerable<DataWithVersion> ReadRecords(string name, int afterVersion, int maxCount)
        {
            return EventsStorage
                .Where(x => x.Name.Equals(name) && x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithVersion(x.Version, x.Data));
        }

        public IEnumerable<DataWithName> ReadRecords(int afterVersion, int maxCount)
        {
            return EventsStorage
                .Where(x => x.Version > afterVersion)
                .Take(maxCount)
                .Select(x => new DataWithName(x.Name, x.Data));
        }

        public void Close()
        {
            EventsStorage = null;
        }

        //DTO para poder inserir em qualquer modelo de banco
        private class EventDTO
        {
            public string Name { get; set; }
            public int Version { get; set; }
            public byte[] Data { get; set; }

            public EventDTO(string name, int version, byte[] data)
            {
                Name = name;
                Version = version;
                Data = data;
            }
        }
    }
}