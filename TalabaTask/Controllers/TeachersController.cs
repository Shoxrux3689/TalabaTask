using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalabaTask.Context;
using TalabaTask.Entities;
using TalabaTask.Migrations;
using TalabaTask.Models;
using TalabaTask.Providers;
using TalabaTask.Services;

namespace TalabaTask.Controllers;

public class TeachersController : Controller
{
	private readonly AppDbContext _db;
	private readonly JwtServiceTeacher _jwtService;
	private readonly long _userId; 
	public TeachersController(AppDbContext appDbContext, 
		JwtServiceTeacher jwtService,
		UserProvider userProvider) 
	{
		_db = appDbContext;
		_jwtService = jwtService;
		_userId = userProvider.UserId;
	}

	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Register(CreateTeacherModel createTeacher)
	{
		if (!ModelState.IsValid)
		{
			return View(createTeacher);
		}

		var teacher = new Teacher()
		{
			FirstName = createTeacher.FirstName,
			LastName = createTeacher.LastName,
			PhoneNumber = createTeacher.PhoneNumber,
			Email = createTeacher.Email,
			Password = createTeacher.Password,
			BirthDate = createTeacher.BirthDate,
		};

		_db.Teachers.Add(teacher);
		await _db.SaveChangesAsync();

		return RedirectToAction("Login");
	}

	[HttpGet]
	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
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

		var token = _jwtService.GenerateToken(teacher);
		HttpContext.Response.Cookies.Append("token", $"{token}");

		return RedirectToAction("Profile");
	}

	[Authorize]
	public async Task<IActionResult> Profile()
	{
		var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == _userId);

		return View(teacher);
	}

	public async Task<IActionResult> GetTeachers()
	{
		return View(await _db.Teachers.ToListAsync());
	}
	public async Task<IActionResult> GetTeachersOver55()
	{
        var teachers = await _db.Teachers
            .Where(s => (DateTime.Now.Year - s.BirthDate.Year > 55) ||
            (DateTime.Now.Year - s.BirthDate.Year == 55 && DateTime.Now.Month - s.BirthDate.Month > 0) ||
            (DateTime.Now.Year - s.BirthDate.Year == 55 && DateTime.Now.Month - s.BirthDate.Month >= 0 && DateTime.Now.Day - s.BirthDate.Day > 0))
            .ToListAsync();

        return View(teachers);
	}

	public async Task<IActionResult> GetTeachersAndStudentsBeeline()
	{
		var students = await _db.Students.ToListAsync();

		students = students.Where(s =>
			s.PhoneNumber[4] == '9'
			&& (s.PhoneNumber[5] == '0' || s.PhoneNumber[5] == '1'))
			.ToList();

		var teachers = await _db.Teachers.ToListAsync();

		teachers = teachers.Where(s => 
			s.PhoneNumber[4] == '9' 
			&& (s.PhoneNumber[5] == '0' || s.PhoneNumber[5] == '1'))
			.ToList();

		var tuple = new Tuple<List<Student>, List<Teacher>>(students, teachers);

		return View(tuple);
	}

	public async Task<IActionResult> GetTeachersOver97Students()
	{
		var teachers = await _db.Teachers
			.Include(t => t.Sciences)
			.ThenInclude(s => s.Gradiates)
			.Where(t => t.Sciences.Any(s => s.Gradiates.Any(g => g.Grade > 97)))
			.ToListAsync();

		return View(teachers);
	}
}
