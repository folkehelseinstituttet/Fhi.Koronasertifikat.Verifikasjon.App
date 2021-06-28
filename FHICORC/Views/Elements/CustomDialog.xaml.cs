using AiForms.Dialogs.Abstractions;
using FHICORC.ViewModels.Custom;
using FHICORC.Configuration;
using FHICORC.Services.Interfaces;
using FHICORC.Utils;

namespace FHICORC.Views.Elements
{
    public partial class CustomDialog : DialogView
    {
        public CustomDialog()
        {
            InitializeComponent();
            IsCanceledOnTouchOutside = false;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is CustomDialogViewModel viewModel)
            {
                IsCanceledOnTouchOutside = viewModel.IsCanceledOnTouchOutside;
            }
        }

        void Cancel(System.Object sender, System.EventArgs e)
        {
            ((CustomDialogViewModel)BindingContext).Cancel();
        }
        void Complete(System.Object sender, System.EventArgs e)
        {
            ((CustomDialogViewModel)BindingContext).Complete();
        }
    }
}
