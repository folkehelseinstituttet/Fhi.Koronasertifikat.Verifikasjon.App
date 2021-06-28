using AiForms.Dialogs.Abstractions;

namespace FHICORC.ViewModels.Custom
{
    public class CustomDialogViewModel
    {
        public IDialogNotifier Notifier { get; set; }
        public CustomDialogViewModel(string title, string body, bool isCanceledOnTouchOutside, string okButtonText = null, string cancelButtonText = null)
        {
            Title = title;
            Body = body;
            OkButtonText = okButtonText;
            CancelButtonText = cancelButtonText;
            CancelButtonVisible = CancelButtonText != null;
            IsCanceledOnTouchOutside = isCanceledOnTouchOutside;
        }

        public string Title { get; }
        public string Body { get; }
        public string OkButtonText { get; }
        public string CancelButtonText { get; }
        public bool IsCanceledOnTouchOutside { get; }
        public bool CancelButtonVisible { get; }

        public void Complete()
        {
            Notifier.Complete();
        }
        public void Cancel()
        {
            Notifier.Cancel();
        }
    }
}
