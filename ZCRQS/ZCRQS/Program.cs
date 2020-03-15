using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using Xunit;

namespace ZCRQS
{   
    
    
    
    public class UpdateModelTests
    {
        private Guid IdPessoa;
        private MemoryWriteRepository _memoryWriteModel;
        private MemoryReadModel _memoryReadModel;
        private GetPessoaByCpfQuery _queryByCpf;

        public UpdateModelTests()
        {
            _memoryWriteModel = new MemoryWriteRepository();
            _memoryReadModel = new MemoryReadModel();
            _queryByCpf = new GetPessoaByCpfQuery(_memoryReadModel);
            
            IdPessoa = Guid.NewGuid();
            
            var updateCpfCommand = new AtualizarCpfCommand(_memoryWriteModel);
            updateCpfCommand.Execute(new ChangeCpfParams("11111111111", IdPessoa));
        }
        
        

        [Fact]
        public void ShouldUpdateAggregate()
        {
            //var memoryWriteModel = new MemoryWriteRepository();
            var updateCpfCommand = new AtualizarCpfCommand(_memoryWriteModel);
            
            updateCpfCommand.Execute(new ChangeCpfParams("02866783140", IdPessoa));

            var pessoa = _queryByCpf.Query(new GetPessoaByCpfParam("02866783140"));
            
            Assert.Equal("02866783140", pessoa.Cpf);
        }
    }

    public sealed class EventStream
    {
        private List<IObserverStream> _observers = new List<IObserverStream>();
        
        private static readonly Lazy<EventStream>
            lazy =
                new Lazy<EventStream>
                    (() => new EventStream());

        public static EventStream Instance { get { return lazy.Value; } }

        private EventStream()
        {
        }
        
        public void Subscribe(IObserverStream observer)
        {
            _observers.Add(observer);
        }
        
        public void Raise(params Event[] events)
        {
            foreach (var @eventUnit in events)
            {
                foreach (var observer in _observers)
                {
                    observer.Raise(eventUnit);
                }
            }
        }
    }

    public interface IObserverStream
    {
        void Raise(Event eventUnit);
    }

    public class AtualizarCpfCommand
    {
        private IWriteRepository<PessoaAggregate> _repository;

        public AtualizarCpfCommand(IWriteRepository<PessoaAggregate> repository)
        {
            _repository = repository;
        }
        
        public void Execute(ChangeCpfParams param)
        {
            var pessoaAggregate = new PessoaAggregate(param.Id);
            pessoaAggregate.UpdateCpf(param.Cpf);
            _repository.Update(pessoaAggregate);
        }
    }

    public interface IWriteRepository<T>
    {
        void Update<U>(U entity);
    }

    public class MemoryWriteRepository : IWriteRepository<PessoaAggregate>
    {
        private IList<PessoaAggregate> pessoasDatabase = new List<PessoaAggregate>();
        public void Update<T>(T entity)
        {
            var pessoaAggregate = entity as PessoaAggregate;
            var pessoa = pessoasDatabase.Where(x => x.Id == pessoaAggregate.Id).SingleOrDefault();

            if (pessoa == null)
                pessoasDatabase.Add(pessoaAggregate);
            else
            {
                pessoa.Update(pessoaAggregate);
            }

            EventStream.Instance.Raise(pessoaAggregate.Events.ToArray());
        }
    }

    public class Event
    {
        public string Name { get; private set; }
        public object Entity { get; private set; }

        public Event(string name, object entity)
        {
            Name = name;
            Entity = entity;
        }
    }

    public class PessoaAggregate
    {
        public string Cpf { get; private set; }
        public Guid Id { get; private set; }

        public IList<Event> Events { get; private set; }

        public PessoaAggregate(Guid id)
        {
            this.Id = id;
            Events = new List<Event>();
        }

        public void UpdateCpf(string cpf)
        {
            this.Cpf = cpf;
            this.Events.Add(new Event("CpfUpdated", this));
        }

        public void Update(PessoaAggregate pessoa)
        {
            this.Cpf = pessoa.Cpf;
            this.Id = pessoa.Id;
            this.Events.Add(new Event("FullPessoaUpdated", pessoa));
        }
    }

    public class MemoryReadModel : IObserverStream, IReadModel<PessoaDTO, string>
    {
        private IList<PessoaAggregate> pessoasDatabase = new List<PessoaAggregate>();

        public MemoryReadModel()
        {
            EventStream.Instance.Subscribe(this);
        }
        
        public PessoaDTO Get(string param)
        {
            var pessoa = pessoasDatabase.Where(x => x.Cpf.Equals(param)).FirstOrDefault();

            if (pessoa != null)
            {
                return new PessoaDTO()
                {
                    Cpf = pessoa.Cpf
                };
            }
            else
                return null;
        }
        

        public void Raise(Event eventUnit)
        {
            if (eventUnit.Entity is PessoaAggregate)
            {
                var aggregate = eventUnit.Entity as PessoaAggregate;
                var pessoa = pessoasDatabase.Where(x => x.Id == aggregate.Id).SingleOrDefault();
                
                if (pessoa == null)
                    pessoasDatabase.Add(aggregate);
                else
                {
                    pessoa.Update(pessoa);
                }
            }
            else
            {
                
            }
            
            
        }
    }

    public class GetPessoaByCpfQuery
    {
        private IReadModel<PessoaDTO, string> _readOnlyModel;
        public GetPessoaByCpfQuery(IReadModel<PessoaDTO, string> readOnlyModel)
        {
            _readOnlyModel = readOnlyModel;
        }
        
        public PessoaDTO Query(GetPessoaByCpfParam query)
        {
            var pessoaDto = _readOnlyModel.Get(query.Cpf);
            return pessoaDto;
        }
    }

    public interface IReadModel<T, U>
    {
        T Get(U param);
    }

    public class GetPessoaByCpfParam
    {
        public string Cpf { get; }

        public GetPessoaByCpfParam(string cpf)
        {
            this.Cpf = cpf;
        }
    }

    public class PessoaDTO
    {
        public string Cpf { get; set; }
        public string Nome { get; set; }
    }

    public class ChangeCpfParams
    {
        public string Cpf { get; }
        public Guid Id { get; }

        public ChangeCpfParams(string cpf, Guid id)
        {
            this.Cpf = cpf;
            this.Id = id;
        }
    }
}