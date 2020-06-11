using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evolucional.Models
{
	public class User
	{
		[Key]
		public int UserId { get; set; }

		[Required]
		[Display(Name = "Usuario")]
		public string Username { get; set; }

		[Required]
		[Display(Name = "Senha")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}