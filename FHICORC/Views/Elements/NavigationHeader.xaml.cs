using System.Runtime.CompilerServices;
using FHICORC.Services;
using FHICORC.Utils;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views.Elements
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigationHeader : Grid
    {
        public NavigationHeader()
        {
            InitializeComponent();
            HeaderBackgroundColour = FHICORCColor.NavigationHeaderBackgroundColor.Color();
            LeftButtonHeightRequest = 30;
            CenterLabelHeightRequest = 40;
            RightButtonHeightRequest = 30;

            CenterLabel.Text = "LANDING_PAGE_TITLE".Translate();


            AutomationProperties.SetIsInAccessibleTree(CenterLabel, true);
            AutomationProperties.SetIsInAccessibleTree(LeftButton, false);
            AutomationProperties.SetIsInAccessibleTree(RightButton, false);
            LeftButtonAccessibilityText =
                "ACCESSIBILITY_BACK_BUTTON_HELP_TEXT".Translate();
            RightButtonAccessibilityText =
                "ACCESSIBILITY_LOGOUT_BUTTON_HELP_TEXT".Translate();
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == HeaderBackgroundColourProperty.PropertyName)
            {
                HeaderGrid.BackgroundColor = HeaderBackgroundColour;
            }
            else if (propertyName == LeftButtonImageSourceProperty.PropertyName)
            {
                LeftButton.Source = LeftButtonImageSource;
                LeftButton.IsVisible = true;
                AutomationProperties.SetIsInAccessibleTree(LeftButton, LeftButtonImageSource != null);
            }
            else if (propertyName == LeftButtonCommandProperty.PropertyName)
            {
                LeftButton.Command = LeftButtonCommand;
            }
            else if (propertyName == LeftButtonHeightRequestProperty.PropertyName)
            {
                LeftButton.HeightRequest = LeftButtonHeightRequest;
            }
            else if (propertyName == LeftButtonAccessibilityTextProperty.PropertyName)
            {
                AutomationProperties.SetName(LeftButton, LeftButtonAccessibilityText);
            }
            else if (propertyName == CenterLabelTextProperty.PropertyName)
            {
                CenterLabel.Text = CenterLabelText;
            }
            else if (propertyName == CenterLabelHeightRequestProperty.PropertyName)
            {
                CenterLabel.HeightRequest = CenterLabelHeightRequest;
            }
            else if (propertyName == RightButtonImageSourceProperty.PropertyName)
            {
                RightButton.Source = RightButtonImageSource;
                RightButton.IsVisible = true;
                AutomationProperties.SetIsInAccessibleTree(RightButton, RightButtonImageSource != null);
            }
            else if (propertyName == RightButtonCommandProperty.PropertyName)
            {
                RightButton.Command = RightButtonCommand;
            }
            else if (propertyName == RightButtonHeightRequestProperty.PropertyName)
            {
                RightButton.HeightRequest = RightButtonHeightRequest;
            }
            else if (propertyName == RightButtonAccessibilityTextProperty.PropertyName)
            {
                AutomationProperties.SetName(RightButton, RightButtonAccessibilityText);
            }
        }

        public static readonly BindableProperty HeaderBackgroundColourProperty =
            BindableProperty.Create(nameof(HeaderBackgroundColour), typeof(Color), typeof(Grid), null,
                BindingMode.OneWay);

        public Color HeaderBackgroundColour
        {
            get { return (Color)GetValue(HeaderBackgroundColourProperty); }
            set { SetValue(HeaderBackgroundColourProperty, value); }
        }


        public static readonly BindableProperty LeftButtonImageSourceProperty =
            BindableProperty.Create(nameof(LeftButtonImageSource), typeof(ImageSource), typeof(ImageButton),
                null, BindingMode.OneWay);

        public ImageSource LeftButtonImageSource
        {
            get { return (ImageSource)GetValue(LeftButtonImageSourceProperty); }
            set { SetValue(LeftButtonImageSourceProperty, value); }
        }


        public static readonly BindableProperty LeftButtonCommandProperty =
            BindableProperty.Create(nameof(LeftButtonCommand), typeof(Command), typeof(ImageButton), null,
                BindingMode.OneWay);

        public Command LeftButtonCommand
        {
            get { return (Command)GetValue(LeftButtonCommandProperty); }
            set { SetValue(LeftButtonCommandProperty, value); }
        }

        public static readonly BindableProperty LeftButtonHeightRequestProperty =
            BindableProperty.Create(nameof(LeftButtonHeightRequest), typeof(int), typeof(ImageButton), null,
                BindingMode.OneWay);

        public int LeftButtonHeightRequest
        {
            get { return (int)GetValue(LeftButtonHeightRequestProperty); }
            set { SetValue(LeftButtonHeightRequestProperty, value); }
        }

        public static readonly BindableProperty LeftButtonAccessibilityTextProperty =
            BindableProperty.Create(nameof(LeftButtonAccessibilityTextProperty), typeof(string), typeof(ImageButton),
                null, BindingMode.OneWay);

        public string LeftButtonAccessibilityText
        {
            get { return (string)GetValue(LeftButtonAccessibilityTextProperty); }
            set { SetValue(LeftButtonAccessibilityTextProperty, value); }
        }

        public static readonly BindableProperty CenterLabelTextProperty =
            BindableProperty.Create(nameof(CenterLabelText), typeof(string), typeof(Label), null,
                BindingMode.OneWay);

        public string CenterLabelText
        {
            get { return (string)GetValue(CenterLabelTextProperty); }
            set { SetValue(CenterLabelTextProperty, value); }
        }

        public static readonly BindableProperty CenterLabelHeightRequestProperty =
            BindableProperty.Create(nameof(CenterLabelHeightRequest), typeof(int), typeof(Image), null,
                BindingMode.OneWay);

        public int CenterLabelHeightRequest
        {
            get { return (int)GetValue(CenterLabelHeightRequestProperty); }
            set { SetValue(CenterLabelHeightRequestProperty, value); }
        }


        public static readonly BindableProperty RightButtonImageSourceProperty =
            BindableProperty.Create(nameof(RightButtonImageSource), typeof(ImageSource), typeof(ImageButton),
                null, BindingMode.OneWay);

        public ImageSource RightButtonImageSource
        {
            get { return (ImageSource)GetValue(RightButtonImageSourceProperty); }
            set { SetValue(RightButtonImageSourceProperty, value); }
        }


        public static readonly BindableProperty RightButtonCommandProperty =
            BindableProperty.Create(nameof(RightButtonCommand), typeof(Command), typeof(ImageButton), null,
                BindingMode.OneWay);

        public Command RightButtonCommand
        {
            get { return (Command)GetValue(RightButtonCommandProperty); }
            set { SetValue(RightButtonCommandProperty, value); }
        }

        public static readonly BindableProperty RightButtonHeightRequestProperty =
            BindableProperty.Create(nameof(RightButtonCommand), typeof(int), typeof(ImageButton), null,
                BindingMode.OneWay);

        public int RightButtonHeightRequest
        {
            get { return (int)GetValue(RightButtonHeightRequestProperty); }
            set { SetValue(RightButtonHeightRequestProperty, value); }
        }

        public static readonly BindableProperty RightButtonAccessibilityTextProperty =
            BindableProperty.Create(nameof(RightButtonAccessibilityTextProperty), typeof(string), typeof(ImageButton),
                null, BindingMode.OneWay);

        public string RightButtonAccessibilityText
        {
            get { return (string)GetValue(RightButtonAccessibilityTextProperty); }
            set { SetValue(RightButtonAccessibilityTextProperty, value); }
        }
    }
}