using Microsoft.EntityFrameworkCore;
using mvcEnterprise.Models;

namespace mvcEnterprise.Data {

	public class AppDBContext : DbContext {
		public AppDBContext (DbContextOptions<AppDBContext> options) : base (options) { }

		// public DbSet<EntityName> EntityNames { get; set; }
		public DbSet<Employee> Employees { get; set; }
		public DbSet<Category> Categories { get; set; }

	}


}	
