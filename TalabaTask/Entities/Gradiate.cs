namespace TalabaTask.Entities;

public class Gradiate
{
	public Student? Student { get; set; }
	public long StudentId { get; set; }
	public Science? Science { get; set; }
	public long ScienceId { get; set; }
	public int Grade { get; set; }
}
