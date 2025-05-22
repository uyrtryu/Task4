using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using ChessAppNew.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace ChessAppNew.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly ReflectionService _reflectionService;
    private string? _assemblyPath;
    private Type? _selectedClass;
    private MethodInfo? _selectedMethod;
    private object? _classInstance;
    private ObservableCollection<ParameterViewModel> _parameters;

    public MainWindowViewModel()
    {
        _reflectionService = new ReflectionService();
        _parameters = new ObservableCollection<ParameterViewModel>();
        LoadAssemblyCommand = new RelayCommand(LoadAssembly);
        ExecuteMethodCommand = new RelayCommand(ExecuteMethod, CanExecuteMethod);
    }

    public string? AssemblyPath
    {
        get => _assemblyPath;
        set => SetProperty(ref _assemblyPath, value);
    }

    public ObservableCollection<Type> Classes { get; } = new();
    public ObservableCollection<MethodInfo> Methods { get; } = new();
    public ObservableCollection<ParameterViewModel> Parameters
    {
        get => _parameters;
        set => SetProperty(ref _parameters, value);
    }

    public Type? SelectedClass
    {
        get => _selectedClass;
        set
        {
            if (SetProperty(ref _selectedClass, value))
            {
                LoadMethods();
            }
        }
    }

    public MethodInfo? SelectedMethod
    {
        get => _selectedMethod;
        set
        {
            if (SetProperty(ref _selectedMethod, value))
            {
                LoadParameters();
            }
        }
    }

    public ICommand LoadAssemblyCommand { get; }
    public ICommand ExecuteMethodCommand { get; }

    private void LoadAssembly()
    {
        if (string.IsNullOrEmpty(AssemblyPath))
        {
            return;
        }

        try
        {
            var classes = _reflectionService.LoadClassesFromAssembly(AssemblyPath);
            Classes.Clear();
            foreach (var type in classes)
            {
                Classes.Add(type);
            }
        }
        catch (Exception ex)
        {
            // TODO: Добавить обработку ошибок
            Console.WriteLine($"Ошибка загрузки сборки: {ex.Message}");
        }
    }

    private void LoadMethods()
    {
        Methods.Clear();
        if (SelectedClass == null) return;

        var methods = _reflectionService.GetPublicMethods(SelectedClass);
        foreach (var method in methods)
        {
            Methods.Add(method);
        }
    }

    private void LoadParameters()
    {
        Parameters.Clear();
        if (SelectedMethod == null) return;

        var parameters = SelectedMethod.GetParameters();
        foreach (var parameter in parameters)
        {
            Parameters.Add(new ParameterViewModel
            {
                Name = parameter.Name ?? "Unknown",
                Type = parameter.ParameterType,
                Value = null
            });
        }
    }

    private bool CanExecuteMethod()
    {
        return SelectedMethod != null && Parameters.All(p => p.Value != null);
    }

    private void ExecuteMethod()
    {
        if (SelectedClass == null || SelectedMethod == null) return;

        try
        {
            if (_classInstance == null)
            {
                _classInstance = _reflectionService.CreateInstance(SelectedClass);
            }

            var parameterValues = Parameters.Select(p => p.Value).ToArray();
            var result = _reflectionService.InvokeMethod(_classInstance, SelectedMethod, parameterValues);

            // TODO: Показать результат выполнения метода
            Console.WriteLine($"Результат: {result}");
        }
        catch (Exception ex)
        {
            // TODO: Добавить обработку ошибок
            Console.WriteLine($"Ошибка выполнения метода: {ex.Message}");
        }
    }
}
