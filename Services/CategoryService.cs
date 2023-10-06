﻿using Azure.Core;
using OdinShopping.Models;

namespace OdinShopping.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        public CategoryService(DataContext context)
        { 
            _context = context;
        }

        public async Task<List<Category>> GetCategory()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> AddCategory(string categoryName)
        {
            Category newCategory = new Category();
            newCategory.CategoryName = categoryName;

            _context.Categories.Add(newCategory);
            int result = await _context.SaveChangesAsync();
            if (result > 0)
                return newCategory;
            else
                return new Category();
        }

        public async Task<Category> UpdateCategory(string categoryName, int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category != null)
            {
                category.CategoryName = categoryName;
                int result = await _context.SaveChangesAsync();

                if (result > 0)
                    return category;
            }

            return new Category();
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);

            if (category != null)
                _context.Categories.Remove(category);
            else
                return false;

            int result = await _context.SaveChangesAsync();

            return result > 0 ? true : false;
        }
    }
}
