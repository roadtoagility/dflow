// // Copyright (C) 2023  Road to Agility

using DFlow.Testing.Supporting.DataProviders;
using DFlow.Testing.Supporting.DomainObjects;
using DFlow.Testing.Supporting.DomainObjects.Events;
using DFlow.Testing.Supporting.Persistence;
using Xunit;

namespace DFlow.Testing.Persistence;

public class PrimaryEntityPersistenceTests
{
    [Theory]
    [ClassData(typeof(PrimaryEntityForEventsPersistence))]
    public void PrimaryEntity_Events_Export(SecondaryEntity secondary, SimpleValueObject name, int expected)
    {
        var entity = PrimaryEntity.NewEntity(secondary,name);
        entity.UpdateSecondary(secondary);
        
        var changeSet = new ExportPrimaryEntityEvents().ToOutBox(entity);
        
        Assert.Equal(expected, changeSet.Count);
    }
}