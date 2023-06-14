using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalabaTask.Context;
using TalabaTask.Entities;
using TalabaTask.Migrations;
using TalabaTask.Models;

namespace TalabaTask.Controllers;

public class TeacherController : Controller
{
	private readonly AppDbContext _db;
	public TeacherController(AppDbContext appDbContext) 
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
}
