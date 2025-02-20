using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Configuration;
using Zorgdossier.Models;

namespace Zorgdossier.Databases
{
    public class ApplicationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConfigurationManager.ConnectionStrings["localDb"].ConnectionString);
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public ApplicationDbContext()
        {

        }

        public DbSet<Student> Student
        {
            get; set;
        }
        public DbSet<Dossier> Dossier
        {
            get; set;
        }
        public DbSet<BasicInformation> BasicInformation
        {
            get; set;
        }
        public DbSet<Phone> Phone
        {
            get; set;
        }
        public DbSet<Question> Question
        {
            get; set;
        }
        public DbSet<ComplaintsSymptoms> ComplaintsSymptoms
        {
            get; set;
        }
        public DbSet<Research> Research
        {
            get; set;
        }
        public DbSet<Policy> Policy
        {
            get; set;
        }
        public DbSet<ContactAdvice> ContactAdvice
        {
            get; set;
        }
        public DbSet<Treatment> Treatment
        {
            get; set;
        }
        public DbSet<Organ> Organ
        {
            get; set;
        }

        public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
        {
            public ApplicationDbContext CreateDbContext(string[] args)
            {
                // Lees de connection string uit de app.config
                string connectionString = ConfigurationManager.ConnectionStrings["localDb"].ConnectionString;

                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

                // Stel de SQLite provider in met de connection string
                optionsBuilder.UseSqlite(connectionString);

                return new ApplicationDbContext(optionsBuilder.Options);
            }
        }
    }
}