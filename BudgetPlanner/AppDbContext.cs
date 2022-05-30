using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BudgetPlanner.Constants;
using BudgetPlanner.Objects;
using Microsoft.EntityFrameworkCore;

namespace BudgetPlanner
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Operations> UserOperations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableDetailedErrors();
            string dbPath = ProgPathes.DataFolderPath + "budgetplanner.db";
            optionsBuilder.UseSqlite(dbPath);
        }

        public AppDbContext()
        {
            var pendingMigrations = Database.GetPendingMigrations();
            
        }
    }
}
