using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Evolucional.Models
{
    public class DatabaseInitializer : DropCreateDatabaseAlways<DataContext>
    {
        protected override void Seed(DataContext context)
        {
            IList<User> users = new List<User>();

            users.Add(new User() { Username = "candidato-evolucional", Password = "123456" });
            users.Add(new User() { Username = "1", Password = "1" });

            context.Users.AddRange(users);

            base.Seed(context);
        }
    }
}