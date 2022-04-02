using LiteDB;

namespace DFlow.Persistence.LiteDB.Model
{
    public class LiteDbContext
    {
        protected LiteDbContext(string connectionString, BsonMapper modelBuilder)
        {
            ModelBuilder = modelBuilder;
            Database = new LiteDatabase(connectionString);
        }

        public ILiteDatabase Database { get; }

        protected BsonMapper ModelBuilder { get; }
    }
}