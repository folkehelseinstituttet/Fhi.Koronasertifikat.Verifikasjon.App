using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using FHICORC.Enums;
using FHICORC.Services;
using FHICORC.Services.Interfaces;
using FHICORC.Tests.TestMocks;
using FHICORC.Views;
using Xamarin.Forms;

namespace FHICORC.Tests.NavigationTests
{
    public class ExtendedNavigationService : NavigationService, INavigationService
    {
        public List<NavigationRecord> History = new List<NavigationRecord>();

        public ExtendedNavigationService() : base(new MockStatusBarService())
        {
        }

        public new void OpenLandingPage()
        {
            History.Add(new NavigationRecord { NavType = NavType.OpenLandingPage, PageType = typeof(LandingPage) });
            base.OpenLandingPage();
        }

        public new async Task PushPage(Page page, bool animated = true, PageNavigationStyle style = PageNavigationStyle.PushInNavigation, object data = null)
        {
            History.Add(new NavigationRecord { NavType = NavType.PushPage, PageType = page.GetType() });
            await base.PushPage(page, animated, style, data);
        }

        public new async Task PopPage(bool animated = true)
        {
            History.Add(new NavigationRecord { NavType = NavType.PopPage, PageType = FindCurrentPage().GetType() });
            await base.PopPage(animated);
        }
    }

    public enum NavType
    {
        PushPage,
        PushModal,
        PopPage,
        OpenTabbar,
        OpenLandingPage,
        CloseTabbar,
        GoToTab
    }

    public class NavigationRecord : IEquatable<NavigationRecord>
    {
        public NavType NavType { get; set; }
        public Type PageType { get; set; }

        public NavigationRecord() { }

        public NavigationRecord(NavType navType)
        {
            NavType = navType;
        }

        public NavigationRecord(NavType navType, Type pageType)
        {
            PageType = pageType;
            NavType = navType;
        }

        public bool Equals([AllowNull] NavigationRecord other)
        {
            return this.PageType == other.PageType
                && this.NavType == other.NavType;
        }

        public string TypeText
        {
            get
            {
                return PageType.Name;
            }
        }
    }
}
