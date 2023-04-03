using System;
using PhotoAPI.Models;

namespace PhotoAPI.Repository
{
	public interface IPhotoRespository
	{

		Task<Photo> Add(Photo photo);
		Task<Photo> FindByName(string name);
		Task<Photo> FindById(Guid id);
		Task Delete(Photo photo);
		Task Update(Photo photo);

	}
}

