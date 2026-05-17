using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using CoffeeShop.Domain;
using System;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Наш первый HTTP-эндпоинт. Сюда менеджер будет слать данные смены для проверки
app.MapPost("/api/shifts/validate", (ShiftDto dto) =>
{
    try
    {
        // Переводим текстовые данные из запроса в наши строгие объекты Domain
        var shift = new Shift(dto.EmployeeName, dto.StartTime, dto.EndTime, dto.LunchBreak);
        
        // Вызываем бизнес-логику, которую мы написали и протестировали ранее
        var isValid = shift.IsValidDuration();

        if (isValid)
        {
            return Results.Ok(new { Message = $"Смена для {dto.EmployeeName} валидна. Длительность: {shift.Duration.TotalHours} ч." });
        }
        else
        {
            return Results.BadRequest(new { Message = "Ошибка: Смена должна быть от 2 до 12 рабочих часов (с учетом обеда)!" });
        }
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});

app.Run();

// Специальный объект (Data Transfer Object) для приема данных из интернета
public record ShiftDto(string EmployeeName, DateTime StartTime, DateTime EndTime, TimeSpan LunchBreak);
