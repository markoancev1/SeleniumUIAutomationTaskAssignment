using NUnit.Framework;
using OpenQA.Selenium;
using SeleniumUIAutomationTaskAssignment.Utilities;

namespace SeleniumUIAutomationTaskAssignment
{
    [TestFixture(BrowserType.Chrome)]
    [TestFixture(BrowserType.Firefox)]
    class PaginationTestCase : TestBase
    {
        Selectors selectors;

        public PaginationTestCase(BrowserType browser) : base(browser) { }

        [SetUp]
        public void Initialize()
        {
            selectors = new(driver);
        }


        [Test]
        public void PaginationTest()
        {
            WaitUtilities.WaitForVisibility(selectors.LocateBtnCookies, driver);

            selectors.btnCookieDisclaimer.Click();

            WaitUtilities.WaitForInvisibility(selectors.LocateBtnCookies, driver);

            WaitUtilities.WaitForVisibility(selectors.LocateSearchBox, driver);
            selectors.SearchBox.SendKeys("book");

            WaitUtilities.WaitForVisibility(selectors.LocateFirstSearchResult, driver);
            string searchUrl = selectors.urlFirstSearchResult;

            selectors.SearchBox.SendKeys(Keys.Enter);

            Assert.True(WaitUtilities.WaitForUrlToBeDisplayed(searchUrl, driver));

            foreach(var pageName in selectors.pageNames)
            {
                WebElementUtilities.ScrollToViewElement(selectors.UlPagination, driver);
                Assert.True(WebElementUtilities.PaginationPageTest(selectors.PagesDisplayed, selectors.UlPagination, pageName, driver));
            }

            WebElementUtilities.ScrollToViewElement(selectors.UlPagination, driver);
            Assert.True(WebElementUtilities.PaginationFirstPageTest(selectors.PagesDisplayed, selectors.UlPagination, driver));

            WebElementUtilities.ScrollToViewElement(selectors.UlPagination, driver);
            Assert.True(WebElementUtilities.PaginationLastPageTest(selectors.PagesDisplayed, selectors.UlPagination, driver));
        }
    }
}
