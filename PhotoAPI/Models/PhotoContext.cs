using System;
using Microsoft.EntityFrameworkCore;

namespace PhotoAPI.Models
{
	public class PhotoContext : DbContext
	{
		public PhotoContext(DbContextOptions<PhotoContext> options) : base(options)
		{

		}
		public DbSet<Photo> Photos { get; set; } = null!;
	}
}

