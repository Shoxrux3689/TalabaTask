using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TalabaTask.Context;
using TalabaTask.Entities;
using TalabaTask.Models;

namespace TalabaTask.Controllers
{
	public class ScienceController : Controller
	{
		private readonly AppDbContext _db;
		public ScienceController(AppDbContext db)
		{
			_db = db;
		}

		public async Task<IActionResult> CreateScience(CreateScienceModel createScience)
		{
			if (!ModelState.IsValid)
			{
				return View(createScience);
			}

			long userId = 0;
			var science = new Science()
			{
				Name = createScience.Name,
				TeacherId = userId
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

	}
}
