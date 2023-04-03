using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoAPI.Models
{
	[Table("photos")]
	public class Photo
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		public string? Name { get; set; }

		[Required]
		public string? Url { get; set; }

	}
}

