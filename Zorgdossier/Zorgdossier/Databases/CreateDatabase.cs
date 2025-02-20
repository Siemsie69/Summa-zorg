using System.Data.SQLite;
using System.IO;

namespace Zorgdossier.Databases
{
    public static class CreateDatabase
    {
        private static string DatabasePath => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases", "database.db");

        public static void InitializeDatabase()
        {
            try
            {
                // Ensure the Databases directory exists
                string directoryPath = Path.GetDirectoryName(DatabasePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Create database file if it doesn't exist
                if (!File.Exists(DatabasePath))
                {
                    SQLiteConnection.CreateFile(DatabasePath);
                    Console.WriteLine("Database file created at: " + DatabasePath);
                }
                else
                {
                    Console.WriteLine("Database already exists at: " + DatabasePath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing database: " + ex.Message);
            }
        }
    }
}