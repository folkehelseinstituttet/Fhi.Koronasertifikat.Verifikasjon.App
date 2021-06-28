using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels;

namespace FHICORC.Tests.TestMocks
{
    public class MockDialogService : IDialogService
    {
        public MockDialogService()
        {
        }

        public Task<bool> ShowAlertAsync(string title, string message, string accept, string cancel)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ShowAlertWithoutTouchOutsideAsync(string title, string message, string accept, string cancel)
        {
            return Task.FromResult(true);
        }

        public Task<bool> ShowLanguageSelectionDialog(string title, string message, string accept)
        {
            return Task.FromResult(true);
        }

        public Task ShowPicker(IEnumerable<ISelection> itemSource, Action<ISelection> onItemPickedAction, string title)
        {
            return Task.FromResult(true);
        }
    }
}
