using DancingGoat.Models.PageTemplates.LandingPage;
using Kentico.PageBuilder.Web.Mvc.PageTemplates;

[assembly: RegisterPageTemplate("LandingPageTemplate","Default Landing page template",
    typeof(LandingPageProperties),
    "PageTemplates/_LandingPageTemplate",
    IconClass = "icon-l-rows-2")]

namespace DancingGoat.App_Start
{
    public class PageBuilderComponentRegister
    {
    }
}