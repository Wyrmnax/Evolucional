using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Evolucional.Models
{
	public class DataContext : DbContext
	{
		public DataContext() : base("projeto-evolucional")
		{
			Database.SetInitializer<DataContext>(new DatabaseInitializer());
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			//Adds configurations for Student from separate class
			//modelBuilder.Configurations.Add(new UserDbDTO());

			modelBuilder.Entity<User>()
				.ToTable("Users");

			modelBuilder.Entity<Student>()
				.ToTable("Student");

			modelBuilder.Entity<Class>()
				.ToTable("Class");

			modelBuilder.Entity<StudentCoursingClass>()
				.ToTable("StudentCoursingClass");
		}
		public DbSet<User> Users { get; set; }
		public DbSet<Student> Students { get; set; }
		public DbSet<Class> Classes { get; set; }
		public DbSet<StudentCoursingClass> StudentsCoursingClasses { get; set; }
	}
}