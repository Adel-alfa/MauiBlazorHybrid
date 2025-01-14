

using MauiBlazorHybrid.SharedLibrary.DTOs;
using Refit;

namespace MauiBlazorHybrid.SharedComponents.Apis
{
    [Headers("Authorization: Bearer")]
    public interface ICategoryApi
    {
        [Post("/api/categories")]
        Task<QuizApiResponse> SaveCategoryAsync(CategoryDto category);
        [Get("/api/categories")]
        Task<CategoryDto[]> GatCategoriesAsync();
        [Get("/api/categories/detail")]
        Task<CategoryDto> GatCategoryByIdsAsync(int id);
    }
}
