using System;

namespace ChessApp.Models
{
    public class ExampleClass : IDynamicClass
    {
        private string _name;
        private int _value;

        public ExampleClass()
        {
            _name = "Default";
            _value = 0;
        }

        public string ClassName => "ExampleClass";
        public string Description => "Пример класса для демонстрации";

        public string GetName()
        {
            return _name;
        }

        public void SetName(string name)
        {
            _name = name;
        }

        public int GetValue()
        {
            return _value;
        }

        public void SetValue(int value)
        {
            _value = value;
        }

        public string CombineNameAndValue()
        {
            return $"{_name}: {_value}";
        }
    }
}