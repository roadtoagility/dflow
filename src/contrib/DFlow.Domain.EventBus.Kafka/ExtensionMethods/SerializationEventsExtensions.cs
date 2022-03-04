// Copyright (C) 2020  Road to Agility
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Library General Public
// License as published by the Free Software Foundation; either
// version 2 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Library General Public License for more details.
//
// You should have received a copy of the GNU Library General Public
// License along with this library; if not, write to the
// Free Software Foundation, Inc., 51 Franklin St, Fifth Floor,
// Boston, MA  02110-1301, USA.
//


using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using DFlow.Domain.Events;

namespace DFlow.Domain.EventBus.Kafka.ExtensionMethods
{
    public static class SerializationEventsExtensions
    {
        public static ArraySegment<byte> SerializeData(this IDomainEvent evt)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using var mem = new MemoryStream();
            formatter.Serialize(mem, evt);
            return mem.ToArray();
        }
        
        public static IDomainEvent DeserializeData(this ArraySegment<byte> data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using var mem = new MemoryStream(data.ToArray());
            return (IDomainEvent)formatter.Deserialize(mem);
        }
    }
}