using MauiBlazorHybrid.Api.Data;
using MauiBlazorHybrid.Api.Data.Entitties;
using MauiBlazorHybrid.SharedLibrary.DTOs;
using Microsoft.EntityFrameworkCore;

namespace MauiBlazorHybrid.Api.Services
{
    public class CategoryService
    {
        private readonly AppDbContext dbContext;

        public CategoryService(AppDbContext _dbContext) 
        {
            dbContext = _dbContext;
        }
        public async Task<QuizApiResponse> CreateOrUpdateCategory(CategoryDto categoryDto)
        {
            if (!await IsMemberUnique(categoryDto)) return QuizApiResponse.Fail("Category's name already exists");
            if (categoryDto.Id == 0)
            {
                Category category  = new Category
                {                    
                    Name = categoryDto.Name,
                };
                dbContext.Categories.Add(category);
            }
            else
            {
                var category =  dbContext.Categories.FirstOrDefault(_=>_.Id == categoryDto.Id); 
                if(category == null)
                {
                    return QuizApiResponse.Fail("Category does not exists");

                }
                else
                {
                    category.Name = categoryDto.Name;
                    dbContext.Categories.Update(category);
                }
            }
            await dbContext.SaveChangesAsync();
            return QuizApiResponse.Success();
        }
        public async Task<CategoryDto?> GetCategory(int id)=> 
            await dbContext.Categories                
                .Where(category => category.Id == id)
                .Select(category => new CategoryDto
                {
                    Name = category.Name,
                    Id = category.Id
                })
                .FirstOrDefaultAsync();
        
        public async Task<CategoryDto[]> GetCategories()=> await dbContext.Categories.AsNoTracking()
            .Select(_=> new CategoryDto
            {
                Name = _.Name,
                Id = _.Id
            }).ToArrayAsync();
        public async Task<bool> IsMemberUnique(CategoryDto categoryDto)=> 
            await dbContext.Categories.AnyAsync(_ => _.Name == categoryDto.Name && _.Id != categoryDto.Id ) == false;
       
    }
}
