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

		return RedirectToAction("GetSciences");
	}

	public async Task<IActionResult> GetScience(long scienceId)
	{
		var science = await _db.Sciences.Include(s => s.StudentSciences).ThenInclude(s => s.Student).FirstOrDefaultAsync(s => s.Id == scienceId);
		var result = await _db.Sciences.Include(s => s.Gradiates).FirstOrDefaultAsync(s => s.Id == scienceId);
		ViewBag.Gradiates = result.Gradiates;

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
	public IActionResult Mark(long scienceId, long studentId) 
	{
		ViewBag.ScienceId = scienceId;
		ViewBag.StudentId = studentId;

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

		return RedirectToAction("GetSciences");
	}

	public async Task<IActionResult> GetScienceStudentIsMostGrade(long studentId)
	{
		var gradiate = await _db.Gradiates
			.Where(g => g.StudentId == studentId)
			.OrderByDescending(g => g.Grade).FirstAsync();

		var science = await _db.Sciences.Include(s => s.Gradiates).FirstOrDefaultAsync(s => s.Id == gradiate.ScienceId);
		ViewBag.Gradiate = gradiate;
		return View(science);
	}

	public async Task<IActionResult> GetScienceTop10Student(long teacherId)
	{
		var sciences = await _db.Sciences.Include(s => s.Gradiates).ThenInclude(g => g.Student)
			.Where(s => s.TeacherId == teacherId).ToListAsync();

		Science? science = sciences.Where(s => s.Gradiates?.Where(g => g.Grade > 80).Count() > 9).FirstOrDefault();

		return View(science);
	}

	//shuni page qoldi
	public async Task<IActionResult> GetScienceMostAverage()
	{
		var science = await _db.Sciences.Include(s => s.Gradiates)
			.OrderByDescending(s => s.Gradiates!.Average(g => g.Grade))
			.FirstAsync();

		return View(science);
	}
}
