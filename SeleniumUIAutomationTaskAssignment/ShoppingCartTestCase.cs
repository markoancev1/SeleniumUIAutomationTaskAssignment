using NUnit.Framework;
using SeleniumUIAutomationTaskAssignment.Utilities;

namespace SeleniumUIAutomationTaskAssignment
{
    [TestFixture(BrowserType.Chrome)]
    class ShoppingCartTestCase : TestBase
    {
        //static ExtentTest test;
        Selectors selectors;

        public ShoppingCartTestCase(BrowserType browser) : base(browser) { }

        [SetUp]
        public void Initialize()
        {
            selectors = new(driver);
        }


        [Test]
        public void ShoppingCartTest()
        {
            WaitUtilities.WaitForVisibility(selectors.LocateBtnCookies, driver);

            selectors.btnCookieDisclaimer.Click();

            WaitUtilities.WaitForInvisibility(selectors.LocateBtnCookies, driver);

            WaitUtilities.WaitForInteractability(selectors.btnCategoryToggler, driver);
            selectors.btnCategoryToggler.Click();

            Assert.True(WaitUtilities.WaitForVisibilityOfAttribute(selectors.dropdownCategory, "class", "show", driver));

            string urlAllCategories = selectors.urlAllCategories;

            WaitUtilities.WaitForInteractability(selectors.btnSeeAllCategories, driver);
            selectors.btnSeeAllCategories.Click();

            Assert.True(WaitUtilities.WaitForUrlToBeDisplayed(urlAllCategories, driver));

            WebElementUtilities.SelectRandomProductFromRandomCategory(selectors.ListOfCategories, selectors.LocatorProduct, driver);

            var productsAdded = WebElementUtilities.PopulateShoppingCartInfo(selectors.ListOfProducts, driver);

            WaitUtilities.WaitForInteractability(selectors.btnShoppingCart, driver);
            selectors.btnShoppingCart.Click();

            WaitUtilities.WaitForVisibility(selectors.LocateShoppingCartDropdown, driver);

            Assert.True(WaitUtilities.WaitForVisibilityOfAttribute(selectors.btnShoppingCart, "aria-expanded", "true", driver));

            string urlViewShoppingCart = WebElementUtilities.GetUrlFromElement(selectors.btnViewShoppingCart);

            WaitUtilities.WaitForInteractability(selectors.btnViewShoppingCart, driver);
            selectors.btnViewShoppingCart.Click();

            Assert.True(WaitUtilities.WaitForUrlToBeDisplayed(urlViewShoppingCart, driver));

            Assert.True(WebElementUtilities.ProductsInShoppingCart(selectors.ListOfProductsInShoppingCart,productsAdded , driver));

            Assert.True(WebElementUtilities.CheckFullPrice(selectors.ListOfProductsInShoppingCart, driver));

            Assert.True(WebElementUtilities.ChangeQtyAndCheckFullPrice(selectors.ListOfProductsInShoppingCart, driver));
        }
    }
}
