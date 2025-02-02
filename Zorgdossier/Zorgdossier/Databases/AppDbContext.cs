using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zorgdossier.Models;

namespace Zorgdossier.Databases
{
    internal class AppDbContext : DbContext
    {
        public DbSet<UserMessage> Tests
        {
            get; set;
        }

        //Deze methode wordt gebruikt om MySQL te gebruiken met alle gegevens uit de connectionString die in app.config gespecificeerd is (met de naam 'MyConnStr').
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            string connstr = ConfigurationManager.ConnectionStrings["MyConnStr"].ConnectionString;

            optionsBuilder.UseMySQL(connstr);
        }
    }
}