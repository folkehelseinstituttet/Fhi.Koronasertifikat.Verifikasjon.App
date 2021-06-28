using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FHICORC.Views.Elements
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CustomConsentSwitch : ContentView
    {
        public CustomConsentSwitch()
        {
            InitializeComponent();
        }

        bool selection1Active = true;
        bool selection2Active = false;

        public event EventHandler SelectionChanged;

        bool selected = false;

        protected virtual void OnSelectionChanged(CustomConsentSwitchEventArgs e)
        {
            SelectionChanged?.Invoke(this, e);
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            
            if (propertyName == InitialStateProperty.PropertyName)
            {
                if (InitialState) UpdateSwitchState();
            }
        }

        private double valueX, valueY;
        private bool IsTurnX, IsTurnY;

        public void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var x = e.TotalX;
            var y = e.TotalY;

            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    Debug.WriteLine("Started");
                    break;
                case GestureStatus.Running:
                    Debug.WriteLine("Running");

                    if ((x >= 5 || x <= -5) && !IsTurnX && !IsTurnY)
                    {
                        IsTurnX = true;
                    }

                    if ((y >= 5 || y <= -5) && !IsTurnY && !IsTurnX)
                    {
                        IsTurnY = true;
                    }

                    if (IsTurnX && !IsTurnY)
                    {
                        if (x <= valueX)
                        {
                            if (selection2Active)
                            {
                                selected = !selected;
                                selection1Active = true;
                                selection2Active = false;
                                UpdateUI();
                            }
                        }

                        if (x >= valueX)
                        {
                            if (selection1Active)
                            {
                                selected = !selected;
                                selection1Active = false;
                                selection2Active = true;
                                UpdateUI();
                            }
                        }
                    }
                    OnSelectionChanged(new CustomConsentSwitchEventArgs(selected));


                    break;
                case GestureStatus.Completed:
                    Debug.WriteLine("Completed");

                    valueX = x;
                    valueY = y;

                    IsTurnX = false;
                    IsTurnY = false;

                    break;
                case GestureStatus.Canceled:
                    Debug.WriteLine("Canceled");
                    break;


            }
        }

        public void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            UpdateSwitchState();
        }

        private void UpdateSwitchState()
        {
            selected = !selected;
            if (selection2Active)
            {
                selection1Active = true;
                selection2Active = false;
            }
            if (selection1Active)
            {
                selection1Active = false;
                selection2Active = true;
            }
            UpdateUI();
            OnSelectionChanged(new CustomConsentSwitchEventArgs(selected));
        }

        private void UpdateUI()
        {
            if (selected)
            {
                switchThumb.TranslateTo(switchThumb.X + 26, 0, 120);
                switchTrack.BackgroundColor = Color.FromHex("#32345C");
                switchThumb.BackgroundColor = Color.FromHex("#F3F9FB");
                switchThumb.Margin = new Thickness(0,0,3,0);
            }
            else
            {
                switchThumb.TranslateTo(switchThumb.X, 0, 120);
                switchTrack.BackgroundColor = Color.FromHex("#F3F9FB");
                switchThumb.BackgroundColor = Color.FromHex("#32345C");
                switchThumb.Margin = new Thickness(3,0,0,0);
            }
        }
        
        public static readonly BindableProperty InitialStateProperty =
            BindableProperty.Create(nameof(InitialState), typeof(bool), typeof(Grid), false);

        public bool InitialState
        {
            get => (bool) GetValue(InitialStateProperty);
            set => SetValue(InitialStateProperty, value);
        }


        void OnSwitchTapped(Object sender, EventArgs e)
        {
            UpdateSwitchState();

        }

        public class CustomConsentSwitchEventArgs : EventArgs
        {
            public bool Selected { get; set; }

            public CustomConsentSwitchEventArgs(bool selected)
            {
                Selected = selected;
            }
        }
    }
}
