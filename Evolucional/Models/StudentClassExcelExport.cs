using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Evolucional.Models
{
	public class StudentClassExcelExport
	{

		public string Aluno { get; set; }
		public string Disciplina { get; set; }
		public float MediaDestaDisciplina { get; set; }
		public float MediaTodasDisciplinas { get; set; }
	}
}