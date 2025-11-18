using Microsoft.EntityFrameworkCore;
using EuroLink.Data;
using EuroLink.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Настройка SQLite базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация сервисов
builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();
builder.Services.AddScoped<PhoneService>();

var app = builder.Build();

// Создание базы при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
