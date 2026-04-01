using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace PlaywrightSite.Tests.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    private readonly WebDriverWait _wait;

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        _wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    }

    protected IWebElement WaitForElementVisible(By locator)
    {
        return _wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return element.Displayed ? element : null;
        })!;
    }
}
