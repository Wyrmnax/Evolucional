using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evolucional.Models
{
	public class Student
	{
		[Key]
		public int StudentId { get; set; }

		[Display(Name = "Aluno")]
		public string Name { get; set; }
	}
}