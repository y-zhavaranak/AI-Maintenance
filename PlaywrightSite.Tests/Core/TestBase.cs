using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace PlaywrightSite.Tests.Core;

public abstract class TestBase
{
    protected IWebDriver Driver = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArgument("--window-size=1920,1080");
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");

        Driver = new ChromeDriver(options);
        Driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
    }

    [TearDown]
    public void TearDown()
    {
        Driver.Quit();
        Driver.Dispose();
    }
}