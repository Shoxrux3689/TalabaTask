using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using TalabaTask.Context;
using TalabaTask.Entities;
using TalabaTask.Models;
using TalabaTask.Providers;

namespace TalabaTask.Controllers;

public class SciencesController : Controller
{
	private readonly AppDbContext _db;
	private readonly long _userId;
	public SciencesController(AppDbContext db, UserProvider userProvider)
	{
		_db = db;
		_userId = userProvider.UserId;
	}

	[HttpGet]
	[Authorize]
	public async Task<IActionResult> CreateScience()
	{
		var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == _userId);
		if (teacher == null)
		{
			return RedirectToAction("Index");
		}
		return View();
	}

	[HttpPost]
	public async Task<IActionResult> CreateScience(CreateScienceModel createScience)
	{
		if (!ModelState.IsValid)
		{
			return View(createScience);
		}

		var science = new Science()
		{
			Name = createScience.Name,
			TeacherId = _userId
		};

		_db.Sciences.Add(science);
		await _db.SaveChangesAsync();

		return RedirectToAction();
	}

	public async Task<IActionResult> GetScience(long scienceId)
	{
		var science = await _db.Sciences.FirstOrDefaultAsync(s => s.Id == scienceId);
		
		return View(science);
	}

	[Authorize]
	public async Task<IActionResult> GetSciences()
	{
		var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == _userId);
		if (teacher != null)
		{
			var sciences = await _db.Sciences.Where(s => s.TeacherId == teacher.Id).ToListAsync();
			return View(sciences);
		}

		var student = await _db.Students.Include(s => s.StudentSciences).FirstOrDefaultAsync(s => s.Id == _userId);
		if (student != null)
		{
			var sciencesId = student.StudentSciences!.Select(s => s.ScienceId).ToList();
			var sciences = await _db.Sciences.Where(s => s.Id == sciencesId.FirstOrDefault(s.Id)).ToListAsync();

			//var sciencesId = student.StudentSciences;
			//var sciences = await _db.Sciences.Where(s => s.Id == Convert.ToInt64(sciencesId!.Select(s => s.ScienceId))).ToListAsync();
			return View(sciences);
		}

		return View();
	}

	[HttpGet]
	public IActionResult AddStudent()
	{
		return View();
	}

	[Authorize]
	[HttpPost]
	public async Task<IActionResult> AddStudent(long scienceId, long studentId)
	{
		var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == _userId);
		var science = await _db.Sciences.FirstOrDefaultAsync(s => s.Id == scienceId);
		if (teacher == null || science == null)
		{
			return View("Index");
		}

		var studentScience = new StudentSciences()
		{
			StudentId = studentId,
			ScienceId = scienceId
		};

		_db.StudentSciences.Add(studentScience);
		await _db.SaveChangesAsync();

		return RedirectToAction("GetSciences");
	}

	[HttpGet]
	public IActionResult Mark() 
	{
		return View();
	}

	[HttpPost]
	[Authorize]
	public async Task<IActionResult> Mark(long scienceId, long studentId, int grade)
	{
		var teacher = await _db.Teachers.FirstOrDefaultAsync(t => t.Id == scienceId);
		if (teacher == null)
		{
			return View("Index");
		}

		var gradiate = new Gradiate()
		{
			StudentId = studentId,
			ScienceId = scienceId,
			Grade = grade
		};

		_db.Gradiates.Add(gradiate);
		await _db.SaveChangesAsync();

		return RedirectToAction();
	}


}
