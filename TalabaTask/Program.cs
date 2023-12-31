using Microsoft.EntityFrameworkCore;
using TalabaTask.Context;
using TalabaTask.Extensions;
using TalabaTask.Providers;
using TalabaTask.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("TalabaDb"));
});
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddScoped<JwtServiceTeacher>();
builder.Services.AddScoped<JwtServiceStudent>();
builder.Services.AddScoped<UserProvider>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
