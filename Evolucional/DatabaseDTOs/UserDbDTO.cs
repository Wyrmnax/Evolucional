using Evolucional.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Evolucional.DatabaseDTOs
{
	public class UserDbDTO :EntityTypeConfiguration<User>
	{
		public UserDbDTO()
		{
			this.Property(s => s.Username)
				.IsRequired()
				.HasMaxLength(100);

			this.Property(s => s.Username)
				.IsConcurrencyToken();

		}
	}
}