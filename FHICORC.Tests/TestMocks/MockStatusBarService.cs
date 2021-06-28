using System;
using FHICORC.Services.Interfaces;
using Xamarin.Forms;

namespace FHICORC.Tests.TestMocks
{
    public class MockStatusBarService : IStatusBarService
    {
        public MockStatusBarService()
        {
        }

        public void RenderQrScannerStatusBarColor(bool isLandingPage)
        {
        }

        public void SetStatusBarColor(Color color)
        {
        }

        public void SetStatusBarColor(Color backgroundColor, Color textColor)
        {
        }
    }
}
