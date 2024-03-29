﻿using System.Collections.Generic;
using System.Linq;

using CMS.Ecommerce;
using CMS.SiteProvider;

using DancingGoat.Infrastructure;

namespace DancingGoat.Repositories.Implementation
{
    /// <summary>
    /// Provides CRUD operations for product variants.
    /// </summary>
    public class KenticoVariantRepository : IVariantRepository
    {
        private int SiteID
        {
            get
            {
                return SiteContext.CurrentSiteID;
            }
        }


        /// <summary>
        /// Returns an enumerable collection of all variants of the given product.
        /// </summary>
        /// <param name="productId">SKU object identifier of the variant's parent product.</param>
        /// <returns>Collection of product variants. See <see cref="ProductVariant"/> for detailed information.</returns>
        public IEnumerable<ProductVariant> GetByProductId(int productId)
        {
            var variantSKUs = VariantHelper.GetVariants(productId).OnSite(SiteID).ToList();

            // Create variants with the options
            return variantSKUs.Select(sku => new ProductVariant(sku.SKUID));
        }


        /// <summary>
        /// Returns a variant with the specified identifier.
        /// </summary>
        /// <param name="variantId">Product variant's SKU object identifier.</param>
        /// <returns><see cref="ProductVariant"/> object representing a product variant with the specified identifier. Returns <c>null</c> if not found.</returns>
        public ProductVariant GetById(int variantId)
        {
            var sku = SKUInfoProvider.GetSKUInfo(variantId);

            if (sku == null || sku.SKUSiteID != SiteID)
            {
                return null;
            }

            return new ProductVariant(variantId);
        }


        /// <summary>
        /// Returns a collection of option categories used in a product's variants.
        /// </summary>
        /// <param name="productId">SKU identifier of the variant's parent product.</param>
        /// <returns>Collection of option categories used in a product's variants. See <see cref="OptionCategoryInfo"/> for detailed information.</returns>
        [CacheDependency("ecommerce.skuoptioncategory|all")]
        public IEnumerable<OptionCategoryInfo> GetVariantOptionCategories(int productId)
        {
            // Get a list of option categories
            return VariantHelper.GetProductVariantsCategories(productId).ToList();
        }


        /// <summary>
        /// Returns a variant for the given parent product which consists of the specified options.
        /// If multiple variants use the given subset of options, one of them is returned (based on setting of the database engine).
        /// </summary>
        /// <param name="productId">SKU identifier of the variant's parent product.</param>
        /// <param name="optionIds">Collection of the variant's product options.</param>
        /// <returns><see cref="ProductVariant"/> object representing a product variant assembled from the specified information. Returns <c>null</c> if such variant does not exist.</returns>
        public ProductVariant GetByProductIdAndOptions(int productId, IEnumerable<int> optionIds)
        {
            var variantSKU = VariantHelper.GetProductVariant(productId, new ProductAttributeSet(optionIds));

            if (variantSKU == null)
            {
                return null;
            }

            return new ProductVariant(variantSKU.SKUID);
        }
    }
}
