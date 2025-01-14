using MauiBlazorHybrid.Api.Services;
using MauiBlazorHybrid.SharedLibrary.DTOs;

namespace MauiBlazorHybrid.Api.Endpoints
{
    public static class CategoryEndpoints
    {
        public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
        {
            var CategoryGroup = app.MapGroup("/api/categories")
                .RequireAuthorization();

            CategoryGroup.MapGet("", async (CategoryService categoryService) =>          
            Results.Ok(await categoryService.GetCategories()));

            CategoryGroup.MapPost("", async (CategoryDto model,CategoryService categoryService) =>
           Results.Ok(await categoryService.CreateOrUpdateCategory( model)))
                .RequireAuthorization(policy=>policy.RequireRole("Admin"));

            CategoryGroup.MapGet("/detail", async (int Id,CategoryService categoryService) =>
           Results.Ok(await categoryService.GetCategory(Id)));
            return app;
        }
    }
}

