using CMS.Tests;

using Kentico.PageBuilder.Web.Mvc;

using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets;

using NSubstitute;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace DancingGoat.Tests.Unit
{
    class TextWidgetControllerTests : UnitTests
    {
        private const string PARTIAL_VIEW_NAME = "Widgets/_TextWidget";
        private const string TEXT = "Sample text";

        private readonly IWidgetPropertiesRetriever<TextWidgetProperties> propertiesRetriever = Substitute.For<IWidgetPropertiesRetriever<TextWidgetProperties>>();


        [Test]
        public void Index_ReturnsCorrectModel()
        {
            propertiesRetriever.Retrieve().Returns(new TextWidgetProperties { Text = TEXT });

            var controller = new TextWidgetController(propertiesRetriever, Substitute.For<ICurrentPageRetriever>());
            controller.ControllerContext = ControllerContextMock.GetControllerContext(controller);

            controller.WithCallTo(c => c.Index())
                .ShouldRenderPartialView(PARTIAL_VIEW_NAME)
                .WithModel<TextWidgetViewModel>(m => m.Text == TEXT);
        }
    }
}
