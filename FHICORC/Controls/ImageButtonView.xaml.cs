using System;
using System.Collections.Generic;
using System.Windows.Input;
using FHICORC.Services;
using Xamarin.Forms;

namespace FHICORC.Controls
{
    public partial class ImageButtonView : ContentView
    {
        public ImageButtonView()
        {
            InitializeComponent();
        }

        #region ImageSource
        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
         propertyName: "Source",
         returnType: typeof(string),
         declaringType: typeof(ImageButtonView),
         defaultValue: "",
         defaultBindingMode: BindingMode.TwoWay,
         propertyChanged: ImageSourcePropertyChanged);

        private static void ImageSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ImageButtonView)bindable;
            control.imgSource.Source = ImageSource.FromFile(newValue.ToString());
        }
        public string Source
        {
            get { return base.GetValue(ImageSourceProperty).ToString(); }
            set { base.SetValue(ImageSourceProperty, value); }
        }
        #endregion

        #region LabelText
        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(
         propertyName: "Text",
         returnType: typeof(string),
         declaringType: typeof(ImageButtonView),
         defaultValue: "",
         defaultBindingMode: BindingMode.TwoWay,
         propertyChanged: LabelTextPropertyChanged);

        public static void LabelTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ImageButtonView control = (ImageButtonView)bindable;
            control.lblText.Text = newValue.ToString();

        }
        public string Text
        {
            get { return (string)base.GetValue(LabelTextProperty); }
            set { base.SetValue(LabelTextProperty, value); }
        }
        #endregion

        #region BackGroundColor
        // this  property used to set background color of frame
        public static new readonly BindableProperty BackgroundColorProperty = BindableProperty.Create(
            propertyName: "BackgroundColor",
            returnType: typeof(Color),
            declaringType: typeof(ImageButtonView),
            defaultValue: Color.White,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: BackgroundColorPropertyChanged);

        public Color BackgroundColor
        {
            get { return (Color)base.GetValue(BackgroundColorProperty); }
            set { base.SetValue(BackgroundColorProperty, value); }
        }

        private static void BackgroundColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            ImageButtonView control = (ImageButtonView)bindable;
            control.frmObject.BackgroundColor = (Color)newValue;
        }
        #endregion

        #region CornerRadius
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(
            propertyName: "CornerRadius",
            returnType: typeof(int),
            declaringType: typeof(ImageButtonView),
            defaultValue: 0,
            defaultBindingMode: BindingMode.TwoWay,
            propertyChanged: CornerRadiusPropertyChanged
            );

        public int CornerRadius
        {
            get { return (int)base.GetValue(CornerRadiusProperty); }
            set { base.SetValue(CornerRadiusProperty, value); }
        }

        private static void CornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ImageButtonView)bindable;
            control.frmObject.CornerRadius = (int)newValue;
        }
        #endregion

        #region ImageHeightWidthProperty

        public static readonly BindableProperty ImageHeightRequestProperty = BindableProperty.Create(
            propertyName: "ImageHeightRequest",
            returnType: typeof(double),
            declaringType: typeof(ImageButtonView),
            propertyChanged: ImageHeightRequestPropertyChanged
            );

        private static void ImageHeightRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ImageButtonView)bindable;
            control.imgSource.HeightRequest = (double)newValue;
        }

        public double ImageHeightRequest
        {
            get { return (double)base.GetValue(ImageHeightRequestProperty); }
            set { base.SetValue(ImageHeightRequestProperty, value); }
        }


        // For WidthRequest
        public static readonly BindableProperty ImageWidthRequestProperty = BindableProperty.Create(
          propertyName: "ImageHeightRequest",
          returnType: typeof(double),
          declaringType: typeof(ImageButtonView),
          propertyChanged: ImageWidthRequestPropertyChanged
          );

        private static void ImageWidthRequestPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ImageButtonView)bindable;
            control.imgSource.WidthRequest = (double)newValue;
        }

        public double ImageWidthRequest
        {
            get { return (double)base.GetValue(ImageWidthRequestProperty); }
            set { base.SetValue(ImageWidthRequestProperty, value); }
        }

        #endregion


        #region  property for Comamnd & Command Parameter

        // for command
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
         propertyName: "Command",
         returnType: typeof(ICommand),
         declaringType: typeof(ImageButtonView),
         propertyChanged: CommandPropertyChanged
         );

        public ICommand Command
        {
            get { return (ICommand)base.GetValue(CommandProperty); }
            set { base.SetValue(CommandProperty, value); }
        }

        private static void CommandPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (ImageButtonView)bindable;

            // this gesture recognizer will inovke the command event whereever it is used
            control.frmObject.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = (Command)newValue,
                CommandParameter = control.CommandParameter
            });

        }

        // for command parameter
        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
         propertyName: "CommandParameter",
         returnType: typeof(object),
         declaringType: typeof(ImageButtonView)
         );

        public object CommandParameter
        {
            get { return base.GetValue(CommandParameterProperty); }
            set { base.SetValue(CommandParameterProperty, value); }
        }
        #endregion

    }
}
