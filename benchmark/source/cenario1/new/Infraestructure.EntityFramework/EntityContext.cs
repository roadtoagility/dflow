namespace Infraestructure.EntityFramework
{
    public static class EntityContext
    {
        public static BenchmarkDBContext GetContext()
        {
            return new BenchmarkDBContext(null);
        }
    }
}