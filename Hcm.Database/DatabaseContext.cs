using Hcm.Core.Database;
using Hcm.Database.Mappings;
using Hcm.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Hcm.Database
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext([NotNull] DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Sallary> Sallaries { get; set; }
        public DbSet<User> Users { get; set; }

        public Task CommitAsync()
        {
            return SaveChangesAsync();
        }

        public Task RollbackAsync()
        {
            var changedEntries = ChangeTracker.Entries()
                .Where(x => x.State != EntityState.Unchanged)
                .ToList();

            foreach (var entry in changedEntries)
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.CurrentValues.SetValues(entry.OriginalValues);
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Added:
                        entry.State = EntityState.Detached;
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Unchanged;
                        break;
                }
            }

            return Task.CompletedTask;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=dss.db", opt =>
                {
                    opt.MigrationsAssembly("Hcm.Api");
                    opt.MigrationsHistoryTable("migration", "dbo");
                });
            }

            base.OnConfiguring(optionsBuilder); 
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EmployeeMap).Assembly);
            
            modelBuilder.Entity<User>().HasData(new User
            {
                Username = "administrator@hcm.com",
                Email = "administrator@hcm.com",
                Phone = "+359878121212121",
                Role = Roles.Administrator,
                Id = Guid.NewGuid().ToString(),
                Password = "6b45f1416832256171cbfe03140a4cab" //Adminpass1_
            });



            base.OnModelCreating(modelBuilder);
        }
    }
}
