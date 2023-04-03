using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PhotoAPI.Models;

namespace PhotoAPI.Repository
{
	public class PhotoRepository : BaseRepository, IPhotoRespository
	{
		public PhotoRepository(PhotoContext dbContext) : base(dbContext)
		{
		}

		public async Task<Photo> Add(Photo photo)
		{
			_dbContext.Photos.Add(photo);
			await _dbContext.SaveChangesAsync();
			return photo;
		}

		public async Task<Photo> FindByName(string name)
		{
			return await _dbContext.Photos
				.Where(c =>	c.Name == name)
				.FirstOrDefaultAsync();
		}

		public async Task<Photo> FindById(Guid id)
		{
			return await _dbContext.Photos
				.Where(c =>	c.Id == id)
				.FirstOrDefaultAsync();
		}

		public async Task Delete(Photo photo)
		{
			_dbContext.Remove(photo);
			await _dbContext.SaveChangesAsync();
		}

		public async Task Update(Photo photo)
		{
			_dbContext.Update(photo);
			await _dbContext.SaveChangesAsync();
		}

	}
}

