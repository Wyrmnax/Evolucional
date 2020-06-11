using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evolucional.Models
{
	public class Class
	{
		[Key]
		public int ClassId { get; set; }

		[Display(Name = "Nome Disciplina")]
		public string Name { get; set; }
	}
}