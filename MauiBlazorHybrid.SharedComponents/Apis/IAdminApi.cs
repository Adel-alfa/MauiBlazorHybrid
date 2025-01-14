using MauiBlazorHybrid.SharedLibrary.DTOs;
using MauiBlazorHybrid.SharedLibrary.Utilities;
using Refit;

namespace MauiBlazorHybrid.SharedComponents.Apis
{
    [Headers("Authorization: Bearer ")]
    public interface IAdminApi
    {
        [Get("/api/admin/students")]
        Task<PageResult<UserDto>> GetUsersAsync(UserApprovedFilter approvedType, int startIndex, int pageSize);
        [Patch("/api/admin/students/{studentId}/toggle-status")]
        Task ToggleUserApprovedStatus(int studentId);

        [Get("/api/admin/home-data")]
        Task<AdminDashboardDto> GetHomeStatisticDataAsync();

        [Get("/api/admin/quizzes/{quizId}/student")]
        Task<AdminQuizStudentListDto> GetQuizStudentsAsync(Guid quizId, int startIndex, int pageSize, bool fetchQuizInfo);
    }
}
