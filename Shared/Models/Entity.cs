using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	public class Entity
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
	}
}
