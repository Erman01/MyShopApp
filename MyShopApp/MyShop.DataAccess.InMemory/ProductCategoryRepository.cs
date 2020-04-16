﻿using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository
    {
        ObjectCache cache = MemoryCache.Default;
        List<ProductCategory>  productCategories;
        public ProductCategoryRepository()
        {
            productCategories = cache["productCategories"] as List<ProductCategory>;
            if (productCategories == null)
            {
                productCategories = new List<ProductCategory>();
            }
        }
        public void Commit()
        {
            cache["productCategories"] = productCategories;
        }
        public void Insert(ProductCategory category)
        {
            productCategories.Add(category);
        }
        public void Edit(ProductCategory productCategory)
        {
            ProductCategory categoryCategoryToEdit = productCategories.Find(x => x.Id == productCategory.Id);

            if (categoryCategoryToEdit != null)
            {
                categoryCategoryToEdit = productCategory;
            }
            else
            {
                throw new Exception("Product Category not found");
            }
        }
        public ProductCategory Find(string id)
        {
            ProductCategory productCategory = productCategories.Find(c => c.Id == id);
            if (productCategory != null)
            {
                return productCategory;
            }
            else
            {
                throw new Exception("Product Category not found");
            }
        }
        public IQueryable<ProductCategory> Collection()
        {
            return productCategories.AsQueryable();
        }
        public void Delete(string id)
        {
            ProductCategory productCategoryToDelete = productCategories.Find(c => c.Id == id);
            if (productCategoryToDelete!=null)
            {
                productCategories.Remove(productCategoryToDelete);
            }
            else
            {
                throw new Exception("Product Category not found");
            }
        }
    }
}
