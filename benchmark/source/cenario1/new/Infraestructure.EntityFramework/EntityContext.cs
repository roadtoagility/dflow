namespace Infraestructure.EntityFramework
{
    public static class EntityContext
    {
        public static BenchmarkDBContext GetContext()
        {
            return new BenchmarkDBContext(null);
        }
    }

    public abstract class EntityFrameworkConnector
    {
        protected readonly BenchmarkDBContext Context;

        public EntityFrameworkConnector(BenchmarkDBContext context)
        {
            Context = context;
        }
    }
}