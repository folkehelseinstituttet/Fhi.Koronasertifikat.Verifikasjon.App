using System;
using System.Collections.Generic;
using AiForms.Dialogs.Abstractions;
using FHICORC.ViewModels.Custom;
using Xamarin.Forms;

namespace FHICORC.Views.Elements
{
    public partial class LanguageSelectionDialog : DialogView
    {
        public LanguageSelectionDialog()
        {
            InitializeComponent();
            IsCanceledOnTouchOutside = false;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext is LanguageSelectionDialogViewModel viewModel)
            {
                IsCanceledOnTouchOutside = viewModel.IsCanceledOnTouchOutside;
            }
        }

        void Cancel(System.Object sender, System.EventArgs e)
        {
            ((LanguageSelectionDialogViewModel)BindingContext).Cancel();
        }
        void Complete(System.Object sender, System.EventArgs e)
        {
            ((LanguageSelectionDialogViewModel)BindingContext).Complete();
        }

        void TapGestureRecognizer_Tapped_Bokmal(object sender, EventArgs e)
        {
            if (!RadioButtonBokmal.IsChecked)
            {
                RadioButtonBokmal.IsChecked = true;
            }
        }

        void TapGestureRecognizer_Tapped_Nynorsk(object sender, EventArgs e)
        {
            if (!RadioButtonNynorsk.IsChecked)
            {
                RadioButtonNynorsk.IsChecked = true;
            }
        }

        void TapGestureRecognizer_Tapped_English(object sender, EventArgs e)
        {
            if (!RadioButtonEnglish.IsChecked)
            {
                RadioButtonEnglish.IsChecked = true;
            }
        }
    }
}
