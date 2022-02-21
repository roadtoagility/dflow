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

namespace AppFabric.Persistence.ReadModel
{
    public class UserProjection
    {
        public UserProjection()
        {
        }
        public UserProjection(Guid id, string name, string cnpj, string commercialEmail, int rowVersion)
        {
            Id = id;
            Name = name;
            Cnpj = cnpj;
            CommercialEmail = commercialEmail;
            IsDeleted = false;
            RowVersion = rowVersion;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Cnpj { get; set; }
        public string CommercialEmail { get; set; }
        
        public bool IsDeleted { get; set; }

        public int RowVersion { get; set; }
        
        public static UserProjection Empty()
        {
            return new UserProjection(Guid.Empty, String.Empty, String.Empty, String.Empty,0);
        }
    }
}