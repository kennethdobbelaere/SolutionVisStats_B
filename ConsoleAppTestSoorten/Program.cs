using VisStatsBL.Interfaces;
using VisStatsBL.Managers;
using VisStatsDL_File;
using VisStatsDL_SQL;

namespace ConsoleAppTestSoorten {
    internal class Program {
        static void Main(string[] args) {

            string filePath = @"C:\Users\kenne\Downloads\Vis\vissoorten1.txt";
           string connectionString = @"Data Source=MSI\SQLEXPRESS;Initial Catalog=PGVVisStats;Integrated Security=True; TrustServerCertificate = true";
            Console.WriteLine("hello");

            IFileProcessor processor = new FileProcessor();
            IVisStatsRepository visStatsRepository = new VisStatsRepository(connectionString);
            VisStatsManager visStatsManager = new VisStatsManager(processor, visStatsRepository);
            visStatsManager.UploadVissoorten(filePath);


        }
    }
}