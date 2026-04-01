using PlaywrightSite.Tests.Core;
using PlaywrightSite.Tests.Pages;

namespace PlaywrightSite.Tests.Tests;

public class RefactoredNavigationPageTests : TestBase
{
    private static readonly string[] RequiredNavigationLinks = ["Docs", "API", "Community"];

    [Test]
    public void Refactored_MainPage_ShouldDisplayNavigationButtons_Docs_Api_Community()
    {
        var page = new RefactoredNavigationPage(Driver)
            .Open();

        TestContext.Progress.WriteLine("Step: Verify main navigation exposes landmark accessibility semantics.");
        Assert.That(() => page.IsMainNavigationAccessibleLandmark(),
            Is.True.After(5000, 200),
            "Main navigation should expose navigation role semantics and aria-label='Main'.");

        foreach (var linkName in RequiredNavigationLinks)
        {
            TestContext.Progress.WriteLine($"Step: Verify '{linkName}' is visible in the Main navigation.");
            Assert.That(() => page.IsNavigationLinkVisible(linkName),
                Is.True.After(5000, 200),
                $"{linkName} link should be visible in Main navigation.");

            TestContext.Progress.WriteLine($"Step: Verify '{linkName}' is accessible by role and name.");
            Assert.That(() => page.IsNavigationLinkAccessibleByRoleAndName(linkName),
                Is.True.After(5000, 200),
                $"{linkName} link should expose role='link' with accessible name '{linkName}'.");
        }
    }

    [TestCase("Docs")]
    [TestCase("API")]
    [TestCase("Community")]
    public void Refactored_MainPage_NavigationLinks_ShouldOpenExpectedDestination(string linkName)
    {
        var page = new RefactoredNavigationPage(Driver)
            .Open();

        TestContext.Progress.WriteLine($"Step: Click '{linkName}' in Main navigation.");
        page.ClickNavigationLink(linkName);

        TestContext.Progress.WriteLine($"Step: Verify '{linkName}' opens expected destination.");
        Assert.That(() => page.IsOnExpectedDestination(linkName),
            Is.True.After(8000, 200),
            $"{linkName} should navigate to its expected destination URL segment.");
    }
}