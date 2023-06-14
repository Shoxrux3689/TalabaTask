namespace TalabaTask.Entities;

public class Teacher
{
	public long Id { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string PhoneNumber { get; set; }
	public string Email { get; set; }
	public string Password { get; set; }
	public DateTime BirthDate { get; set; }
	public List<Science>? Sciences { get; set; }
}
