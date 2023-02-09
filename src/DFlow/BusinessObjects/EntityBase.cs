// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.


using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using DFlow.Events;
using DFlow.Validation;

namespace DFlow.BusinessObjects;

public abstract class EntityBase<TIdentity> : BaseValidation,
    IEntityIdentity<TIdentity>, IDomainEvents
{
    private readonly IList<DomainEvent> _events = new List<DomainEvent>();

    protected EntityBase(TIdentity identity, VersionId version)
    {
        Identity = identity;
        Version = version;
    }

    public TIdentity Identity { get; }

    public VersionId Version { get; }

    public bool IsNew() => Version.IsNew;

    public void RaisedEvent(DomainEvent @event)
    {
        this._events.Add(@event);
    }

    public IReadOnlyList<DomainEvent> GetEvents()
    {
        return this._events.ToImmutableList();
    }

    public override string ToString()
    {
        return $"[ENTITY]:[IDENTITY: {Identity}]";
    }

    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var entity = (EntityBase<TIdentity>)obj;

        return GetEqualityComponents().SequenceEqual(entity.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(GetEqualityComponents());
    }
}