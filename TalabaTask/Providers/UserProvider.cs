using System.Security.Claims;

namespace TalabaTask.Providers;

public class UserProvider
{
	private readonly IHttpContextAccessor _contextAccessor;

	public UserProvider(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	protected HttpContext? Context => _contextAccessor.HttpContext;

	public long UserId => Convert.ToInt64(Context.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
