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
using AppFabric.Persistence.Framework.Model;

namespace AppFabric.Persistence.Model
{
    public class ProjectState : PersistentState
    {
        public ProjectState(Guid id, string name, string code, decimal budget, DateTime startDate, Guid clientId, string owner, string orderNumber, int status, byte[] rowVersion)
        :base(startDate, rowVersion)
        {
            Id = id;
            ClientId = clientId;
            Name = name;
            Code = code;
            StartDate = startDate;
            Budget = budget;
            Owner = owner;
            Status = status;
            OrderNumber = orderNumber;
        }

        public Guid Id { get; set; }
        public Guid ClientId { get; set;}
        public string Name { get; set;}
        public string Code { get; set;}
        public DateTime StartDate { get; set;}
        public decimal Budget { get; set;}
        
        public string Owner { get; set;}
        
        public string OrderNumber { get; set;}
        
        public int Status { get; set;}
    }
}