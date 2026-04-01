using OpenQA.Selenium;

namespace PlaywrightSite.Tests.Pages;

public class NavigationPage : BasePage
{
    private static readonly By DocsButton = By.XPath("//a[normalize-space()='Docs']");
    private static readonly By ApiButton = By.XPath("//a[normalize-space()='API']");
    private static readonly By CommunityButton = By.XPath("//a[normalize-space()='Community']");

    public NavigationPage(IWebDriver driver) : base(driver)
    {
    }

    public NavigationPage Open()
    {
        Driver.Navigate().GoToUrl("https://playwright.dev/");
        return this;
    }

    public bool IsDocsButtonDisplayed() => WaitForElementVisible(DocsButton).Displayed;

    public bool IsApiButtonDisplayed() => WaitForElementVisible(ApiButton).Displayed;

    public bool IsCommunityButtonDisplayed() => WaitForElementVisible(CommunityButton).Displayed;
}
