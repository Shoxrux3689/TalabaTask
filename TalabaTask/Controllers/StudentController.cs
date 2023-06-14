using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalabaTask.Context;
using TalabaTask.Entities;
using TalabaTask.Models;

namespace TalabaTask.Controllers
{
	public class StudentController : Controller
	{
		private readonly AppDbContext _db;
		public StudentController(AppDbContext appDb) 
		{
			_db = appDb;
		}

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

			return Ok();
		}

		public async Task<IActionResult> Profile()
		{
			var student = _db.Students.FirstOrDefault();
			
			return View(student);
		}
	}
}
