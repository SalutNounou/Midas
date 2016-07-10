using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using Midas.Model;
using Midas.Model.Documents;

namespace Midas.DAL
{
    public class MidasContext : DbContext, IDbContext
    {

        public DbSet<Security> Securities { get; set; }
        

        public IQueryable<T> Find<T>() where T : class
        {
            return Set<T>();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new SecurityConfiguration());
            modelBuilder.Configurations.Add(new FinancialStatementConfiguration());
        }

        public void Rollback()
        {
            ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }
    }




    public class SecurityConfiguration : EntityTypeConfiguration<Security>
    {
        public SecurityConfiguration()
        {
            ToTable("Securities");
            HasKey(s =>s.Ticker);
            Property(s => s.Name).IsRequired();
            Property(c => c.RowVersion).IsRowVersion();
            Property(s => s.Currency).IsRequired();
            Property(s => s.Market).IsRequired();
            Property(s => s.DateOfLatest10QFinancialStatement).IsRequired();
            Property(s => s.DateOfLatest10KFinancialStatement).IsRequired();
            //HasMany(s => s.FinancialStatements).WithRequired(f => f.Security);
        }
    }

    public class FinancialStatementConfiguration : EntityTypeConfiguration<FinancialStatement>
    {
        public FinancialStatementConfiguration()
        {
            ToTable("FinancialStatements");
            HasKey(s => s.Id);
            Property(s => s.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);
            Property(s => s.CompanyName).IsRequired();
        }
    }

    
}