using System;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon.S3.Model;

namespace PhotoAPI.Services
{
    public class S3UploadService : IPhotoUploadService
    {

        private readonly IConfiguration _configuration;

        

        public S3UploadService(IConfiguration configuration)
        {
            configuration = _configuration;
        }

        public async Task<string> UploadPhoto(IFormFile photo, string photoName)
        {
            string awsAccessKey = "AKIARSTMEM43ZA5F3FLO";
            string awsSecret = "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";
            string bucketName = "photos-easyaccess";
            string url = $"https://{Environment.GetEnvironmentVariable("AWS_BUCKET_NAME")}.s3.amazonaws.com";
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
            string awsAccessKey = _configuration["AWSCredentials:AWS_ACCESS_KEY"].ToString();
            string awsSecret = _configuration["AWSCredentials:AWS_SECRET"].ToString();
            string bucketName = _configuration["AWSCredentials:AWS_BUCKET_NAME"].ToString();
            string url = $"https://{Environment.GetEnvironmentVariable("AWS_BUCKET_NAME")}.s3.amazonaws.com";
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

