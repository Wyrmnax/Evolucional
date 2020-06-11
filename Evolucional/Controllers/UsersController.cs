using Evolucional.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Evolucional.Controllers
{
    public class UsersController : Controller
    {
        private DataContext db = new DataContext();
        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        public ActionResult Register()
		{
            return View();
		}

        [HttpPost]
        public ActionResult Register(User usr)
		{
            if (ModelState.IsValid)
			{
                db.Users.Add(usr);
                db.SaveChanges();
                return RedirectToAction("Index");
			}
            else
			{
                ModelState.AddModelError("", "Model State Invalid");
			}
            return View(usr);
		}

        public ActionResult Login()
		{
            return View();
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(User users)
		{
            if(ModelState.IsValid)
			{
                using (DataContext db = new DataContext())
				{
                    var obj = db.Users.Where(u => u.Username.Equals(users.Username) && u.Password.Equals(users.Password)).FirstOrDefault();
                    if (obj != null)
					{
                        Session["UserId"] = obj.UserId.ToString();
                        Session["Username"] = obj.Username.ToString();
                        Session["AuthError"] = null;
                        return RedirectToAction("LoggedIn");
					}
                    else
					{
                        Session["AuthError"] = "AuthError";
					}

				}
			}
            return View(users);
		}

        public ActionResult LoggedIn()
		{
            Session["Processado"] = null;
            if (Session["UserID"] != null)
			{
                if (Session["Botao1Clicked"] == "Clicked")
                {
                    Session["Processado"] = "Processado";
                    Session["Botao1Clicked"] = null;
                }
                return View();
			}
            else
			{
                return RedirectToAction("Login");
            }
        }
        public ActionResult Botao1Click()
        {
            AdicionarAlunos();
            AdicionarDisciplinas();
            DarNotas();
            Session["Botao1Clicked"] = "Clicked";


            return RedirectToAction("LoggedIn");
        }

        private void AdicionarAlunos()
		{
            int studentNameCounter = 0;

            var existStudent = db.Students.OrderBy(u => u.Name).ToList().LastOrDefault();
            if (existStudent != null)
            {
                existStudent.Name.TryParseIntInvariant(NumberStyles.Integer, out studentNameCounter);
                studentNameCounter++;
            }

            for (int i = 0; i < 1000; i++)
            {
                Student aluno = new Student();
                int auxiliarName = studentNameCounter + i;
                aluno.Name = auxiliarName.ToString();
                db.Students.Add(aluno);
            }
            db.SaveChanges();
        }

        private void AdicionarDisciplinas()
        {
            var existClasses = db.Classes.FirstOrDefault();
            if (existClasses == null)
            {
                db.Classes.Add(new Class { Name = "Matemática" });
                db.Classes.Add(new Class { Name = "Português" });
                db.Classes.Add(new Class { Name = "História" });
                db.Classes.Add(new Class { Name = "Geografica" }); //Geografia? Documento de referencia aparentemente esta soletrado errado
                db.Classes.Add(new Class { Name = "Inglês" });
                db.Classes.Add(new Class { Name = "Biologia" });
                db.Classes.Add(new Class { Name = "FIlosofia" }); // Filosifia? Documento de referencia aparentemente esta com capitalizacao errada
                db.Classes.Add(new Class { Name = "Física" });
                db.Classes.Add(new Class { Name = "Química" });
                db.SaveChanges();
            }
        }

        private void DarNotas()
		{
            List<Student> todosAlunos = db.Students.ToList();
            List<Class> todasDisciplinas = db.Classes.ToList();
            List<StudentCoursingClass> novosRegistros = new List<StudentCoursingClass>();

            foreach (Student aluno in todosAlunos)
			{
                foreach(Class disciplina in todasDisciplinas)
				{
                    //valida se ja existe nota
                    if (db.StudentsCoursingClasses.Where(u => u.Student.StudentId == aluno.StudentId && u.Class.ClassId == disciplina.ClassId).ToList().Count() == 0)
					{
                        StudentCoursingClass studentCoursingClass = new StudentCoursingClass();
                        studentCoursingClass.Student = aluno;
                        studentCoursingClass.Class = disciplina;

                        var rand = new Random();

                        float nota = rand.Next(1001);
                        nota = nota / 100;//Integer from 0 to 1000, divided by 100. 

                        studentCoursingClass.Grade = nota;
                        novosRegistros.Add(studentCoursingClass);
                    }
				}
			}
            db.StudentsCoursingClasses.AddRange(novosRegistros);
            db.SaveChanges();
		}

        public ActionResult Botao2Click()
        {
            float media = 0;
            List<Student> todosAlunos = db.Students.ToList();
            List<Class> todasDisciplinas = db.Classes.ToList();

            //debug
            List<StudentCoursingClass> todosSCC = db.StudentsCoursingClasses.ToList();

            List<StudentClassExcelExport> resultadoExportadoExcel = new List<StudentClassExcelExport>();

            foreach (Student aluno in todosAlunos)
			{
                media = 0;
                var registrosAluno = db.StudentsCoursingClasses.Where(s => s.Student.StudentId == aluno.StudentId).ToList();
                if (registrosAluno != null && registrosAluno.Count() > 0)
                {
                    media = registrosAluno.Average(r => r.Grade);

                    foreach (Class disciplina in todasDisciplinas)
					{
                        StudentClassExcelExport alunoClasseLinha = new StudentClassExcelExport();
                        alunoClasseLinha.Aluno = aluno.Name;
                        alunoClasseLinha.Disciplina = disciplina.Name;
                        alunoClasseLinha.MediaDestaDisciplina = registrosAluno.FirstOrDefault(a => a.Class.ClassId == disciplina.ClassId).Grade;
                        alunoClasseLinha.MediaTodasDisciplinas = media;
                        resultadoExportadoExcel.Add(alunoClasseLinha);
                    }
                }
            }
            var data = resultadoExportadoExcel.ToList();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=RegistroAlunos.xls");
            Response.AddHeader("Content-Type", "application/vnd.ms-excel");
            WriteTsv(data, Response.Output);
            Response.End();


            return RedirectToAction("LoggedIn");
        }


        private void WriteTsv<T>(IEnumerable<T> data, TextWriter output)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            foreach (PropertyDescriptor prop in props)
            {
                output.Write(prop.DisplayName); // header
                output.Write("\t");
            }
            output.WriteLine();
            foreach (T item in data)
            {
                foreach (PropertyDescriptor prop in props)
                {
                    output.Write(prop.Converter.ConvertToString(
                         prop.GetValue(item)));
                    output.Write("\t");
                }
                output.WriteLine();
            }
        }


    }
}