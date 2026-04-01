using OpenQA.Selenium;

namespace PlaywrightSite.Tests.Pages;

public class RefactoredNavigationPage : BasePage
{
    private static readonly By MainNavigation = By.CssSelector("nav[aria-label='Main']");

    private static readonly IReadOnlyDictionary<string, string> ExpectedPathByLinkName =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Docs"] = "/docs/",
            ["API"] = "/docs/api/",
            ["Community"] = "/community/"
        };

    public RefactoredNavigationPage(IWebDriver driver) : base(driver)
    {
    }

    public RefactoredNavigationPage Open()
    {
        Driver.Navigate().GoToUrl("https://playwright.dev/");
        WaitForElementVisible(MainNavigation);
        return this;
    }

    public bool IsNavigationLinkVisible(string linkName)
    {
        return WaitForNavigationLink(linkName).Displayed;
    }

    public bool IsNavigationLinkAccessibleByRoleAndName(string linkName)
    {
        var link = WaitForNavigationLink(linkName);
         var explicitRole = link.GetDomAttribute("role");
         var hasLinkRoleSemantics = string.IsNullOrWhiteSpace(explicitRole)
                        || string.Equals(explicitRole, "link", StringComparison.OrdinalIgnoreCase);
         var hasExpectedName = string.Equals(link.Text.Trim(), linkName, StringComparison.OrdinalIgnoreCase)
                      || string.Equals(link.GetDomAttribute("aria-label"), linkName, StringComparison.OrdinalIgnoreCase);

         return string.Equals(link.TagName, "a", StringComparison.OrdinalIgnoreCase)
             && hasLinkRoleSemantics
             && hasExpectedName;
    }

    public void ClickNavigationLink(string linkName)
    {
        WaitForNavigationLink(linkName).Click();
    }

    public bool IsOnExpectedDestination(string linkName)
    {
        var expectedPath = GetExpectedPath(linkName);
        return WaitForCondition(driver =>
            driver.Url.Contains(expectedPath, StringComparison.OrdinalIgnoreCase));
    }

    private IWebElement WaitForNavigationLink(string linkName)
    {
        return WaitForElementVisible(By.XPath($"//nav[@aria-label='Main']//a[normalize-space()='{linkName}']"));
    }

    private static string GetExpectedPath(string linkName)
    {
        if (!ExpectedPathByLinkName.TryGetValue(linkName, out var expectedPath))
        {
            throw new ArgumentException($"Unsupported navigation link: {linkName}", nameof(linkName));
        }

        return expectedPath;
    }
}