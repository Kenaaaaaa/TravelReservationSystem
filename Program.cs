using Microsoft.EntityFrameworkCore;
using TravelReservationSystem.Data;

var builder = WebApplication.CreateBuilder(args);

// 1️⃣ Regjistrimi i shërbimeve
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 34))
    ));

builder.Services.AddControllersWithViews();

// 🔑 Shto këtë për të aktivizuar session
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// 2️⃣ Middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

// 🧠 Shto këtë përpara autorizimit
app.UseSession(); // Aktivizon sessionet për HttpContext

app.UseAuthentication();
app.UseAuthorization();

// 3️⃣ Rrutimi
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
