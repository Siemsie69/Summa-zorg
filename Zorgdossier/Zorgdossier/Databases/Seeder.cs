using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zorgdossier.Databases
{
    internal class Seeder
    {
        //Deze methode wordt gebruikt om testdata in de MySQL database te zetten.
        internal void TestData()
        {
            using AppDbContext db = new();
            db.Tests.AddRange(
                [
                    new()
                    {
                        Id = 1, Text = "Testwaarde1"
                    },
                    new()
                    {
                        Id = 2, Text = "Testwaarde2"
                    },
                    new()
                    {
                        Id = 3, Text = "Testwaarde3"
                    }
                ]);

            db.SaveChanges();
        }
    }
}