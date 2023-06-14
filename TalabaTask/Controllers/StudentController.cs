using Microsoft.AspNetCore.Mvc;

namespace TalabaTask.Controllers
{
	public class StudentController : Controller
	{
		public async Task<IActionResult> Register()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login() 
		{
			return Ok();
		}
	}
}
