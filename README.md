<center><img src="docs/img/logodflow_200x200.png" width="350" /></center>

# DFlow - Domain Flow

It is a set of framework to learn, organize and accelerate development of the heart of software.

## Project Informations
[![GitHub issues](https://img.shields.io/github/issues/roadtoagility/dflow)](https://img.shields.io/github/issues/roadtoagility/dflow)
[![GitHub stars](https://img.shields.io/github/stars/roadtoagility/dflow)](https://github.com/roadtoagility/dflow/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/roadtoagility/dflow)](https://github.com/roadtoagility/dflow/network)
[![GitHub license](https://img.shields.io/github/license/roadtoagility/dflow)](https://github.com/roadtoagility/dflow/blob/master/LICENSE.TXT)

## Code Quality
[![codecov](https://codecov.io/gh/roadtoagility/dflow/branch/develop/graph/badge.svg?token=5I6T20JZC8)](https://codecov.io/gh/roadtoagility/dflow)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/008ea00243504ac5ab31a24ebed9e5e8)](https://www.codacy.com/gh/roadtoagility/dflow/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=roadtoagility/dflow&amp;utm_campaign=Badge_Grade)
[![CodeQL](https://github.com/roadtoagility/dflow/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/roadtoagility/dflow/actions/workflows/codeql-analysis.yml)

## CI Status
[![Build and Testing](https://github.com/roadtoagility/dflow/actions/workflows/dotnet.yml/badge.svg)](https://github.com/roadtoagility/dflow/actions/workflows/dotnet.yml/badge.svg)

## Breaking Changes in 0.6.3 -> v0.8.0

That is a major refactoring of DFlow to be more "friendly" and have consistent programming model.

- DFlow.Business will be maintained any more
- DFlow.Business.Cqrs will be maintained any more
- DFlow.Domain become DFlow (back to origins :)
- DFlow.Specifications become a separated assembly
- BaseEntity -> EntityBase
- All Events must inherits from DomainEvent abstract class
- There a new interface to raise events, so events can be raised from Entities or via Aggregates as you need.
- DFlow.Domain.Events merged with DFlow and had several APIs changes

## About Project

Domain Flow aka **DFlow** is a very light and opnionated set of frameworks to help implement the hearth of applications based on Domain-Driven Design. There are 3 assemblys organized as described bellow:

- **DFlow**: Core assembly providing objets to implement Entities, ValueObjects, Validations and Aggregates;
- **DFlow.Specifications**: It is a Specification Design Pattern implementation;
- **DFlow.Persistence**: This project depends on DFlow because it is responsible to translate from/to domain objects  representation. It is a set os interfaces to materialize domain layer to be persisted in any format that you want. The 3 major patterns implemented are Unity Of Work and Repository.

Addon: DFlow.Persistence.
DFlow as based on clean architecture principles, so domain layer aka Entities are first class citizen.   

## Usage

### Referencing DFlow
```shell

```


### Creating Domain objects

1. The most basic DFlow business object is the **Value Object** implementation that allows you follow the principle of non-primitive obsession for the objects you want. 

```c#
// Value Object
public sealed class Email : ValueOf<string,Email>
{

}
```

2. Defining and Entity

```c#
// Entity
public class User : EntityBase<UserId>
{
    public User(UserId identity, Email mail, VersionId version)
        : base(identity, version)
    {
        Mail = mail;

        AppendValidationResult(identity.ValidationStatus.Failures);
        AppendValidationResult(mail.Failures);
    }

    public Email Mail { get; private set; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Identity;
        yield return Mail;
    }

    // DOmain objects can have static contualized api based on factory methods
    public static PrimaryEntity From(PrimaryEntityId id, SecondaryEntity secondary, SimpleValueObject simpleObject,
        VersionId version)
    {
        return new PrimaryEntity(id, secondary, simpleObject, version);
    }

    // Domain Objects can have instance business methods
    // Entities can raise events
    public void Update(Email mail)
    {
        if (!mail.ValidationStatus.IsValid)
        {
            AppendValidationResult(mail.ValidationStatus.Failures);
        }

        Mail = mail;
        RaisedEvent(UserMailUpdatedEvent.For(this));
    }

}
```
## Getting Help
The best way to get help for Npgsql is to post a question to Stack Overflow and tag it with the npgsql tag. If you think you've encountered a bug or want to request a feature, open an issue in the appropriate project's github repository.

## Contributors
* Adriano Ribeiro [@drr00t](https://github.com/drr00t)
* Douglas José Ramalho Araujo [@dougramalho](https://github.com/dougramalho)
* Marco V Gurrola [@marcovgurrola](https://github.com/marcovgurrola)
* Zama Bandeira Braga [@zamabraga](https://github.com/zamabraga)


## Supporters

| Supporter                                                            | Description                                                                                                                         | 
|----------------------------------------------------------------------|-------------------------------------------------------------------------------------------------------------------------------------|
| <img src="img/jetbrains-variant-4.png" alt="./img/" width="200px" /> | All Products Pack License for Open Source under program [**Free License Programs**](https://www.jetbrains.com/community/education/) |

## Thanks
A special thank you to [Jetbrains](http://jetbrains.com/) for donating licenses to the project.

## License

This Source Code Form is subject to the terms of the Mozilla Public
License, v. 2.0. If a copy of the MPL was not distributed with this
file, You can obtain one at http://mozilla.org/MPL/2.0/.

