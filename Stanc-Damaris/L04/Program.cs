using System;
//using Azure.Data.Tables;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;

namespace L04
{
    class Program
    {
        private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");
            Task.Run(async () => {await Initialize(); })
              .GetAwaiter()
              .GetResult();
        }

        static async Task Initialize() 
        {
          string storageConnectionString = "DefaultEndpointsProtocol=https;"
            + "AccountName=labdatc"
            + ";AccountKey=IdtIU0U9Z80qtfWavDUe9VSCqQJfNydQSXtQ4ihdyeB1GupuexwvMIB3iEB4PLueATJbkyK7vbKheZhERgkihw=="
            + ";EndpointSuffix=core.windows.net";

          var account = CloudStorageAccount.Parse(storageConnectionString);
          tableClient = account.CreateCloudTableClient();

          studentsTable = tableClient.GetTableReference("studenti");

          await studentsTable.CreateIfNotExistsAsync();

          await AddNewStudent();
          await GetAllStudents();
          await EditStudent();
          //await DeleteStudent();
        }

        private static async Task AddNewStudent()
        {
            var student = new StudentEntity("UPT", "1990307072532");
            student.FirstName = "Bprenume";
            student.LastName = "Bnume";
            student.Email = "bprenume.bnume@gmail.com";
            student.Year = 3;
            student.PhoneNumber = "0758521484";
            student.Faculty = "AC";

            var insertOperation = TableOperation.Insert(student);

            await studentsTable.ExecuteAsync(insertOperation);
        }

        private static async Task GetAllStudents() 
        {
            Console.WriteLine("Universitate\tCNP\tNume\tEmail\tNumar de telefon\tAn");
            TableQuery<StudentEntity> query = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (StudentEntity entity in resultSegment.Results)
                {
                    Console.WriteLine("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}", entity.PartitionKey, entity.RowKey, entity.FirstName, entity.LastName, entity.Email, entity.Year, entity.PhoneNumber, entity.Faculty);
                }

            }while (token != null);
        }

         private static async Task EditStudent()
        {
            var student = new StudentEntity("UPT", "1990307072532");
            student.FirstName = "Bprenume";
            student.Year = 3;
            student.ETag = "*";

            var editOperation = TableOperation.Merge(student);
            await studentsTable.ExecuteAsync(editOperation);
        }

 private static async Task DeleteStudent()
        {
            var student = new StudentEntity("UPT", "1990307072532");
            student.FirstName = "Bprenume";
            student.Year = 3;
            student.ETag = "*";

            var deleteOperation = TableOperation.Delete(student);
            await studentsTable.ExecuteAsync(deleteOperation);
        }

    }
}
