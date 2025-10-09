using System.Data.Entity;

namespace WFApp_Electronic_Scale
{
    public sealed class ScaleDbContext : DbContext
    {
        private readonly string _tableName;

        public ScaleDbContext(string nameOrConnectionString, string tableName)
            : base(nameOrConnectionString)
        {
            _tableName = tableName;
        }

        public DbSet<WeightRecord1> Weights1 { get; set; }
        public DbSet<WeightRecord2> Weights2 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeightRecord1>().ToTable("Tartim1");
            modelBuilder.Entity<WeightRecord2>().ToTable("Tartim2");

            //         var entity = modelBuilder.Entity<WeightRecord1>();
            //entity.ToTable(_tableName);
            //entity.HasKey(e => e.Id);
        }

    }
}


