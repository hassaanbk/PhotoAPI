using System;
using PhotoAPI.Models;

namespace PhotoAPI.Repository
{
	public abstract class BaseRepository
	{
		protected readonly PhotoContext _dbContext;

		public BaseRepository(PhotoContext context)
		{
			_dbContext = context;
		}
	}
}

