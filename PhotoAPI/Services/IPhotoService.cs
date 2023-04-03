using System;
using PhotoAPI.Models;
namespace PhotoAPI.Services
{
    public interface IPhotoService
    {
        Task<Photo> Add(Photo photo);
        Task<Photo> FindByName(string name);
        Task<Photo> FindById(Guid id);
        Task Delete(Photo photo);
        Task Update(Photo photo);
    }
}

