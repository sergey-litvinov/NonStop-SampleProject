using System.ComponentModel.DataAnnotations;
using System.Data.Entity;

namespace ContactApp.Data
{
	public class EfContext : DbContext
	{
		public EfContext()
			: base("Contacts")
		{
			
		}
		public DbSet<Contact> Contacts { get; set; }
	}

	public class Contact
	{
		public int Id { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }
		
		[StringLength(16)]
		public string Phone { get; set; }

		[StringLength(16)]
		public string Mobile { get; set; }

		[StringLength(16)]
		public string Work { get; set; }

		[StringLength(500)]
		public string Address { get; set; }
		
		public string Photo { get; set; }

		[StringLength(50)]
		public string Email { get; set; }
	}
}