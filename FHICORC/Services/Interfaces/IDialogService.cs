using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.ViewModels;

namespace FHICORC.Services.Interfaces
{
    public interface IDialogService
    {
        // Alert that can be cancelled by tapping outside the alert.
        Task<bool> ShowAlertAsync(string title, string message, string accept, string cancel);

        Task<bool> ShowLanguageSelectionDialog(string title, string message, string accept);

        // Alert that cannot be cancelled by tapping outside the alert.
        Task<bool> ShowAlertWithoutTouchOutsideAsync(string title, string message, string accept, string cancel);
    }
}