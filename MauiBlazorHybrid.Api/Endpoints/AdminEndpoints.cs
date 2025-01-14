using MauiBlazorHybrid.Api.Services;
using MauiBlazorHybrid.SharedLibrary.Utilities;

namespace MauiBlazorHybrid.Api.Endpoints
{
    public static  class AdminEndpoints
    {
        public static IEndpointRouteBuilder MapAdminEndpoints(this IEndpointRouteBuilder app)
        {
            var adminGroup = app.MapGroup("/api/admin")
                .RequireAuthorization(policy => policy.RequireRole("Admin"));

            adminGroup.MapGet("/home-data", async(AdminService adminService)=>
            Results.Ok(await adminService.GetHomeStatisticDataAsync()))
                .RequireAuthorization(policy => policy.RequireRole("Admin"));

            adminGroup.MapGet("/quizzes/{quizId:guid}/student",async(Guid quizId, int startIndex, int pageSize,bool fetchQuizInfo, AdminService adminService)=>
                Results.Ok(await adminService.GetQuizStudentsAsync(quizId, startIndex, pageSize,fetchQuizInfo)));

            var group = adminGroup.MapGroup("/students");

            group.MapGet("", async (UserApprovedFilter approvedType, int startIndex, int pageSize, AdminService adminService) =>
             Results.Ok(await adminService.GetUserAsync(approvedType, startIndex, pageSize)));

            group.MapPatch("{studentId:int}/toggle-status", async (int studentId, AdminService adminService) =>
            {
                await adminService.ToggleUserApprovedStatus(studentId);
                Results.Ok();
            });

         

            return app;
        }
    }
}
