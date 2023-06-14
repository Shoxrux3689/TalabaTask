namespace TalabaTask.Entities;

public class Science
{
	public long Id { get; set; }
	public string Name { get; set; }
	public Teacher? Teacher { get; set; }
	public long TeacherId { get; set; }
	public List<StudentSciences>? StudentSciences { get; set; }
	public List<Gradiate>? Gradiates { get; set; }
}
