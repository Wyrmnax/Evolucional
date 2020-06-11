using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Evolucional.Models
{
	public class StudentCoursingClass
	{
		[Key]
		public int StudentClassId { get; set; }
		public Student Student { get; set; }
		public Class Class { get; set; }
		public float Grade { get; set; }

	}
}