using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumUIAutomationTaskAssignment.Utilities
{
    public class WaitUtilities
    {
        public static void WaitForVisibility(By locate, IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            wait.PollingInterval = TimeSpan.FromMilliseconds(1);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locate));
        }

        public static void WaitForInvisibility(By locate, IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            wait.PollingInterval = TimeSpan.FromSeconds(1);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(locate));
        }

        public static void WaitForVisibilityByXpath(string path, IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            wait.PollingInterval = TimeSpan.FromSeconds(1);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath(path)));
        }

        public static void WaitForInteractability(IWebElement element, IWebDriver driver)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            wait.PollingInterval = TimeSpan.FromSeconds(1);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(element));
        }

        public static void WaitForVisibilityOfAllElements(By locate, IWebDriver driver)
        {

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));

            wait.PollingInterval = TimeSpan.FromSeconds(1);

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.VisibilityOfAllElementsLocatedBy(locate));
        }

        public static bool WaitForVisibilityOfAttribute(IWebElement element, string attributeName, string attributeValue, IWebDriver driver)
        {
            try
            {
                Func<IWebDriver, bool> testCondition = (x) => element.GetAttribute(attributeName).Contains(attributeValue);

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                wait.PollingInterval = TimeSpan.FromSeconds(1);

                wait.Until(testCondition);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool WaitForUrlToBeDisplayed(string url, IWebDriver driver)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.UrlContains(url));

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
