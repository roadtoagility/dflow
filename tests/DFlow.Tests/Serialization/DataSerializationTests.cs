using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DFlow.Domain.Events;
using DFlow.Tests.Supporting.DomainObjects;
using DFlow.Tests.Supporting.DomainObjects.Events;
using DFlow.Tests.Supporting.ExtensionMethods;
using Xunit;
using Version = DFlow.Domain.BusinessObjects.Version;

namespace DFlow.Tests.Serialization
{
    public class DataSerializationTests
    {
        [Fact]
        public void ShouldCreateAggregateWithVersionCorrectly()
        {
            var businessEntity = BusinessEntity.New();
            var eventData = EntityForSerializationAddedEvent.For(businessEntity);  
            var data = eventData.SerializeData();
            var eventAgain = (EntityForSerializationAddedEvent)data.DeserializeData();
            
            Assert.Equal(eventData.Id, eventAgain.Id);
            Assert.Equal(eventData.When, eventAgain.When);
            Assert.Equal(eventData.Version, eventAgain.Version);
        }


        ArraySegment<byte> SerializeData(IDomainEvent evt)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var mem = new MemoryStream())
            {
                formatter.Serialize(mem, evt);
                return mem.ToArray();
            }
        }
        
        IDomainEvent DeserializeData(ArraySegment<byte> data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var mem = new MemoryStream(data.ToArray()))
            {
                return (IDomainEvent)formatter.Deserialize(mem);
            }
        }
    }
}