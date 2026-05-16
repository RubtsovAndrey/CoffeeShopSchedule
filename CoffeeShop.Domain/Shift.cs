using System;

namespace CoffeeShop.Domain
{
    public class Shift
    {
        public string EmployeeName { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public TimeSpan LunchBreak { get; } // Новое свойство для обеда

        // Добавили lunchBreak в конструктор с дефолтным значением
        public Shift(string employeeName, DateTime startTime, DateTime endTime, TimeSpan lunchBreak = default)
        {
            EmployeeName = employeeName;
            StartTime = startTime;
            EndTime = endTime;
            LunchBreak = lunchBreak;
        }

        // Длительность теперь: (Конец - Начало) минус Обед
        public TimeSpan Duration => (EndTime - StartTime) - LunchBreak;

        public bool IsValidDuration()
        {
            var hours = Duration.TotalHours;
            return hours >= 2 && hours <= 12;
        }
    }
}
