using System;
using PhotoAPI.Repository;
using PhotoAPI.Models;
namespace PhotoAPI.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly IPhotoRespository _photoRespository;

        public PhotoService(IPhotoRespository photoRespository)
        {
            _photoRespository = photoRespository ?? throw new ArgumentNullException(nameof(photoRespository));
        }

        public Task<Photo> Add(Photo photo)
        {
            if (_photoRespository == null)
            {
                throw new ArgumentNullException(nameof(_photoRespository), "The photo repository is null.");
            }

            photo.Id = Guid.NewGuid();
            var res = _photoRespository.Add(photo);
            return res;
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

