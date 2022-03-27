// Copyright (C) 2021  Road to Agility
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

using DFlow.Business.Cqrs;
using Microsoft.Extensions.DependencyInjection;

namespace DFlow.Business.MessageBus.FluentMediator
{
    public static class BusinessMessageBusContribServiceCollectionExtensions
    {
        public static void AddBusinessMessageBusForFluentMediatr(this IServiceCollection services)
        {
            // message bus implementation
            services
                .AddScoped<IBusinessMessageBus, FluentMediatorBusinessMessageBus>();

        }
    }
}