using System;

namespace ChessApp.Models
{
    public interface IDynamicClass
    {
        string ClassName { get; }
        string Description { get; }
    }
}