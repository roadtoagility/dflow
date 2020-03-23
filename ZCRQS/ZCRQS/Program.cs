using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;
using Core.Shared;
using Xunit;

namespace ZCRQS
{   
    public class UpdateModelTests
    {
        private Guid IdPessoa;
        private MemoryWriteRepository _memoryWriteModel;
        private MemoryReadModel _memoryReadModel;
        private GetPessoaByIdQuery _queryByCpf;

        public UpdateModelTests()
        {
            _memoryWriteModel = new MemoryWriteRepository();
            _memoryReadModel = new MemoryReadModel();
            _queryByCpf = new GetPessoaByIdQuery(_memoryReadModel);
            
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

            var pessoa = _queryByCpf.Query(new GetPessoaByIdParam(IdPessoa));
            
            Assert.Equal("02866783140", pessoa.Cpf);
        }
    }

    public class AtualizarCpfCommand
    {
        private RepositoryBase _repository;
        
        public AtualizarCpfCommand(RepositoryBase repository)
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

    

    public class MemoryWriteRepository : RepositoryBase
    {
        private IList<PessoaAggregate> pessoasDatabase = new List<PessoaAggregate>();

        protected override void OnUpdate<T>(T entity)
        {
            var pessoaAggregate = entity as PessoaAggregate;
            var pessoa = pessoasDatabase.Where(x => x.Id == pessoaAggregate.Id).SingleOrDefault();

            if (pessoa == null)
                pessoasDatabase.Add(pessoaAggregate);
            else
            {
                pessoa.Update(pessoaAggregate);
            }
        }
    }

    public class AggregateFactory
    {
        Dictionary<AggregateType, BaseAggregate<Guid>> factory = new Dictionary<AggregateType, BaseAggregate<Guid>>();

        public AggregateFactory()
        {
            factory.Add(AggregateType.PurchaseOrderAggregate, CreatePurchaseOrderAggregate());
        }
        
        public BaseAggregate<Guid> Create(AggregateType type)
        {
            return factory[type];
        }
        
        private BaseAggregate<Guid> CreatePurchaseOrderAggregate()
        {
            throw new NotImplementedException();
        }
    }

    public enum AggregateType
    {
        PurchaseOrderAggregate
    }

    public enum PurchaseOrderEntitiesType
    {
        PurchaseOrder,
        OrderLineItem,
        Part
    }

    public class PurchaseOrderFactory
    {
        Dictionary<PurchaseOrderEntitiesType, EntityBase<Guid>> factory = new Dictionary<PurchaseOrderEntitiesType, EntityBase<Guid>>();


        public PurchaseOrderFactory()
        {
            factory.Add(PurchaseOrderEntitiesType.Part, CreatePart());
            factory.Add(PurchaseOrderEntitiesType.PurchaseOrder, CreatePurchase());
            factory.Add(PurchaseOrderEntitiesType.OrderLineItem, CreateOrderLineItem());
        }
        
        public EntityBase<Guid> Create(PurchaseOrderEntitiesType type)
        {
            return factory[type];
        }
        
        private EntityBase<Guid> CreatePurchase()
        {
            throw new NotImplementedException();
        }

        private EntityBase<Guid> CreateOrderLineItem()
        {
            throw new NotImplementedException();
        }

        private EntityBase<Guid> CreatePart()
        {
            throw new NotImplementedException();
        }
    }

    public class Pessoa
    {
        public string Nome { get; protected set; }
        public Endereco Endereco { get; set; }
    }

    public class Endereco
    {
        public string Logradouro { get; protected set; }
    }

    public abstract class BaseAggregate<T>
    {
        public T Id;
        public IList<Event> Events { get; private set; }

        public BaseAggregate(T id, IList<Event> events)
        {
            Id = id;
            Events = events;
        }
    }

    public class PurchaseOrderAggregate : BaseAggregate<Guid>
    {
        private PurchaseOrder Order;
        
        public PurchaseOrderAggregate(Guid id, IList<Event> events) : base(id, events)
        {
            
        }
    }

    

    public abstract class EntityBase<T>
    {
        public T RootId { get; private set; }

        public EntityBase(T rootId)
        {
            RootId = rootId;
        }
    }
    
    public class PurchaseOrder : EntityBase<Guid>
    {
        public IList<OrderLineItem> LineItems { get; set; }

        public PurchaseOrder(Guid rootId) : base(rootId)
        {
        }
    }
    
    public class Part : EntityBase<Guid>
    {
        public Part(Guid rootId) : base(rootId)
        {
        }
    }
    
    public class OrderLineItem : EntityBase<Guid>
    {
        public Part Part {get;set;}
        public decimal Quantity { get; set; }

        public OrderLineItem(Guid rootId) : base(rootId)
        {
        }
    }
        

    public class PessoaAggregate : IEventAggregate
    {
        public string Cpf { get; private set; }
        public Guid Id { get; private set; }

        public IList<Event> Events { get; private set; }
        
        public IList<Pessoa> Pessoas { get; private set; }

        public PessoaAggregate(Guid id)
        {
            this.Id = id;
            Events = new List<Event>();
        }

        public void UpdateCpf(string cpf)
        {
            this.Cpf = cpf;
            this.Events.Add(new Event(this.Id, "CpfUpdated", this));
        }

        public void Update(PessoaAggregate pessoa)
        {
            this.Cpf = pessoa.Cpf;
            this.Id = pessoa.Id;
            this.Events.Add(new Event(this.Id, "FullPessoaUpdated", pessoa));
        }

        public Event[] GetEvents()
        {
            return Events.ToArray();
        }
    }

    public class MemoryReadModel : IObserverStream, IReadModel<PessoaDTO, Guid>
    {
        private IList<PessoaAggregate> pessoasDatabase = new List<PessoaAggregate>();

        public MemoryReadModel()
        {
           // EventStream.Instance.Subscribe(this);
        }
        
        public PessoaDTO Get(Guid param)
        {
            var pessoa = pessoasDatabase.Where(x => x.Id == param).FirstOrDefault();

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
        

        //TODO: não poode deixar um raise para todos os tipos de eventos de umm queryhandler, corrigir isso
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

        //TODO: mover isso, cada query só pode se inscrever em um evento, mover para o construtor o evento e criar um base que implementa o getlisteners
        public string[] GetEventListers()
        {
            return new string[]{"FullPessoaUpdated", "CpfUpdated"};
        }
    }

    public class GetPessoaByIdQuery
    {
        private IReadModel<PessoaDTO, Guid> _readOnlyModel;
        public GetPessoaByIdQuery(IReadModel<PessoaDTO, Guid> readOnlyModel)
        {
            _readOnlyModel = readOnlyModel;
        }
        
        
        public PessoaDTO Query(GetPessoaByIdParam query)
        {
            var pessoaDto = _readOnlyModel.Get(query.Id);
            return pessoaDto;
        }
    }

    public class GetPessoaByIdParam
    {
        public Guid Id { get; }

        public GetPessoaByIdParam(Guid id)
        {
            Id = id;
        }
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