using walkeasyfinal.Models;
using Microsoft.EntityFrameworkCore;
using walkeasyfinal;

var builder = WebApplication.CreateBuilder(args);

// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<PaymentService>();
builder.Services.AddSession(); // <-- Moved here

// Session configuration
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Exception handling and HSTS for non-development environments
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Session middleware should only be used once
app.UseSession();

// Default routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=welcome}/{id?}");

app.Run();
