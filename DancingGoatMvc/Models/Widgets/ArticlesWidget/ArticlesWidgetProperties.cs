﻿using Kentico.PageBuilder.Web.Mvc;

namespace DancingGoat.Models.Widgets
{
    /// <summary>
    /// Properties for Articles widget.
    /// </summary>
    public sealed class ArticlesWidgetProperties : IWidgetProperties
    {
        /// <summary>
        /// Number of articles to show.
        /// </summary>
        public int Count { get; set; } = 5;
    }
}