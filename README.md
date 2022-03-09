<center><img src="docs/img/logodflow_200x200.png" width="350" /></center>

# DFlow - Domain Events Flow

Domain events Flow is a platform to support development with agility in mind based on Domain-Driven Design tools.

## Project Informations
[![GitHub issues](https://img.shields.io/github/issues/roadtoagility/dflow)](https://img.shields.io/github/issues/roadtoagility/dflow)
[![GitHub stars](https://img.shields.io/github/stars/roadtoagility/dflow)](https://github.com/roadtoagility/dflow/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/roadtoagility/dflow)](https://github.com/roadtoagility/dflow/network)
[![GitHub license](https://img.shields.io/github/license/roadtoagility/dflow)](https://github.com/roadtoagility/dflow/blob/master/LICENSE.TXT)

## Code Quality
[![codecov](https://codecov.io/gh/roadtoagility/dflow/branch/develop/graph/badge.svg?token=5I6T20JZC8)](https://codecov.io/gh/roadtoagility/dflow)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/008ea00243504ac5ab31a24ebed9e5e8)](https://www.codacy.com/gh/roadtoagility/dflow/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=roadtoagility/dflow&amp;utm_campaign=Badge_Grade)

## Build Status

[![DFlow AppVeyor Build](https://ci.appveyor.com/api/projects/status/481jwy9pnyh0fv41/branch/master?svg=true)](https://ci.appveyor.com/project/drr00t/dflow/branch/master)

## Tests Passing
![AppVeyor tests](https://img.shields.io/appveyor/tests/drr00t/dfow)

## Nuget packages from DFlow Platform
Core Packages|Description|Dependencies|Latest Version|
-|-|-|-|
DFlow| First implementaion with full event sourcing aggregation support|none|![DFlow NuGet version](https://img.shields.io/nuget/v/DFlow.svg)|
DFlow.Domain|Base framewor to rich domain modelling based on Object Value, Entity and Aggregates design patterns|- DFlow.Domain.Events|![DFlow NuGet version](https://img.shields.io/nuget/v/DFlow.Domain.svg)|
DFlow.Domain.Events|Base framework that provides capability to declare events raise from Aggregates|none|![DFlow NuGet version](https://img.shields.io/nuget/v/DFlow.Domain.Events.svg)|
DFlow.Business.Cqrs|CQRS framework to implement command and query handlers event and event publishing as well|- DFlow.Domain </br>- DFlow.Domain.Events </br>- DFlow.Domain.EventBus.(*)</br>|![DFlow NuGet version](https://img.shields.io/nuget/v/DFlow.Business.Cqrs.svg)|
DFlow.Persistence|Base framework providing an Unity of Work and Repository design patterns to support persistence layer implementation|- DFlow.Domain </br>|![DFlow NuGet version](https://img.shields.io/nuget/v/DFlow.Persistence.svg)|

Contrib Pastyle="ckages|Description|Dependencies|Latest Version|
-|-|-|-|
DFlow.Persistence.EntityFramework|Specialization of DFlow.Persistence to support EntityFramework based persistence layers abstraction|- DFlow.Domain </br>- DFlow.Domain.Persistence </br>|![DFlow NuGet version](https://img.shields.io/nuget/v/DFlow.Persistence.EntityFramework.svg)|
DFlow.Domain.EventBus.FluentMediator|Implementation of IDomainEventBus to be used to DomainEvents publishing |- DFlow.Domain.Events </br>|![DFlow NuGet version](https://img.shields.io/nuget/v/DFlow.Domain.EventBus.FluentMediator.svg)|
DFlow.Persistence.LiteDB|Specialization of DFlow.Persistence to support LiteDB |- DFlow.Domain </br>- DFlow.Domain.Persistence </br>|![DFlow NuGet version](https://img.shields.io/nuget/v/DFlow.Persistence.LiteDB.svg)|

## About Project
DFlow was born as a reference implementation of Domain-Drive Design architecture based with aggregates supporting Event Sourcing design pattern but evolves to a full flagged platform to helps you to implement your own framework based on DFlow Domain-Driven concepts supported by Domain-Driven Design tools.

Do you have a lot of flexibility to evolve your applications. Each platform library enforce just few concepts used by each specific layer, do you pay for what you use but you can implement your view of DFlow interfaces and evolve as your desire.

As an example, we provide contrib packages like **Dflow.Persistence.EntityFramework** that enables you to use ORM based approach to your persistence layer using EntityFramework with any database driver that you want and DFlow.EventBus.FluentMediator that is a interface to used the excellent framework FluentMediator.

## Documentation
For now we implement few example projects that you can look at in [samples](https://github.com/roadtoagility/dflow/tree/master/samples) folder

Sample|Description|Link|
-|-|-|
SimplestApp| It is a basicv application using **DFlow.Domain** library|[SimplestApp](https://github.com/roadtoagility/dflow/tree/master/samples/SimplestApp)|
SimplestApp.Business.Cqrs| It is a basic implementation of CQRS-base application using **DFlow.Business.Cqrs** library |[SimplestApp.Business.Cqrs](https://github.com/roadtoagility/dflow/tree/master/samples/SimplestApp.Business.Cqrs)|
SimplestApp.Persistence.EntityFramework|It is a basic application using all libraries of DFlow platform for CQRS + DDD + ORM|[SimplestApp.Persistence.EntityFramework](https://github.com/roadtoagility/dflow/tree/master/samples/SimplestApp.Persistence.EntityFramework)|

## Roadmap
  * Finish support to Event Sourcing in Persistence Layer
  * Test a 100% of code
  * Finish the performance tests support
  * Improve documentation
  * much more...

## Supporters

| Supporter | Description |      |
| ---- | ----- | ----------- |
| <img src="img/jetbrains-variant-4.png" alt="./img/" width="300px" /> | All Products Pack License for Open Source under program [**Free License Programs**](https://www.jetbrains.com/community/education/) |             |

### Thank You for all **supporters** of this project

## License

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.

