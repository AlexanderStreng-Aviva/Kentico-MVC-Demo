﻿using System.Collections.Generic;

using DancingGoat.Models.Articles;

namespace DancingGoat.Models.Widgets
{
    /// <summary>
    /// View model for Articles widget.
    /// </summary>
    public sealed class ArticlesWidgetViewModel
    {
        /// <summary>
        /// Latest articles to display.
        /// </summary>
        public IEnumerable<ArticleViewModel> Articles { get; set; }


        /// <summary>
        /// Number of articles to show.
        /// </summary>
        public int Count { get; set; }
    }
}