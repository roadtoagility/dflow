namespace Ecommerce.Persistence.State;

public sealed record ProductState(Guid Id, string Name, string Description
        , double Weight, byte[] RowVersion)
    : State(RowVersion);