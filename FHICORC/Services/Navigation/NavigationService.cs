using FHICORC.Models;
using FHICORC.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHICORC.Enums;
using FHICORC.Views;
using Xamarin.Forms;
using System.Threading;
using FHICORC.Configuration;

namespace FHICORC.Services
{
    public class NavigationService : INavigationService
    {
        private SemaphoreSlim _semaphore = new SemaphoreSlim(1);

        public int CurrentTab { get; set; } = 1;
        public static Stack<Page> ModalPages = new Stack<Page>();
        public static Stack<NavigationPage> _navPagesWithStatusBar = new Stack<NavigationPage>();
        IStatusBarService _androidStatusBarService;

        public NavigationService(IStatusBarService statusBarService)
        {
            _androidStatusBarService = statusBarService;
        }

        public void OpenLandingPage()
        {
            ResetNavigationStack();

            NavigationPage newPage = new NavigationPage(new LandingPage());
            _navPagesWithStatusBar.Push(newPage);
            Application.Current.MainPage = newPage;
        }

        public async Task OpenAcceptTermsPage()
        {
            AcceptTermsPage newPage = new AcceptTermsPage();
            await PushPage(newPage, true, PageNavigationStyle.PushModallyFullscreen, null);
        }

        void ResetNavigationStack()
        {
            ModalPages = new Stack<Page>();
            _navPagesWithStatusBar = new Stack<NavigationPage>();
        }

        public async Task PushPage(Page page, bool animated = true, PageNavigationStyle style = PageNavigationStyle.PushInNavigation, object data = null)
        {
            await _semaphore.WaitAsync();

            try
            {
                switch (style)
                {
                    case PageNavigationStyle.PushInNavigation:

                        await PushPageInNavigation(page, animated, data);
                        break;

                    case PageNavigationStyle.PushModallySheetPageIOS:

                        page.SetNavigationStyleCardView();
                        if (Device.RuntimePlatform == Device.iOS)
                            await PushSheetPageModalOnIOS(page, animated, data);
                        else
                            await PushModalWithNavigation(page, animated, data);
                        break;

                    case PageNavigationStyle.PushModallyFullscreen:

                        page.SetNavigationStyleFullscreen();
                        await PushModalWithNavigation(page, animated, data);
                        break;
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        protected async Task PushPageInNavigation(Page page, bool animated = true, object data = null)
        {
            await page.Initialize(data);
            await FindCurrentPage().Navigation.PushAsync(page, animated);
        }

        public async Task PopPage(bool animated = true)
        {
            await _semaphore.WaitAsync();

            try
            {

                Page pageToPop = FindCurrentPage();
                if (pageToPop.IsModal())
                {
                    if (pageToPop.Navigation.NavigationStack.Any()) {//The modal has a navigation page
                        _navPagesWithStatusBar.Pop();
                    }
                    await pageToPop.Navigation.PopModalAsync(animated);
                    ModalPages.Pop();
                }
                else
                {
                    var poppedPage = await pageToPop.Navigation.PopAsync(animated);
                    if (poppedPage == null)
                    {
                        //There were no pages to close.
                        //Use default Android behaviour to close the app.
                        if (Device.RuntimePlatform == Device.Android)
                        {
                            IoCContainer.Resolve<ICloseButtonService>().ClickCloseButton();
                        }
                    }
                    else if (pageToPop.Navigation.NavigationStack.Count >= 1)
                    {
                        //A Nav page was popped
                        _navPagesWithStatusBar.Pop();
                    }
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task PopPage()
        {
            await PopPage(true);
        }

        public void OnDidDismissSheetPageOnIOS()
        {
            ModalPages.Pop();
        }

        private async Task PushModalWithNavigation(Page page, bool animated = true, object data = null)
        {
            await page.Initialize(data);

            NavigationPage newPage = new NavigationPage(page);
            await PushModal(newPage, animated);

            _navPagesWithStatusBar.Push(newPage);
            ModalPages.Push(page);
        }

        //Pushes a modal on iOS
        private async Task PushSheetPageModalOnIOS(Page page, bool animated = true, object data = null)
        {
            await page.Initialize(data);

            await PushModal(page, animated);

            ModalPages.Push(page);
        }

        private async Task PushModal(Page page, bool animated = true)
        {
            Page currentPage = FindCurrentPage();

            await currentPage.Navigation.PushModalAsync(page, animated);
        }

        public async Task<Page> FindCurrentPageAsync()
        {
            await _semaphore.WaitAsync();

            try
            {
                return FindCurrentPage();
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public Page FindCurrentPage()
        {
            //When a modal is pushed it has it's own navigation stack on top of another navigation stack.
            if (ModalPages.Any())
            {
                //If the last modal page, has a navigation stack, then return the top most item on the stack.
                Page modalRoot = ModalPages.Peek();
                if (modalRoot.Navigation.NavigationStack.Count > 1)
                {
                    return modalRoot.Navigation.NavigationStack.Last();
                }
                var modalNavPage = modalRoot as NavigationPage;
                if (modalNavPage != null)
                {
                    return modalNavPage.CurrentPage;
                }

                //Otherwise the modal it self must be the top.
                return modalRoot;
            }

            if (_navPagesWithStatusBar.Any())
            {
                return _navPagesWithStatusBar.Peek().CurrentPage;
            }

            //If there is no tabbar yet
            Page rootPage = Application.Current.MainPage;
            var navPage = rootPage as NavigationPage;
            if (navPage != null)
            {
                return navPage.CurrentPage;
            }
            return rootPage;
        }

        public async Task GoToErrorPage(ErrorPageModel data)
        {
            var lastModal = FindCurrentPage().Navigation.ModalStack.LastOrDefault();
            if (lastModal == null || lastModal.GetType() != typeof(BaseErrorPage))
            {
                await PushModalWithNavigation(new BaseErrorPage(data.Type), true, data);
            }
        }

        //Only use this method if there is no way to use PopPage instead.
        public void RemoveInstancesOfPageFromStackBelowTopPage(Type pageType)
        {
            IReadOnlyList<Page> currentNavigationStack = FindCurrentPage().Navigation?.NavigationStack;
            if (currentNavigationStack == null || !currentNavigationStack.Any())
            {
                return;
            }

            int numberOfInstances = currentNavigationStack.Count((it) => it.GetType() == pageType);
            bool isTopPage = currentNavigationStack.Last().GetType() == pageType;

            if (numberOfInstances > 1 || (numberOfInstances == 1 && !isTopPage))
            {
                var pageToPop = currentNavigationStack.First((it) => it.GetType() == pageType);
                pageToPop.Navigation.RemovePage(pageToPop);
            }
        }

        public void SetStatusBar(Color backgroundColor, Color textColor)
        {
            if (!_navPagesWithStatusBar.Any()) return;

            _navPagesWithStatusBar.Peek().BarBackgroundColor = backgroundColor;
            _navPagesWithStatusBar.Peek().BarTextColor = textColor;
            _androidStatusBarService.SetStatusBarColor(backgroundColor, textColor);
        }
    }
}