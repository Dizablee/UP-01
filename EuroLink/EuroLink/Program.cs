using Microsoft.EntityFrameworkCore;
using EuroLink.Data;
using EuroLink.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Используем InMemory базу - не требует установки пакетов
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EuroLink"));

builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();
builder.Services.AddScoped<PhoneService>();

var app = builder.Build();

// АВТОМАТИЧЕСКОЕ СОЗДАНИЕ БАЗЫ ДАННЫХ
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages();

app.Run();