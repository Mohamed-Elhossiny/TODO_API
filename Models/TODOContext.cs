using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;

namespace TODO_API.Models
{
	public class TODOContext : DbContext
	{
		public virtual DbSet<Task> Task { get; set; }
		public TODOContext() { }
		public TODOContext(DbContextOptions<TODOContext> options) : base(options) { }
	}
}
