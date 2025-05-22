using System;

namespace ChessApp.Models
{
    public class TestClass : IDynamicClass
    {
        public string ClassName => "TestClass";
        public string Name => "Тестовый класс";
        public string Description => "Класс для тестирования рефлексии";

        public string TestMethod(string input)
        {
            return $"Обработано: {input}";
        }

        public int AddNumbers(int a, int b)
        {
            return a + b;
        }

        public void PrintMessage(string message)
        {
            Console.WriteLine($"Сообщение: {message}");
        }
    }
}