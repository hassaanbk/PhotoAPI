using System;
namespace PhotoAPI.Services
{
    public class PhotoUploadService : IPhotoUploadService
    {
        public PhotoUploadService()
        {
        }

        public async Task<string> UploadPhoto(IFormFile photo, string photoName)
        {
            using(var fileStream = new FileStream(photoName, FileMode.Create))
            {
                await photo.CopyToAsync(fileStream);
                fileStream.Close();
            }
            return photoName;
        }

        public async Task<string> DeletePhoto(string name)
        {
            File.Delete(name);
            return "File deleted";
        }
    }
}

