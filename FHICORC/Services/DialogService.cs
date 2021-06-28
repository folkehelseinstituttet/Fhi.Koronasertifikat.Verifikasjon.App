using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AiForms.Dialogs;
using FHICORC.Services.Interfaces;
using FHICORC.ViewModels;
using FHICORC.ViewModels.Custom;
using FHICORC.Views.Elements;

namespace FHICORC.Services
{
    public class DialogService : IDialogService
    {
        
        public DialogService()
        {
        }

        public async Task<bool> ShowAlertAsync(string title, string message, string accept, string cancel)
        {
            if (string.IsNullOrEmpty(accept))
            {
                accept = "DIALOG_ACCEPT_BUTTON".Translate();
            }

            bool isCanceledOnTouchOutside = true;
            CustomDialogViewModel viewmodel = new CustomDialogViewModel(title, message, isCanceledOnTouchOutside, accept, cancel);
            return await Dialog.Instance.ShowAsync<CustomDialog>(viewmodel);
        }

        public async Task<bool> ShowLanguageSelectionDialog(string title, string message, string accept)
        {
            bool isCanceledOnTouchOutside = true;
            LanguageSelectionDialogViewModel viewmodel = new LanguageSelectionDialogViewModel(title, message, isCanceledOnTouchOutside, accept);
            return await Dialog.Instance.ShowAsync<LanguageSelectionDialog>(viewmodel);
        }

        public async Task<bool> ShowAlertWithoutTouchOutsideAsync(string title, string message, string accept, string cancel)
        {
            if (string.IsNullOrEmpty(accept))
            {
                accept = "DIALOG_ACCEPT_BUTTON".Translate();
            }

            bool isCanceledOnTouchOutside = false;
            CustomDialogViewModel viewmodel = new CustomDialogViewModel(title, message, isCanceledOnTouchOutside, accept, cancel);
            return await Dialog.Instance.ShowAsync<RootDetectionDialog>(viewmodel);
        }
    }
}