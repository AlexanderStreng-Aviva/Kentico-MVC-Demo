using System;

using CMS.DocumentEngine;
using CMS.SiteProvider;
using CMS.DocumentEngine.Types.DancingGoatMvc;
using CMS.Tests;

using Kentico.PageBuilder.Web.Mvc;

using DancingGoat.Controllers.Widgets;
using DancingGoat.Models.Widgets;

using Tests.DocumentEngine;

using NSubstitute;
using NUnit.Framework;
using TestStack.FluentMVCTesting;

namespace DancingGoat.Tests.Unit
{
    public class BannerWidgetControllerTests : UnitTests
    {
        private const string PARTIAL_VIEW_NAME = "Widgets/_BannerWidget";
        private const int SITE_ID = 1;
        private const string BANNER_TEXT = "Banner text";

        private Article page;
        private BannerWidgetController controller;
        private readonly IComponentPropertiesRetriever<BannerWidgetProperties> propertiesRetriever = Substitute.For<IComponentPropertiesRetriever<BannerWidgetProperties>>();
        private readonly ICurrentPageRetriever currentPageRetriever = Substitute.For<ICurrentPageRetriever>();
        private readonly Guid attachmentGuid = Guid.Parse("00000000-0000-0000-0000-000000000001");


        [SetUp]
        public void SetUp()
        {
            Fake().DocumentType<Article>(Article.CLASS_NAME);
            page = new Article
            {
                DocumentName = "Name"
            };
            currentPageRetriever.Retrieve(Arg.Any<IPageBuilderFeature>()).Returns(page);

            Fake<SiteInfo, SiteInfoProvider>().WithData(
                new SiteInfo
                {
                    SiteID = SITE_ID,
                    SiteName = "Site"
                });
            Fake<AttachmentInfo, AttachmentInfoProvider>().WithData(
                new AttachmentInfo
                {
                    AttachmentGUID = attachmentGuid,
                    AttachmentDocumentID = page.DocumentID,
                    AttachmentSiteID = SITE_ID
                });

            controller = new BannerWidgetController(propertiesRetriever, currentPageRetriever);
            controller.ControllerContext = ControllerContextMock.GetControllerContext(controller);
        }


        [Test]
        public void Index_RetrieveExistingDocumentAttachment_ReturnsCorrectModel()
        {
            propertiesRetriever
                .Retrieve()
                .Returns(new BannerWidgetProperties { ImageGuid = attachmentGuid, Text = BANNER_TEXT });

            controller.WithCallTo(c => c.Index())
                .ShouldRenderPartialView(PARTIAL_VIEW_NAME)
                .WithModel<BannerWidgetViewModel>(m => {
                    Assert.Multiple(() =>
                    {
                        Assert.That(m.Image.AttachmentGUID, Is.EqualTo(attachmentGuid));
                        Assert.That(m.Text, Is.EqualTo(BANNER_TEXT));
                    });
                });
        }


        [Test]
        public void Index_RetrieveNotExistingDocumentAttachment_ReturnsModelWithEmptyImage()
        {
            propertiesRetriever
                .Retrieve()
                .Returns(new BannerWidgetProperties { ImageGuid = Guid.Parse("00000000-0000-0000-0000-000000000002"), Text = BANNER_TEXT });

            Assert.Multiple(() =>
            {
                controller.WithCallTo(c => c.Index())
                    .ShouldRenderPartialView(PARTIAL_VIEW_NAME)
                    .WithModel<BannerWidgetViewModel>(m => {
                        Assert.Multiple(() =>
                        {
                            Assert.That(m.Image, Is.Null);
                            Assert.That(m.Text, Is.EqualTo(BANNER_TEXT));
                        });
                    });
            });
        }
    }
}
