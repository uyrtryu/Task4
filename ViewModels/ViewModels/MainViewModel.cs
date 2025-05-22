using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using ChessApp.Models;
using CommunityToolkit.Mvvm.Input;

namespace ChessApp.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly ReflectionService _reflectionService;
        private string? _assemblyPath;
        private Type? _selectedClass;
        private MethodInfo? _selectedMethod;
        private string? _result;
        private ObservableCollection<Type> _availableClasses;
        private ObservableCollection<MethodInfo> _availableMethods;
        private ObservableCollection<ParameterViewModel> _methodParameters;

        public MainViewModel()
        {
            _reflectionService = new ReflectionService();
            _availableClasses = new ObservableCollection<Type>();
            _availableMethods = new ObservableCollection<MethodInfo>();
            _methodParameters = new ObservableCollection<ParameterViewModel>();

            LoadAssemblyCommand = new RelayCommand(LoadAssembly);
            ExecuteMethodCommand = new RelayCommand(ExecuteMethod);
        }

        public string? AssemblyPath
        {
            get => _assemblyPath;
            set => SetProperty(ref _assemblyPath, value);
        }

        public Type? SelectedClass
        {
            get => _selectedClass;
            set
            {
                if (SetProperty(ref _selectedClass, value) && value != null)
                {
                    LoadMethods(value);
                }
            }
        }

        public MethodInfo? SelectedMethod
        {
            get => _selectedMethod;
            set
            {
                if (SetProperty(ref _selectedMethod, value) && value != null)
                {
                    LoadParameters(value);
                }
            }
        }

        public string? Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        public ObservableCollection<Type> AvailableClasses
        {
            get => _availableClasses;
            set => SetProperty(ref _availableClasses, value);
        }

        public ObservableCollection<MethodInfo> AvailableMethods
        {
            get => _availableMethods;
            set => SetProperty(ref _availableMethods, value);
        }

        public ObservableCollection<ParameterViewModel> MethodParameters
        {
            get => _methodParameters;
            set => SetProperty(ref _methodParameters, value);
        }

        public ICommand LoadAssemblyCommand { get; }
        public ICommand ExecuteMethodCommand { get; }

        private void LoadAssembly()
        {
            if (string.IsNullOrEmpty(AssemblyPath))
            {
                Result = "Укажите путь к библиотеке";
                return;
            }

            try
            {
                var classes = _reflectionService.LoadClassesFromAssembly(AssemblyPath);
                AvailableClasses.Clear();
                foreach (var classType in classes)
                {
                    AvailableClasses.Add(classType);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Result = $"Ошибка загрузки сборки: {ex.Message}";
            }
        }

        private void LoadMethods(Type classType)
        {
            AvailableMethods.Clear();
            var methods = _reflectionService.GetPublicMethods(classType);
            foreach (var method in methods)
            {
                AvailableMethods.Add(method);
            }
        }

        private void LoadParameters(MethodInfo method)
        {
            MethodParameters.Clear();
            foreach (var parameter in method.GetParameters())
            {
                MethodParameters.Add(new ParameterViewModel
                {
                    Name = parameter.Name,
                    Type = parameter.ParameterType,
                    Value = null
                });
            }
        }

        private void ExecuteMethod()
        {
            if (SelectedClass == null || SelectedMethod == null)
            {
                Result = "Выберите класс и метод";
                return;
            }

            try
            {
                var instance = _reflectionService.CreateInstance(SelectedClass);
                var parameters = MethodParameters.Select(p => p.Value).ToArray();
                var result = _reflectionService.InvokeMethod(instance, SelectedMethod, parameters);
                Result = result?.ToString() ?? "Метод выполнен успешно";
            }
            catch (Exception ex)
            {
                Result = $"Ошибка выполнения метода: {ex.Message}";
            }
        }
    }

    public class ParameterViewModel : ViewModelBase
    {
        private string? _name;
        private Type? _type;
        private object? _value;

        public string? Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public Type? Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public object? Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }
    }
}