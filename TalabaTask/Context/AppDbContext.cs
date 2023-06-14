using Microsoft.EntityFrameworkCore;
using TalabaTask.Entities;

namespace TalabaTask.Context;

public class AppDbContext : DbContext
{
	public DbSet<Student> Students { get; set; }
	public DbSet<Teacher> Teachers { get; set; }
	public DbSet<Science> Sciences { get; set; }
	public DbSet<Gradiate> Gradiates { get; set; }
	public DbSet<StudentSciences> StudentSciences { get; set; }
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
	{

	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<Gradiate>()
			.HasKey(g => new { g.StudentId, g.ScienceId });

		modelBuilder.Entity<StudentSciences>()
			.HasKey(s => new { s.StudentId, s.ScienceId });
	}
}
