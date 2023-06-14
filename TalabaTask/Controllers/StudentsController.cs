using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalabaTask.Context;
using TalabaTask.Entities;
using TalabaTask.Models;
using TalabaTask.Providers;
using TalabaTask.Services;

namespace TalabaTask.Controllers;

public class StudentsController : Controller
{
	private readonly AppDbContext _db;
	private readonly JwtServiceStudent jwtService;
	private readonly UserProvider _userProvider;

	public StudentsController(AppDbContext appDb, JwtServiceStudent serviceStudent, UserProvider userProvider) 
	{
		_db = appDb;
		_userProvider = userProvider;
		jwtService = serviceStudent;
	}

	[HttpGet]
	public IActionResult Register()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Register(CreateStudentModel studentModel)
	{
		if (!ModelState.IsValid)
		{
			return View(studentModel);
		}

		var student = new Student()
		{
			FirstName = studentModel.FirstName,
			LastName = studentModel.LastName,
			PhoneNumber = studentModel.PhoneNumber,
			Email = studentModel.Email,
			Password = studentModel.Password,
			BirthDate = studentModel.BirthDate,
			StudentRegNumber = Guid.NewGuid(),
		};

		_db.Students.Add(student);
		await _db.SaveChangesAsync();

		return RedirectToAction("Login");
	}

	public IActionResult Login()
	{
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> Login(StudentLoginModel studentLogin) 
	{
		if (!ModelState.IsValid)
		{
			return View(studentLogin);
		}

		var student = await _db.Students.FirstOrDefaultAsync(s => s.PhoneNumber == studentLogin.PhoneNumber);
		if (student == null || student.Password != studentLogin.Password)
		{
			return View(studentLogin);
		}

		var token = jwtService.GenerateToken(student);
		HttpContext.Response.Cookies.Append("token", $"{token}");

		return RedirectToAction("Profile");
	}

	[Authorize]
	public async Task<IActionResult> Profile()
	{
		var student = await _db.Students.FirstOrDefaultAsync(s => s.Id == _userProvider.UserId);
		
		return View(student);
	}
}
