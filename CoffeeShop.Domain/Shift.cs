using System;

namespace CoffeeShop.Domain
{
    public class Shift
    {
        public Guid Id { get; private set; } 
        
        // 1. Добавляем private set; ко всем свойствам
        public string EmployeeName { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public TimeSpan LunchBreak { get; private set; }

        // 2. Скрытый пустой конструктор. 
        // Мы (программисты) не сможем им воспользоваться, а EF Core — сможет!
        private Shift() 
        { 
            EmployeeName = null!; // Восклицательный знак отключает панику компилятора
        }

        // 3. Наш основной рабочий конструктор остается без изменений
        public Shift(string employeeName, DateTime startTime, DateTime endTime, TimeSpan lunchBreak = default)
        {
            Id = Guid.NewGuid();
            EmployeeName = employeeName;
            StartTime = startTime;
            EndTime = endTime;
            LunchBreak = lunchBreak;
        }

        public TimeSpan Duration => (EndTime - StartTime) - LunchBreak;

        public bool IsValidDuration()
        {
            var hours = Duration.TotalHours;
            return hours >= 2 && hours <= 12;
        }
    }
}
