using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleRestAPI.Data;

namespace SimpleRestAPI.Data
{
    public class CategoryDal : ICategory
    {
        private List<Category> _categories = new List<Category>();

        public CategoryDal()
        {
            _categories = new List<Category>
            {
                new Category { categoryId = 1, Categoryname = "ASP.NET Core" },
                new Category { categoryId = 2, Categoryname = "ASP.NET MVC" },
                new Category { categoryId = 3, Categoryname = "ASP.NET Web API" },
                new Category { categoryId = 4, Categoryname = "Blazor" },
                new Category { categoryId = 5, Categoryname = "Xamarin" },
                new Category { categoryId = 6, Categoryname = "Azure" }
            };
        }


        public Category AddCategory(Category category)
        {
            _categories.Add(category);
            return category;
        }

        public void DeleteCategory(int categoryId)
        {
            var category = GetCategoryById(categoryId);
            if(category != null)
            {
                _categories.Remove(category);
            }
        }

        public IEnumerable<Category> GetCategories()
        {
            return _categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            var category = _categories.FirstOrDefault(c => c.categoryId == categoryId);
            if(category == null)
            {
                throw new Exception("Category not found");
            }
            return category ;
        } 

        public Category UpdateCategory(Category category)
        {
            var existingCategory = GetCategoryById(category.categoryId);
            if (existingCategory != null)
            {
                existingCategory.Categoryname = category.Categoryname;
            }
            return existingCategory;
        }
    }
}