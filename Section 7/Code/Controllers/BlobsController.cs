using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage;

namespace PacktCourse.AppDemo.Controllers
{
    public class BlobsController : Controller
    {
        private static string connectionString = "DefaultEndpointsProtocol=https;AccountName=prodstorageaccnt;AccountKey=Kcu4c4w18BK4TURJvXlZ/pyuoST8OU1OgLhJCv58lQfz/Isr9SnprGsM7c9bQk9sOueRqCSlcTPWi/TC3H1nBg==;EndpointSuffix=core.windows.net";

        public async Task<IActionResult> Index()
        {
            CloudStorageAccount storageAccount;
            var list = new List<Uri>();
            if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
            {
                var blobStorage = storageAccount.CreateCloudBlobClient();
                var container = blobStorage.GetContainerReference("uploads");
                await container.CreateIfNotExistsAsync();
                var permissions = await container.GetPermissionsAsync();

                // Set the access level.
                permissions.PublicAccess = Microsoft.WindowsAzure.Storage.Blob.BlobContainerPublicAccessType.Blob;
                foreach (var item in (await container.ListBlobsSegmentedAsync(null)).Results)
                {
                    list.Add(item.Uri);
                }
            }

            return View(list);
        }
        
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            CloudStorageAccount storageAccount;
            if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
            {
                var blobStorage = storageAccount.CreateCloudBlobClient();
                var container = blobStorage.GetContainerReference("uploads");
                await container.CreateIfNotExistsAsync();
                
                var blobBlock = container.GetBlockBlobReference(Guid.NewGuid().ToString() + file.FileName);
                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    var buffer = memoryStream.ToArray();
                    await blobBlock.UploadFromByteArrayAsync(buffer, 0, buffer.Length);
                }
            }
            return RedirectToAction("Index");
        }
    }
}