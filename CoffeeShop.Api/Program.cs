using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CoffeeShop.Domain;
using CoffeeShop.Api;
using System;
using System.Linq; // Нужно для работы со списками

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Просим фреймворк достать строку из секции ConnectionStrings -> DefaultConnection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

var app = builder.Build();

// НАША ЛОВУШКА ДЛЯ ОШИБОК (Middleware)
app.Use(async (context, next) =>
{
    try
    {
        // Пропускаем запрос дальше по трубе к нашим эндпоинтам
        await next(); 
    }
    catch (Exception ex)
    {
        // Если кто-то внутри трубы упал, мы ловим ошибку здесь!
        
        // 1. Пишем подробности в лог (для разработчиков и саппорта)
        app.Logger.LogError(ex, "КРИТИЧЕСКИЙ СБОЙ СИСТЕМЫ: Произошла непредвиденная ошибка!");

        // 2. Формируем вежливый ответ (для пользователя)
        context.Response.StatusCode = 500; // 500 - Internal Server Error
        await context.Response.WriteAsJsonAsync(new { Message = "Упс! Что-то сломалось на нашей стороне. Техподдержка уже разбужена и чинит!" });
    }
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseStaticFiles();

// 1. Добавили слово "async" перед параметрами
app.MapPost("/api/shifts", async (ShiftDto dto, AppDbContext db) =>
{
    var shift = new Shift(dto.EmployeeName, dto.StartTime, dto.EndTime, dto.LunchBreak);
    
    if (!shift.IsValidDuration())
    {
        return Results.BadRequest(new { Message = "Ошибка: Смена должна быть от 2 до 12 рабочих часов!" });
    }

    db.Shifts.Add(shift);
    
    // 2. Добавили "await" перед сохранением и вызвали асинхронную версию метода
    await db.SaveChangesAsync(); 

    return Results.Ok(new { Message = "Смена успешно сохранена!", ShiftId = shift.Id });
});

// 3. Здесь тоже добавили "async"
app.MapGet("/api/shifts", async (AppDbContext db) =>
{
    // 4. Добавили "await" и используем ToListAsync() вместо обычного ToList()
    var allShifts = await db.Shifts.ToListAsync();
    return Results.Ok(allShifts);
});

app.MapDelete("/api/shifts/{id}", async (Guid id, AppDbContext db) =>
{
    // 1. Ищем смену в базе по ID
    var shift = await db.Shifts.FindAsync(id);
    
    // 2. Если такой смены нет, возвращаем ошибку 404 (Not Found)
    if (shift is null)
    {
        return Results.NotFound(new { Message = "Смена с таким ID не найдена!" });
    }

    // 3. Если нашли - удаляем и сохраняем изменения
    db.Shifts.Remove(shift);
    await db.SaveChangesAsync();

    return Results.Ok(new { Message = $"Смена {id} успешно удалена!" });
});

app.MapGet("/api/crash", () =>
{
    // Имитируем жесткое падение кода
    throw new Exception("База данных взорвалась!");
});

app.Run();

public record ShiftDto(string EmployeeName, DateTime StartTime, DateTime EndTime, TimeSpan LunchBreak);
