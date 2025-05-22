using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ChessAppNew.Models;

public class ReflectionService
{
    public IEnumerable<Type> LoadClassesFromAssembly(string assemblyPath)
    {
        if (!File.Exists(assemblyPath))
            throw new FileNotFoundException("Указанная библиотека не найдена", assemblyPath);

        try
        {
            var assembly = Assembly.LoadFrom(assemblyPath);
            Console.WriteLine($"Загружена сборка: {assembly.FullName}");

            var types = assembly.GetTypes();
            Console.WriteLine($"Найдено типов: {types.Length}");

            var implementingTypes = types.Where(t =>
                t.GetInterfaces().Any(i => i.Name == "IDynamicClass") &&
                !t.IsAbstract &&
                !t.IsInterface)
                .ToList();

            Console.WriteLine($"Найдено классов, реализующих IDynamicClass: {implementingTypes.Count}");
            foreach (var type in implementingTypes)
            {
                Console.WriteLine($"- {type.FullName}");
            }

            return implementingTypes;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке сборки: {ex.Message}");
            throw;
        }
    }

    public IEnumerable<MethodInfo> GetPublicMethods(Type type)
    {
        return type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => !m.IsSpecialName) // Исключаем свойства и события
            .ToList();
    }

    public object CreateInstance(Type type)
    {
        var instance = Activator.CreateInstance(type);
        if (instance == null)
            throw new InvalidOperationException($"Не удалось создать экземпляр типа {type.FullName}");
        return instance;
    }

    public object? InvokeMethod(object instance, MethodInfo method, object?[]? parameters)
    {
        try
        {
            return method.Invoke(instance, parameters);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при вызове метода: {ex.Message}");
            throw;
        }
    }
}