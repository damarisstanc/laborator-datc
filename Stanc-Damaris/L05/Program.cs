
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Models;
using System;

namespace L05
{
    class Program
    {
         private static CloudTableClient tableClient;
        private static CloudTable studentsTable;
        private static CloudTable metricsTable;
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

          metricsTable = tableClient.GetTableReference("metrics");
            await metricsTable.CreateIfNotExistsAsync();

          //await AddNewStudent();
          await GetAllStudents();
         // await EditStudent();
          //await DeleteStudent();
        }

        private static async Task AddNewStudent()
        {
            var student = new StudentEntity("UVT", "1980307072532");
            student.FirstName = "Cprenume";
            student.LastName = "Cnume";
            student.Email = "cprenume.cnume@gmail.com";
            student.Year = 3;
            student.PhoneNumber = "0752421484";
            student.Faculty = "CH";

            var insertOperation = TableOperation.Insert(student);

            await studentsTable.ExecuteAsync(insertOperation);
        }

        private static async Task GetAllStudents() 
        {
            int Count = 0;

            //Console.WriteLine("Universitate\tCNP\tNume\tEmail\tNumar de telefon\tAn");
            Console.WriteLine("PartitionKey\tRowKey\tCount");
            TableQuery<StudentEntity> query1 = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "UPT"));;
            TableQuery<StudentEntity> query2 = new TableQuery<StudentEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "UVT"));;
            TableQuery<StudentEntity> query3 = new TableQuery<StudentEntity>();

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<StudentEntity> resultSegment = await studentsTable.ExecuteQuerySegmentedAsync(query1, token);

                token = resultSegment.ContinuationToken;

                foreach (StudentEntity entity in resultSegment.Results)
                {
                    ++Count;
                    Console.WriteLine("{0}\t{1}\t{2}", entity.PartitionKey, entity.RowKey, Count);      
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
