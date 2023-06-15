using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalabaTask.Context;
using TalabaTask.Entities;
using TalabaTask.Migrations;
using TalabaTask.Models;

namespace TalabaTask.Controllers;

public class TeachersController : Controller
{
	private readonly AppDbContext _db;
	public TeachersController(AppDbContext appDbContext) 
	{
		_db = appDbContext;
	}
	public IActionResult Register(CreateTeacherModel createTeacher)
	{
		var teacher = new Teacher()
		{
			FirstName = createTeacher.FirstName,
			LastName = createTeacher.LastName,
			PhoneNumber = createTeacher.PhoneNumber,
			Email = createTeacher.Email,
			Password = createTeacher.Password,
			BirthDate = createTeacher.BirthDate,
		};

		return View();
	}

	public async Task<IActionResult> Login(LoginTeacherModel loginTeacher)
	{
		if (!ModelState.IsValid)
		{
			return View(loginTeacher);
		}

		var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.PhoneNumber == loginTeacher.PhoneNumber);

		if (teacher == null || teacher.Password != loginTeacher.Password)
		{
			return View(loginTeacher);
		}

		return RedirectToAction("Profile");
	}

	public async Task<IActionResult> Profile()
	{
		var teacher = await _db.Teachers.FirstOrDefaultAsync();

		return View(teacher);
	}

	public async Task<IActionResult> GetTeachersOver50()
	{
		var teachers = await _db.Teachers
			.Where(t => t.BirthDate.Year - DateTime.Now.Year > 54)
			.ToListAsync();

		return View(teachers);
	}

	public async Task<IActionResult> GetTeachersAndStudentsBeeline()
	{
		var students = await _db.Students
			.Where(s => s.PhoneNumber[5] == '9' 
			&& (s.PhoneNumber[6] == '0' || s.PhoneNumber[6] == '1'))
			.ToListAsync();
		
		var teachers = await _db.Teachers
			.Where(s => s.PhoneNumber[5] == '9' 
			&& (s.PhoneNumber[6] == '0' || s.PhoneNumber[6] == '1'))
			.ToListAsync();

		var tuple = new Tuple<List<Student>, List<Teacher>>(students, teachers);

		return View(tuple);
	}

}
