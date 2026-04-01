using PlaywrightSite.Tests.Core;
using PlaywrightSite.Tests.Pages;

namespace PlaywrightSite.Tests.Tests;

public class NavigationPageTests : TestBase
{
    [Test]
    public void NavigationPage_ShouldDisplayNavigationButtons_Docs_Api_Community()
    {
        var navigationPage = new NavigationPage(Driver)
            .Open();

        Assert.That(navigationPage.IsDocsButtonDisplayed(), Is.True, "Docs button should be displayed.");
        Assert.That(navigationPage.IsApiButtonDisplayed(), Is.True, "API button should be displayed.");
        Assert.That(navigationPage.IsCommunityButtonDisplayed(), Is.True, "Community button should be displayed.");
    }
}
