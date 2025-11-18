using Microsoft.EntityFrameworkCore;
using EuroLink.Data;
using EuroLink.Services;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddRazorPages();

// Настройка InMemory базы данных
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("EuroLink"));

// Регистрация сервисов
builder.Services.AddScoped<IPhoneRepository, PhoneRepository>();
builder.Services.AddScoped<PhoneService>();

var app = builder.Build();

// Автоматическое создание базы данных при запуске
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.EnsureCreated();
}

// Конфигурация HTTP pipeline
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