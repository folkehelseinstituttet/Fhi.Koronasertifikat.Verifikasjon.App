using System;
using System.Collections.Generic;
using AiForms.Dialogs.Abstractions;
using FHICORC.ViewModels.Custom;
using Xamarin.Forms;

namespace FHICORC.Views.Elements
{
    public partial class RootDetectionDialog : DialogView
    {
        public RootDetectionDialog()
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
