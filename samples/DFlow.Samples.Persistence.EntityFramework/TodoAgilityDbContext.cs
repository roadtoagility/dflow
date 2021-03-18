using Microsoft.EntityFrameworkCore;
using AppFabric.Persistence.Framework.Model;
using AppFabric.Persistence.Model;
using AppFabric.Persistence.ReadModel;

namespace AppFabric.Persistence
{
    public class AppFabricDbContext : AggregateDbContext
    {
        public AppFabricDbContext(DbContextOptions<AppFabricDbContext> options)
            : base(options)
        {

        }

        public DbSet<ProjectState> Projects { get; set; }
        public DbSet<ProjectProjection> ProjectsProjection { get; set; }
        public DbSet<UserState> Users { get; set; }
        public DbSet<UserProjection> UsersProjection { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectState>(
                b =>
                {
                    b.Property(e => e.Id).ValueGeneratedNever().IsRequired();
                    b.Property(e => e.Code).IsRequired();
                    b.Property(e => e.Name).IsRequired();
                    b.Property(e => e.Budget).IsRequired();
                    b.HasKey(e => e.Id);
                    b.Property(e => e.ClientId).IsRequired();
                    b.Property(e => e.StartDate).IsRequired();
                    
                    b.Property(p => p.PersistenceId);
                    b.Property(q => q.IsDeleted);
                    b.HasQueryFilter(project => EF.Property<bool>(project, "IsDeleted") == false);
                    b.Property(e => e.CreateAt);
                    b.Property(e => e.RowVersion);
                });

            modelBuilder.Entity<UserState>(
                b =>
                {
                    b.Property(e => e.Id).ValueGeneratedNever().IsRequired();
                    b.Property(e => e.Name).IsRequired();
                    b.Property(e => e.Cnpj).IsRequired();
                    
                    b.Property(p => p.PersistenceId);
                    b.Property(e => e.IsDeleted);
                    b.HasQueryFilter(user => EF.Property<bool>(user, "IsDeleted") == false);
                    b.Property(e => e.CreateAt);
                    b.Property(e => e.RowVersion);
                });
            
            modelBuilder.Entity<ClientState>(
                b =>
                {
                    b.Property(e => e.ProjectId).ValueGeneratedNever();
                    b.Property(e => e.ClientId).IsRequired();
                    b.HasKey(e => e.ClientId);
                    b.Property(p => p.PersistenceId);
                    b.Property(q => q.IsDeleted);
                    b.HasQueryFilter(client => EF.Property<bool>(client, "IsDeleted") == false);
                    b.Property(e => e.CreateAt);
                    b.Property(e => e.RowVersion);
                });

            #region projection
            
            modelBuilder.Entity<ProjectProjection>(p =>
            {
                p.Property(pr => pr.Id).ValueGeneratedNever();
                p.HasKey(pr => pr.Id);
                p.Property(pr => pr.Name);
                p.Property(pr => pr.Code);
                p.Property(pr => pr.StartDate);
                p.Property(pr => pr.Budget);
                p.Property(pr => pr.ClientId);
                p.HasQueryFilter(proj => EF.Property<bool>(proj, "IsDeleted") == false);
            });
            
            modelBuilder.Entity<UserProjection>(u =>
            {
                u.Property(pr => pr.Id).ValueGeneratedNever();
                u.HasKey(pr => pr.Id);
                u.Property(pr => pr.Name);
                u.Property(pr => pr.Cnpj);
                u.Property(pr => pr.CommercialEmail);
                u.HasQueryFilter(user => EF.Property<bool>(user, "IsDeleted") == false);
            });
            
            #endregion
        }
    }
}