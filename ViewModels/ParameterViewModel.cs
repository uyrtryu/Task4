using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ChessAppNew.ViewModels;

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