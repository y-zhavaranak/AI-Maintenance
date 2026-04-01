using OpenQA.Selenium;

namespace PlaywrightSite.Tests.Pages;

public class RefactoredNavigationPage : BasePage
{
    private static readonly By MainNavigation = By.CssSelector("nav[aria-label='Main']");

    private static readonly IReadOnlyDictionary<string, string> ExpectedPathPrefixByLinkName =
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

    public bool IsNavigationLinkEnabled(string linkName)
    {
        return WaitForNavigationLink(linkName).Enabled;
    }

    public bool IsMainNavigationAccessibleLandmark()
    {
        var nav = WaitForElementVisible(MainNavigation);
        var explicitRole = nav.GetDomAttribute("role");
        var hasNavigationRoleSemantics = string.IsNullOrWhiteSpace(explicitRole)
                                         || string.Equals(explicitRole, "navigation", StringComparison.OrdinalIgnoreCase);
        var hasExpectedAriaLabel = string.Equals(nav.GetDomAttribute("aria-label"), "Main", StringComparison.OrdinalIgnoreCase);

        return string.Equals(nav.TagName, "nav", StringComparison.OrdinalIgnoreCase)
               && hasNavigationRoleSemantics
               && hasExpectedAriaLabel;
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
        var expectedPathPrefix = GetExpectedPathPrefix(linkName);
        return WaitForCondition(driver =>
        {
            var destinationUrl = new Uri(driver.Url);
            var isExpectedHost = string.Equals(destinationUrl.Host, "playwright.dev", StringComparison.OrdinalIgnoreCase);
            var isExpectedPath = destinationUrl.AbsolutePath.StartsWith(expectedPathPrefix, StringComparison.OrdinalIgnoreCase);

            return isExpectedHost && isExpectedPath;
        });
    }

    private IWebElement WaitForNavigationLink(string linkName)
    {
        ValidateSupportedLinkName(linkName);
        return WaitForElementVisible(By.XPath($"//nav[@aria-label='Main']//a[normalize-space()='{linkName}']"));
    }

    private static string GetExpectedPathPrefix(string linkName)
    {
        if (!ExpectedPathPrefixByLinkName.TryGetValue(linkName, out var expectedPathPrefix))
        {
            throw new ArgumentException($"Unsupported navigation link: {linkName}", nameof(linkName));
        }

        return expectedPathPrefix;
    }

    private static void ValidateSupportedLinkName(string linkName)
    {
        if (!ExpectedPathPrefixByLinkName.ContainsKey(linkName))
        {
            throw new ArgumentException($"Unsupported navigation link: {linkName}", nameof(linkName));
        }
    }
}