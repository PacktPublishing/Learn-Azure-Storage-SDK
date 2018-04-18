using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using PacktCourse.AppDemo.Models;

namespace PacktCourse.AppDemo.Controllers
{
    public class TablesController : Controller
    {
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=prodstorageaccnt;AccountKey=Kcu4c4w18BK4TURJvXlZ/pyuoST8OU1OgLhJCv58lQfz/Isr9SnprGsM7c9bQk9sOueRqCSlcTPWi/TC3H1nBg==;EndpointSuffix=core.windows.net";

        public async Task<IActionResult> Index()
        {
            CloudStorageAccount storageAccount;
            if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
            {
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference("people");
                await table.CreateIfNotExistsAsync();
                var query = new TableQuery<Person>();
                var result = await table.ExecuteQuerySegmentedAsync(query, null);

                return View(result);
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Save() 
        {
            Person person = new Person()
            {
                PartitionKey = "demo",
                RowKey = Guid.NewGuid().ToString(),
                Name = Request.Form["Name"],
                Gender = Request.Form["Gender"].ToString() == "Male" ? true : false,
                Message = Request.Form["Message"]
            };
            CloudStorageAccount storageAccount;
            if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
            {
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference("people");
                await table.CreateIfNotExistsAsync();
                var operation = TableOperation.Insert(person);
                await table.ExecuteAsync(operation);
            }
            
            return RedirectToAction("Index");
        }
    }
}