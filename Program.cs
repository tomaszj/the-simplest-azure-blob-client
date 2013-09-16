using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace WelcomeToTheConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // This is a simple program accessing the blob storage on local Windows
            // Azure emulator. It puts a new file with random name everytime it
            // runs. Then all files within the container are listed and printed.
            Console.WriteLine("Simple blob storage access program started");

            var connectionString = ConfigurationManager.ConnectionStrings["StorageConnectionString"].ConnectionString;
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();

            var testContainer = blobClient.GetContainerReference("testcontainer");
            testContainer.CreateIfNotExists();

            // Create a file with random name on each execution
            string howAboutSomeRandomValue = new Random().Next().ToString();
            string randomFilename = howAboutSomeRandomValue + ".txt";

            var randomBlob = testContainer.GetBlockBlobReference(randomFilename);
            randomBlob.UploadText("New file with test content");

            Console.WriteLine("New random file created, listing container contents.");

            var blockBlobs = testContainer.ListBlobs().Where(b => b.GetType() == typeof(CloudBlockBlob)).Cast<CloudBlockBlob>();
            if (blockBlobs != null)
            {
                int i = 0;
                int blockBlobsCount = blockBlobs.Count();
                foreach (var blockBlob in blockBlobs)
                {
                    i++;
                    var contents = blockBlob.DownloadText();
                    Console.WriteLine(String.Format("{0}/{1}: {2}", i, blockBlobsCount, contents));
                }
            }

            Console.WriteLine("Finished!");
        }
    }
}
