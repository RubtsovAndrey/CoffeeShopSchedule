using System;

namespace CoffeeShop.Domain
{
    public class Shift
    {
        public string EmployeeName { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }

        public Shift(string employeeName, DateTime startTime, DateTime endTime)
        {
            EmployeeName = employeeName;
            StartTime = startTime;
            EndTime = endTime;
        }

        public TimeSpan Duration => EndTime - StartTime;

        public bool IsValidDuration()
        {
            var hours = Duration.TotalHours;
            return hours >= 2 && hours <= 12;
        }
    }
}
