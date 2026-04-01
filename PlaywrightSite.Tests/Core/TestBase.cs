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
        options.AddArgument("--start-maximized");

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