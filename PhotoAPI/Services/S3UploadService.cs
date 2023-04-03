using System;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon.S3.Model;

namespace PhotoAPI.Services
{
    public class S3UploadService : IPhotoUploadService
    {


         string awsAccessKey = Environment.GetEnvironmentVariable(variable: "AWS_ACCESS_KEY")!;
         string awsSecret = Environment.GetEnvironmentVariable(variable: "AWS_SECRET")!;
         string bucketName = Environment.GetEnvironmentVariable(variable: "AWS_BUCKET_NAME")!;
         string url = $"https://{Environment.GetEnvironmentVariable("AWS_BUCKET_NAME")}.s3.amazonaws.com";

        public S3UploadService()
        { 
        }

        public async Task<string> UploadPhoto(IFormFile photo, string photoName)
        {
           
            await using var memoryStream = new MemoryStream();
            photo.CopyTo(memoryStream);

            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = memoryStream,
                Key = photoName,
                BucketName = bucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            using var client = new AmazonS3Client(awsAccessKey, awsSecret, Amazon.RegionEndpoint.USEast1);

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);

            return $"{url}/{photoName}";

        }

        public async Task<string> DeletePhoto(string photoName)
        {
            using var client = new AmazonS3Client(awsAccessKey, awsSecret, Amazon.RegionEndpoint.USEast1);
            DeleteObjectRequest req = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = photoName
            };
            await client.DeleteObjectAsync(req);
            
            return "successfully deleted";
        }
    }
}

