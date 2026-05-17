using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain;
using CoffeeShop.Api;
using System;
using System.Linq; // Нужно для работы со списками

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=coffeeshop.db"));

var app = builder.Build();

// 1. POST: Создать и СОХРАНИТЬ смену
// Обрати внимание: мы добавили AppDbContext прямо в параметры. .NET сам подставит базу данных!
app.MapPost("/api/shifts", (ShiftDto dto, AppDbContext db) =>
{
    var shift = new Shift(dto.EmployeeName, dto.StartTime, dto.EndTime, dto.LunchBreak);
    
    if (!shift.IsValidDuration())
    {
        return Results.BadRequest(new { Message = "Ошибка: Смена должна быть от 2 до 12 рабочих часов!" });
    }

    // Сохраняем в базу данных
    db.Shifts.Add(shift);
    db.SaveChanges(); // Только в этот момент данные физически пишутся в файл

    return Results.Ok(new { Message = "Смена успешно сохранена!", ShiftId = shift.Id });
});

// 2. GET: Получить ВСЕ смены из базы
app.MapGet("/api/shifts", (AppDbContext db) =>
{
    // Берем таблицу Shifts и превращаем её в список
    var allShifts = db.Shifts.ToList();
    return Results.Ok(allShifts);
});

app.Run();

public record ShiftDto(string EmployeeName, DateTime StartTime, DateTime EndTime, TimeSpan LunchBreak);
