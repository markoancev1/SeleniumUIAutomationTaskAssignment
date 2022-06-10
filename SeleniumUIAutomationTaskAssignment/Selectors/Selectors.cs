using OpenQA.Selenium;
using SeleniumUIAutomationTaskAssignment.Utilities;

namespace SeleniumUIAutomationTaskAssignment
{
    public class Selectors
    {
        public Selectors(IWebDriver webDriver) => driver = webDriver;

        private IWebDriver driver { get; }

        public By LocateBtnCookies => By.ClassName("c-cookiesDisclaimer__button");
        public By LocateCategoryToggler => By.ClassName("hd-desktopNavToggler");
        public By LocateSearchBox => By.Id("hd-search__input");
        public By LocatorProduct => By.CssSelector("li[data-qa='allCategoriesPageSubcategoryName']");
        public By LocateShoppingCartDropdown => By.ClassName("hd-cartDropdown__content");
        public By LocateFirstSearchResult => By.CssSelector(".js-hd-autocomplete__content > li:nth-child(1) > a:nth-child(1)");
        public IWebElement btnCookieDisclaimer => driver.FindElement(LocateBtnCookies);
        public IWebElement btnShoppingCart => driver.FindElement(By.Id("hd-cartDropdown__toggler"));
        public IWebElement btnViewShoppingCart => driver.FindElement(By.CssSelector("a[data-qa='headerShoppingCartPopUpShoppingCartPageLink']"));
        public IWebElement btnSeeAllCategories => driver.FindElement(By.XPath("//a[@href='/en-ca/categories']"));
        public IWebElement btnCategoryToggler => driver.FindElement(LocateCategoryToggler);
        public IWebElement SearchBox => driver.FindElement(LocateSearchBox);
        public IWebElement dropdownCategory => driver.FindElement(By.ClassName("hd-nav"));
        public IWebElement DivCopyright => driver.FindElement(By.ClassName("ft-copyright"));
        public IWebElement UlPagination => driver.FindElement(By.ClassName("c-pagination__list"));
        public IList<IWebElement> PagesDisplayed => UlPagination.FindElements(By.ClassName("c-pagination__item"));
        public IList<IWebElement> ListOfCategories => driver.FindElements(By.ClassName("cat-categories__item"));
        public IList<IWebElement> ListOfProducts => driver.FindElements(By.ClassName("c-frontOfPack"));
        public IList<IWebElement> ListOfProductsInShoppingCart => driver.FindElements(By.ClassName("ch-cartPageItem"));
        public IWebElement FirstSearchResult => driver.FindElement(LocateFirstSearchResult);
        public string urlFirstSearchResult => WebElementUtilities.GetUrlFromElement(FirstSearchResult);
        public string urlAllCategories => WebElementUtilities.GetUrlFromElement(btnSeeAllCategories);

        public List<string> pageNames = new List<string>()
        {
            "1",
            "2",
            "3",
            "4",
            "Continue",
            "Back",
            "..."
        };
    }
}
