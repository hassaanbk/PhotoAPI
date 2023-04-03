using System;
using Amazon.S3.Transfer;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;

namespace PhotoAPI.Services
{
    public class S3UploadService : IPhotoUploadService
    {


         private readonly string _awsAccessKey;
        private readonly string _awsSecret;
        private readonly string _bucketName;
        private readonly string _url;

        public S3UploadService()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var awsAccessKey = MyConfig.GetValue<string>("AWS_ACCESS_KEY");
            var awsSecret = MyConfig.GetValue<string>("AWS_SECRET");
            var bucky = MyConfig.GetValue<string>("AWS_BUCKET_NAME");

            _awsAccessKey = awsAccessKey;
            _awsSecret = awsSecret;
            _bucketName = bucky;
            _url = $"https://{bucky}.s3.amazonaws.com";
        }

        public async Task<string> UploadPhoto(IFormFile photo, string photoName)
        {
           
            await using var memoryStream = new MemoryStream();
            photo.CopyTo(memoryStream);

            var uploadRequest = new TransferUtilityUploadRequest()
            {
                InputStream = memoryStream,
                Key = photoName,
                BucketName = _bucketName,
                CannedACL = S3CannedACL.PublicRead
            };

            using var client = new AmazonS3Client(_awsAccessKey, _awsSecret, Amazon.RegionEndpoint.USEast1);

            var transferUtility = new TransferUtility(client);

            await transferUtility.UploadAsync(uploadRequest);

            return $"{_url}/{photoName}";

        }

        public async Task<string> DeletePhoto(string photoName)
        {
            using var client = new AmazonS3Client(_awsAccessKey, _awsSecret, Amazon.RegionEndpoint.USEast1);
            DeleteObjectRequest req = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = photoName
            };
            await client.DeleteObjectAsync(req);
            
            return "successfully deleted";
        }
    }
}

