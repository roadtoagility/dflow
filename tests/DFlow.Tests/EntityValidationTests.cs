using DFlow.Domain.BusinessObjects;
using DFlow.Tests.Supporting.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DFlow.Tests
{
    public class EntityValidationTests
    {
        [Fact]
        public void EntityShouldBeInvalid()
        {
            //var obj = BusinessEntity.From(EntityTestId.GetNext(), Version.New());

            //Assert.True(obj.Errors.Any());
            //Assert.True(obj.IsValid);
            //Assert.Equal("email", obj.Errors[0].Source);
            //Assert.Equal("Email shoud not be null", obj.Errors[0].Message);


            FrotaAgregation frotaAgg = new FrotaAgregation();
            frotaAgg.Attach(new Veiculo("123456789"));
            frotaAgg.AddValidations();

            Assert.True(frotaAgg.Veiculos[0].Errors.Any());
            Assert.False(frotaAgg.Veiculos[0].IsValid);
            Assert.Equal("chassi", frotaAgg.Veiculos[0].Errors[0].Source);
            Assert.Equal("chassi inválido", frotaAgg.Veiculos[0].Errors[0].Message);

            //Assert.True(frotaAgg.Errors.Any());
            //Assert.False(frotaAgg.IsValid);
            //Assert.Equal("chassi", frotaAgg.Errors[0].Source);
            //Assert.Equal("chassi inválido", frotaAgg.Errors[0].Message);
        }
    }

    public static class FrotaValidations
    {
        public static void AddValidations(this FrotaAgregation frota) //Element<Veiculo>
        {
            frota.Accept(new ChassiValidator());
        }
    }

    public class ChassiValidator : IValidatable<Veiculo>
    {
        public void Validate(Veiculo veiculo)
        {
            if (veiculo == null)
            {
                veiculo.AddError(new Error("veículo", "veículo nulo"));
            }

            veiculo.AddError(new Error("chassi", "chassi inválido"));
        }
    }

    public class Veiculo : Element<Veiculo>
    {
        public string Chassi { get; private set; }

        public Veiculo(string chassi)
        {
            Chassi = chassi;
        }

        public override void Validate(IValidatable<Veiculo> validation)
        {
            validation.Validate(this);
        }
    }

    public class FrotaAgregation {

        public List<Veiculo> Veiculos { get; private set; } = new List<Veiculo>();

        public void Attach(Veiculo veiculo)
        {
            Veiculos.Add(veiculo);
        }

        public void Detach(Veiculo veiculo)
        {
            Veiculos.Remove(veiculo);
        }

        public void Accept(IValidatable<Veiculo> validation)
        {
            foreach(var veiculo in Veiculos)
            {
                veiculo.Validate(validation);
            }
        }
    }

    public interface IValidatable<T>
    {
        void Validate(T element); // void Visit(Element element);
    }

    public abstract class Element<T>
    {
        public List<Error> Errors { get; private set; } = new List<Error>();
        public bool IsValid { get { return !Errors.Any(); } }
        public abstract void Validate(IValidatable<T> visitor); // Accept

        public void AddError(Error error)
        {
            Errors.Add(error);
        }
    }

    public class Error
    {
        public string Source { get; private set; }
        public string Message { get; private set; }

        public Error(string source, string message)
        {
            Source = source;
            Message = message;
        }
    }
}
