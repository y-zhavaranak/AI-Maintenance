using OpenQA.Selenium;

namespace PlaywrightSite.Tests.Pages;

public class NavigationProfessionalPage : BasePage
{
    private static readonly By MainNavigation = By.CssSelector("nav[aria-label='Main']");

    private static readonly IReadOnlyDictionary<string, string> ExpectedPathPrefixByLinkName =
        new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Docs"] = "/docs/",
            ["API"] = "/docs/api/",
            ["Community"] = "/community/"
        };

    public NavigationProfessionalPage(IWebDriver driver) : base(driver)
    {
    }

    public NavigationProfessionalPage Open()
    {
        Driver.Navigate().GoToUrl("https://playwright.dev/");
        WaitForElementVisible(MainNavigation);
        return this;
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

    public bool IsNavigationLinkVisible(string linkName) => WaitForNavigationLink(linkName).Displayed;

    public bool IsNavigationLinkEnabled(string linkName) => WaitForNavigationLink(linkName).Enabled;

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
            var destination = new Uri(driver.Url);
            return string.Equals(destination.Host, "playwright.dev", StringComparison.OrdinalIgnoreCase)
                   && destination.AbsolutePath.StartsWith(expectedPathPrefix, StringComparison.OrdinalIgnoreCase);
        });
    }

    public bool IsOnUnexpectedApiPathAfterDocsClick()
    {
        return WaitForCondition(driver =>
        {
            var destination = new Uri(driver.Url);
            return string.Equals(destination.Host, "playwright.dev", StringComparison.OrdinalIgnoreCase)
                   && destination.AbsolutePath.StartsWith("/docs/", StringComparison.OrdinalIgnoreCase)
                   && !destination.AbsolutePath.StartsWith("/docs/api/", StringComparison.OrdinalIgnoreCase);
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