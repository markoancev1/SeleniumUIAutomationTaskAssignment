using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

public enum BrowserType
{
    Firefox,
    Chrome
}

public class TestBase
{
    protected IWebDriver driver { get; set; }
    protected BrowserType _browserType;

    public TestBase(BrowserType browser)
    {
        _browserType = browser;
    }

    [OneTimeSetUp]
    public void InitializeTest()
    {
        ChooseDriverInstance(_browserType);

        driver.Manage().Window.Maximize();

        driver.Navigate().GoToUrl("https://www.dodax.ca/en-ca/");
    }
    public void ChooseDriverInstance(BrowserType browserType)
    {
        if (browserType == BrowserType.Chrome)
        {
            ChromeOptions chromeOptions = new();
            //chromeOptions.AddArguments("--headless");
            //chromeOptions.AddArguments("--disable-gpu");
            //chromeOptions.AddArguments("--window-size=1920,1080");
            driver = new ChromeDriver(chromeOptions);
        }

        else if (browserType == BrowserType.Firefox)
            driver = new FirefoxDriver();
    }

    [OneTimeTearDown]
    public void CleanUp()
    {
        driver.Close();
        driver.Quit();
    }
}