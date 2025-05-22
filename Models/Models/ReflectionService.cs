using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace ChessApp.Models
{
    public class ReflectionService
    {
        public IEnumerable<Type> LoadClassesFromAssembly(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
                throw new FileNotFoundException("Указанная библиотека не найдена", assemblyPath);

            try
            {
                // Загружаем сборку
                var assembly = Assembly.LoadFrom(assemblyPath);
                Console.WriteLine($"Загружена сборка: {assembly.FullName}");

                // Получаем все типы
                var types = assembly.GetTypes();
                Console.WriteLine($"Найдено типов в сборке: {types.Length}");

                // Фильтруем типы, реализующие IDynamicClass
                var implementingTypes = new List<Type>();
                foreach (var type in types)
                {
                    try
                    {
                        if (type.IsClass && !type.IsAbstract)
                        {
                            var interfaces = type.GetInterfaces();
                            if (interfaces.Any(i => i.FullName == typeof(IDynamicClass).FullName))
                            {
                                implementingTypes.Add(type);
                                Console.WriteLine($"Найден класс: {type.FullName}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при проверке типа {type.FullName}: {ex.Message}");
                    }
                }

                Console.WriteLine($"Всего найдено классов, реализующих IDynamicClass: {implementingTypes.Count}");
                return implementingTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine("Ошибка загрузки типов:");
                if (ex.LoaderExceptions != null)
                {
                    foreach (var loaderException in ex.LoaderExceptions)
                    {
                        if (loaderException != null)
                        {
                            Console.WriteLine($"- {loaderException.Message}");
                        }
                    }
                }
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Общая ошибка при загрузке сборки: {ex}");
                throw;
            }
        }

        public IEnumerable<MethodInfo> GetPublicMethods(Type classType)
        {
            try
            {
                var methods = classType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => !m.IsSpecialName)
                    .ToList();

                Console.WriteLine($"Найдено публичных методов в классе {classType.Name}: {methods.Count}");
                foreach (var method in methods)
                {
                    Console.WriteLine($"- {method.Name}");
                }

                return methods;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при получении методов класса {classType.Name}: {ex}");
                throw;
            }
        }

        public object CreateInstance(Type classType)
        {
            try
            {
                var instance = Activator.CreateInstance(classType);
                if (instance == null)
                {
                    throw new InvalidOperationException($"Не удалось создать экземпляр типа {classType.Name}");
                }
                return instance;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при создании экземпляра класса {classType.Name}: {ex}");
                throw;
            }
        }

        public object? InvokeMethod(object instance, MethodInfo method, object?[] parameters)
        {
            try
            {
                return method.Invoke(instance, parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при вызове метода {method.Name}: {ex}");
                throw;
            }
        }
    }
}