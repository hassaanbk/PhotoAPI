using System;
using PhotoAPI.Repository;
using PhotoAPI.Models;
namespace PhotoAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRespository? _photoRespository;

        public Task<Photo> Add(Photo photo)
        {
            photo.Id = Guid.NewGuid();
            return _photoRespository.Add(photo);
        }

        public Task<Photo> FindByName(string name)
        {
            return _photoRespository.FindByName(name);
        }

        public Task<Photo> FindById(Guid id)
        {
            return _photoRespository.FindById(id);
        }

        public Task Delete(Photo photo)
        {
            return _photoRespository.Delete(photo);
        }

        public Task Update(Photo photo)
        {
            return _photoRespository.Update(photo);
        }
    }
}

