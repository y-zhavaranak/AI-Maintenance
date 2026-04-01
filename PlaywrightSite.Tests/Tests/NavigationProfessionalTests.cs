using PlaywrightSite.Tests.Core;
using PlaywrightSite.Tests.Pages;

namespace PlaywrightSite.Tests.Tests;

public class NavigationProfessionalTests : TestBase
{
    private const string RequirementId = "TC-NAV-001";
    private static readonly string[] RequiredNavigationLinks = ["Docs", "API", "Community"];

    [Test]
    public void TC_NAV_001_MainNavigation_ShouldExposeAccessibleAndActionableLinks()
    {
        // Given
        var page = new NavigationProfessionalPage(Driver)
            .Open();

        // Then
        TestContext.Progress.WriteLine($"[{RequirementId}] Verify main navigation landmark semantics.");
        Assert.That(() => page.IsMainNavigationAccessibleLandmark(),
            Is.True.After(5000, 200),
            "Main navigation should expose navigation role semantics and aria-label='Main'.");

        foreach (var linkName in RequiredNavigationLinks)
        {
            TestContext.Progress.WriteLine($"[{RequirementId}] Verify '{linkName}' is visible.");
            Assert.That(() => page.IsNavigationLinkVisible(linkName),
                Is.True.After(5000, 200),
                $"{linkName} should be visible in main navigation.");

            TestContext.Progress.WriteLine($"[{RequirementId}] Verify '{linkName}' is enabled.");
            Assert.That(() => page.IsNavigationLinkEnabled(linkName),
                Is.True.After(5000, 200),
                $"{linkName} should be enabled in main navigation.");

            TestContext.Progress.WriteLine($"[{RequirementId}] Verify '{linkName}' role/name accessibility semantics.");
            Assert.That(() => page.IsNavigationLinkAccessibleByRoleAndName(linkName),
                Is.True.After(5000, 200),
                $"{linkName} should expose link role semantics and accessible name.");
        }
    }

    [TestCase("Docs")]
    [TestCase("API")]
    [TestCase("Community")]
    public void TC_NAV_001_MainNavigation_Link_ShouldNavigateToExpectedHostAndPath(string linkName)
    {
        // Given
        var page = new NavigationProfessionalPage(Driver)
            .Open();

        // When
        TestContext.Progress.WriteLine($"[{RequirementId}] Click '{linkName}' from main navigation.");
        page.ClickNavigationLink(linkName);

        // Then
        TestContext.Progress.WriteLine($"[{RequirementId}] Verify destination host/path for '{linkName}'.");
        Assert.That(() => page.IsOnExpectedDestination(linkName),
            Is.True.After(8000, 200),
            $"{linkName} should navigate to playwright.dev with expected path prefix.");
    }

    [Test]
    public void TC_NAV_001_EdgeCase_DocsLink_ShouldNotNavigateToApiPath()
    {
        // Given
        var page = new NavigationProfessionalPage(Driver)
            .Open();

        TestContext.Progress.WriteLine($"[{RequirementId}] Verify Docs is visible and enabled before click.");
        Assert.That(() => page.IsNavigationLinkVisible("Docs") && page.IsNavigationLinkEnabled("Docs"),
            Is.True.After(5000, 200),
            "Docs link should be visible and enabled before navigation.");

        // When
        TestContext.Progress.WriteLine($"[{RequirementId}] Click 'Docs' from main navigation.");
        page.ClickNavigationLink("Docs");

        // Then
        TestContext.Progress.WriteLine($"[{RequirementId}] Verify Docs does not land on API path.");
        Assert.That(() => page.IsOnUnexpectedApiPathAfterDocsClick(),
            Is.True.After(8000, 200),
            "Docs click should navigate under /docs/ and must not open /docs/api/.");
    }
}