using System;
namespace FHICORC.ViewModels
{
    public interface ISelection : IControlWithText
    {
        bool IsSelected { get; set; }
        string AccessibilityText { get; }
    }
}
