using NUnit.Framework;
using OpenQA.Selenium;

namespace SeleniumUIAutomationTaskAssignment.Utilities
{
    public class WebElementUtilities
    {
        private static void ScrollTo(IWebDriver driver, int xPosition = 0, int yPosition = 0)
        {
            var format = String.Format("window.scrollTo({0}, {1})", xPosition, yPosition);
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            js.ExecuteScript(format);
        }

        public static IWebElement ScrollToViewLocate(By locate, IWebDriver driver)
        {
            WaitUtilities.WaitForVisibility(locate, driver);

            var element = driver.FindElement(locate);
            ScrollToViewElement(element, driver);

            WaitUtilities.WaitForVisibility(locate, driver);
            return element;
        }

        public static void ScrollToTheBottom(IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollTo(0, document.body.scrollHeight - 150)");
            Thread.Sleep(1500);
        }

        public static void ScrollToViewElement(IWebElement element, IWebDriver driver)
        { 
            if (element.Location.Y > 200)
            {
                ScrollTo(driver, 0, element.Location.Y - 500);
                Thread.Sleep(1000);
            }
        }

        public static bool PaginationPageTest(IList<IWebElement> pages, IWebElement element, string pageSignature, IWebDriver driver)
        {
            for(var i = 0; i<pages.Count; i++)
            {
                if (pages[i].FindElement(By.TagName("a")).Text.Contains($"{pageSignature}"))
                {
                    WebElementUtilities.ScrollToViewElement(element, driver);

                    string pageUrl = GetUrlFromElement(pages[i].FindElement(By.TagName("a")));
                    pages[i].Click();

                    return WaitUtilities.WaitForUrlToBeDisplayed(pageUrl, driver);
                }
            }
            return false;
        }

        public static bool PaginationFirstPageTest(IList<IWebElement> pages, IWebElement element, IWebDriver driver)
        {
            List<int> pageIndex = ListNumbericStrings(pages);
            if (!ContainsClass(HyperlinkInsideElement(pages[pageIndex[0]]), "c-pagination__link--active"))
            {
                WebElementUtilities.ScrollToViewElement(element, driver);

                string pageUrl = GetUrlFromElement(HyperlinkInsideElement(pages[pageIndex[0]]));
                pages[pageIndex[0]].Click();

                return WaitUtilities.WaitForUrlToBeDisplayed(pageUrl, driver);
            }
            return false;
        }

        public static bool PaginationLastPageTest(IList<IWebElement> pages, IWebElement element, IWebDriver driver)
        {
            List<int> pageIndex = ListNumbericStrings(pages);

            if (!ContainsClass(HyperlinkInsideElement(pages[pageIndex[pageIndex.Count - 1]]), "c-pagination__link--active"));
            {
                WebElementUtilities.ScrollToViewElement(element, driver);

                string pageUrl = GetUrlFromElement(HyperlinkInsideElement(pages[pageIndex[pageIndex.Count - 1]]));
                pages[pageIndex[pageIndex.Count - 1]].Click();

                return WaitUtilities.WaitForUrlToBeDisplayed(pageUrl, driver);
            }
            return false;
        }

        public static List<int> ListNumbericStrings(IList<IWebElement> pages)
        {
            List<int> pageIndex = new List<int>();
            for(var i=0; i<pages.Count; i++)
            {
                if(int.TryParse(GetTextFromElement(HyperlinkInsideElement(pages[i])), out int n)){
                    pageIndex.Add(i);
                }
            }
            return pageIndex;
        }

        public static bool SelectRandomProductFromRandomCategory(IList<IWebElement> categories, By product, IWebDriver driver)
        {
            Random rnd = new Random();

            IWebElement randomCategory = categories[rnd.Next(categories.Count - 1)];

            IList<IWebElement> products = randomCategory.FindElements(product);

            IWebElement randomProduct = products[rnd.Next(products.Count - 1)];
            IWebElement productHyperlink = HyperlinkInsideElement(randomProduct);

            string productUrl = GetUrlFromElement(productHyperlink);

            WaitUtilities.WaitForInteractability(randomProduct, driver);
            productHyperlink.Click();

            return WaitUtilities.WaitForUrlToBeDisplayed(productUrl, driver);
        }

        public static Dictionary<string, Product> PopulateShoppingCartInfo(IList<IWebElement> products, IWebDriver driver)
        {
            var random = new Random();
            Product productInfo = new Product();

            Dictionary<string, Product> shoppingCart = new Dictionary<string, Product>();
            Dictionary<string, Product> sortedShoppingCart = new Dictionary<string, Product>();


            for (var i = 0; i < 3; i++)
            {

                int index =  random.Next(products.Count - 1);
                var name = products[index].FindElement(By.ClassName("c-frontOfPack__heading")).Text;
                var price = double.Parse(products[index].FindElement(By.ClassName("c-frontOfPack__priceValue")).Text.Trim('$'));

                var buttonAddToCart = products[index].FindElement(By.ClassName("c-buttonAddToCartIconType"));

                if(index > 25)
                {
                    ScrollToTheBottom(driver);
                }
                WebElementUtilities.ScrollToViewElement(buttonAddToCart, driver);

                WaitUtilities.WaitForInteractability(buttonAddToCart, driver);
                buttonAddToCart.Click();

                WaitUtilities.WaitForVisibilityOfAttribute(buttonAddToCart,"class", "resultSuccess", driver);

                if (shoppingCart.ContainsKey(name))
                {
                    productInfo.price += price;
                    productInfo.qty += 1;
                    shoppingCart[name] = productInfo;;
                }
                else
                {
                    productInfo.price = price;
                    productInfo.qty = 1;
                    shoppingCart.Add(name, productInfo);
                }
            }

            foreach (var product in shoppingCart.OrderBy(x => x.Key))
            {
                sortedShoppingCart.Add(product.Key, product.Value);
            }

            return sortedShoppingCart;
        }

        public static bool ProductsInShoppingCart(IList<IWebElement> products, Dictionary<string, Product> productsAdded, IWebDriver driver)
        {
            Dictionary<string, Product> shoppingCart = new Dictionary<string, Product>();
            Product productInfo = new Product();

            for (var i = 0; i < products.Count; i++)
            {
                WaitUtilities.WaitForInteractability(products[i], driver);
                ScrollToViewElement(products[i], driver);

                var name = products[i].FindElement(By.CssSelector("div.ch-cartPageItem__header.d-none.d-md-block.mb-3 > h2 > a")).Text;
                var qty = int.Parse(products[i].FindElement(By.ClassName("c-productQty__input")).GetAttribute("value"));
                var price = double.Parse(products[i].FindElement(By.ClassName("ch-cartPageItem__price")).Text.Trim('$'));

                productInfo.price = price;
                productInfo.qty = qty;

                shoppingCart.Add(name, productInfo);
            }

            foreach (var product in productsAdded)
            {
                if (!shoppingCart.Keys.Contains(product.Key) && !shoppingCart.Values.Contains(product.Value))
                {

                    return false;
                }
            }
            return true;
        }

        public static bool CheckFullPrice(IList<IWebElement> products, IWebDriver driver)
        {
            decimal fullPrice = decimal.Parse(
                driver.FindElement(By.CssSelector("span.ch-sideBox__lineValue.ch-sideBox__lineValue--total.js-ch-sideBox__lineValue--total"))
                .Text.Trim('$'));

            decimal sum = 0;
            foreach(var product in products)
            {
                decimal productPrice = decimal.Parse(product.FindElement(By.ClassName("ch-cartPageItem__price")).Text.Trim('$'));
                sum += productPrice;
            }

            if (sum != fullPrice) return false;
            return true;
        }

        public static bool ChangeQtyAndCheckFullPrice(IList<IWebElement> products, IWebDriver driver)
        {
            List<double> prices = new List<double>();
            var random = new Random();
            double fullPrice = double.Parse(
               driver.FindElement(By.CssSelector("span.ch-sideBox__lineValue.ch-sideBox__lineValue--total.js-ch-sideBox__lineValue--total"))
               .Text.Trim('$'));
            double changedFullPrice = 0;

            foreach (var product in products)
            {
                double productPrice = double.Parse(product.FindElement(By.ClassName("ch-cartPageItem__price")).Text.Trim('$'));
                prices.Add(productPrice);
            }

            int index = random.Next(prices.Count - 1);

            ScrollToViewElement(products[index].FindElement(By.ClassName("c-productQty__btn--increase")), driver);
            products[index].FindElement(By.ClassName("c-productQty__btn--increase")).Click();
            
            if(!(double.Parse(products[index].FindElement(By.ClassName("ch-cartPageItem__price")).Text.Trim('$')) == prices[index] * 2))
            {
                return false;
            }
            else
            {
                prices[index] = (double.Parse(products[index].FindElement(By.ClassName("ch-cartPageItem__price")).Text.Trim('$')));
            }

            foreach(var price in prices)
            {
                changedFullPrice += price;
            }

            if(changedFullPrice > fullPrice && changedFullPrice != double.Parse(
               driver.FindElement(By.CssSelector("span.ch-sideBox__lineValue.ch-sideBox__lineValue--total.js-ch-sideBox__lineValue--total"))
               .Text.Trim('$')));

            return true;
        }

        public static string GetUrlFromElement(IWebElement element) => element.GetAttribute("href");
        public static string GetTextFromElement(IWebElement element) => element.Text;
        public static bool ContainsClass(IWebElement element, string className) => 
            element.GetAttribute("class").Contains(className);
        public static IWebElement HyperlinkInsideElement(IWebElement element) => element.FindElement(By.TagName("a"));
    }

    public class Product
    {
        public int qty { get; set; }
        public double price { get; set; }
    }
}
