using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PacktCourse.ConsoleDemo
{
    class Program
    {
        private static string ConnectionString => "DefaultEndpointsProtocol=https;AccountName=packtcoursestorage;AccountKey=M9T3dvZrUPPEesTiPpoDG+PqpE9spBNT1rUlOLylzKKdenKigIjfJ5ctqqNQbckLpN98v5VdzCxlKoLSxEq2KA==;EndpointSuffix=core.windows.net";

        // References for the cloud objects.
        private static CloudStorageAccount account;
        private static CloudBlobClient blobClient;
        private static CloudBlobContainer container;

        private static string filePath = @"E:\blobUpload.txt";
        private static string newFile = @"E:\blobDownload.txt";

        private static string containerName = "offline-container";

        static void Main(string[] args)
        {
            // 1. Establish a connection.
            establishConnection();

            if(account != null)
            {
                // Get the reference to the blob.
                blobClient = account.CreateCloudBlobClient();
                Console.WriteLine("[Connection] Connected successfully, uploading file now...");

                runTasks();
            }
            
            Console.Read();
        }

        private async static void runTasks() 
        {
            // 2. Create a container.
            await createContainer(containerName);

            // 3. Upload the file.
            await uploadBlob(filePath);

            // 4. Download the file.
            await downloadBlob(newFile);

            // 5. Print the SAS token.
            await getSasToken();

            // 6. Print the content.
            printContent(newFile);

            // 7. Delete the blob.
            await deleteBlobFile();
        }

        private static void establishConnection()
        {
            if(!CloudStorageAccount.TryParse(ConnectionString, out account))
            {
                Console.WriteLine("[Connection] Connection string is invalid.");
            }
        }

        private async static Task createContainer(string containerName)
        {
            container = blobClient.GetContainerReference(containerName);
            
            // Validation.
            await container.CreateIfNotExistsAsync();
        }

        private async static Task uploadBlob(string filePath)
        {
            var blobReference = container.GetBlockBlobReference("myBlob");
            
            using (var file = File.OpenRead(filePath))
            {
                await blobReference.UploadFromStreamAsync(file);
                Console.WriteLine("[Upload] File uploaded successfully.");
            }
        }

        private async static Task downloadBlob(string fileName)
        {
            var blobReference = container.GetBlockBlobReference("myBlob");

            await blobReference.DownloadToFileAsync(fileName, FileMode.OpenOrCreate);
            Console.WriteLine("[Download] File downloaded successfully.");
        }

        private async static Task getSasToken()
        {
            var blobReference = container.GetBlockBlobReference("myBlob");
            if(await blobReference.ExistsAsync())
            {
                var token = blobReference.GetSharedAccessSignature(new SharedAccessBlobPolicy
                {
                    SharedAccessExpiryTime = DateTime.Now.AddDays(1)
                });

                Console.WriteLine($"The SAS token for the required policy is: {token}.");
            }
        }

        private async static Task deleteBlobFile()
        {
            var blobReference = container.GetBlockBlobReference("myBlob");
            if(await blobReference.DeleteIfExistsAsync())
            {
                Console.WriteLine("[Delete] Blob was deleted.");
            } else
            {
                Console.WriteLine("[Delete] Blob cannot be deleted.");
            }
        }

        private static void printContent(string fileName)
        {
            string content = File.ReadAllText(fileName);
            Console.WriteLine();
            Console.WriteLine("Content of the file downloaded is:");
            Console.WriteLine(content);
        }
    }
}
