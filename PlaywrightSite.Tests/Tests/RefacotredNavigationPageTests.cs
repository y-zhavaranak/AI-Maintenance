using PlaywrightSite.Tests.Core;
using PlaywrightSite.Tests.Pages;

namespace PlaywrightSite.Tests.Tests;

public class RefactoredNavigationPageTests : TestBase
{
    private const string RequirementId = "TC-NAV-001";
    private static readonly string[] RequiredNavigationLinks = ["Docs", "API", "Community"];

    [Test]
    public void TC_NAV_001_MainNavigation_ShouldDisplayAccessibleEnabledLinks_Docs_Api_Community()
    {
        // Given
        var page = new RefactoredNavigationPage(Driver)
            .Open();

        // Then
        TestContext.Progress.WriteLine($"[{RequirementId}] Step: Verify main navigation exposes landmark accessibility semantics.");
        Assert.That(() => page.IsMainNavigationAccessibleLandmark(),
            Is.True.After(5000, 200),
            "Main navigation should expose navigation role semantics and aria-label='Main'.");

        foreach (var linkName in RequiredNavigationLinks)
        {
            TestContext.Progress.WriteLine($"[{RequirementId}] Step: Verify '{linkName}' is visible in the Main navigation.");
            Assert.That(() => page.IsNavigationLinkVisible(linkName),
                Is.True.After(5000, 200),
                $"{linkName} link should be visible in Main navigation.");

            TestContext.Progress.WriteLine($"[{RequirementId}] Step: Verify '{linkName}' is enabled for interaction.");
            Assert.That(() => page.IsNavigationLinkEnabled(linkName),
                Is.True.After(5000, 200),
                $"{linkName} link should be enabled in Main navigation.");

            TestContext.Progress.WriteLine($"[{RequirementId}] Step: Verify '{linkName}' is accessible by role and name.");
            Assert.That(() => page.IsNavigationLinkAccessibleByRoleAndName(linkName),
                Is.True.After(5000, 200),
                $"{linkName} link should expose role='link' with accessible name '{linkName}'.");
        }
    }

    [TestCase("Docs")]
    [TestCase("API")]
    [TestCase("Community")]
    public void TC_NAV_001_MainNavigation_Link_ShouldNavigateToExpectedDestination(string linkName)
    {
        // Given
        var page = new RefactoredNavigationPage(Driver)
            .Open();

        // When
        TestContext.Progress.WriteLine($"[{RequirementId}] Step: Click '{linkName}' in Main navigation.");
        page.ClickNavigationLink(linkName);

        // Then
        TestContext.Progress.WriteLine($"[{RequirementId}] Step: Verify '{linkName}' opens expected destination host and path.");
        Assert.That(() => page.IsOnExpectedDestination(linkName),
            Is.True.After(8000, 200),
            $"{linkName} should navigate to the expected host and path prefix.");
    }

    [Test]
    public void TC_NAV_001_MainNavigation_ShouldRejectUnsupportedLinkName()
    {
        // Given
        var page = new RefactoredNavigationPage(Driver)
            .Open();

        // When / Then (edge case)
        TestContext.Progress.WriteLine($"[{RequirementId}] Step: Verify unsupported link names are rejected.");
        Assert.That(
            () => page.ClickNavigationLink("Pricing"),
            Throws.TypeOf<ArgumentException>().With.Property("ParamName").EqualTo("linkName"),
            "Unsupported navigation names should fail fast with ArgumentException for 'linkName'.");
    }
}