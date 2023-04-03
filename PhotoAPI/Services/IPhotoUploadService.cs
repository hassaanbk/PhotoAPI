using System;
namespace PhotoAPI.Services
{
    public interface IPhotoUploadService
    {
        public Task<string> UploadPhoto(IFormFile photo, string photoName);
        public Task<string> DeletePhoto(string photoName);
     
    }
}

