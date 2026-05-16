using System;
using Xunit;
using CoffeeShop.Domain;

namespace CoffeeShop.Tests
{
    public class ShiftTests
    {
        [Fact]
        public void IsValidDuration_ShiftIs8Hours_ReturnsTrue()
        {
            // Подготовка (Arrange)
            var start = new DateTime(2026, 5, 20, 9, 0, 0); 
            var end = new DateTime(2026, 5, 20, 17, 0, 0);  
            var shift = new Shift("Иван", start, end);

            // Действие (Act)
            var isValid = shift.IsValidDuration();

            // Проверка (Assert)
            Assert.True(isValid);
        }

        [Fact]
        public void IsValidDuration_ShiftIs14Hours_ReturnsFalse()
        {
            // Подготовка (Arrange)
            var start = new DateTime(2026, 5, 20, 8, 0, 0); 
            var end = new DateTime(2026, 5, 20, 22, 0, 0);  
            var shift = new Shift("Анна", start, end);

            // Действие (Act)
            var isValid = shift.IsValidDuration();

            // Проверка (Assert)
            Assert.False(isValid);
        }
    }
}
