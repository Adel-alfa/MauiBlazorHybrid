using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public record AdminDashboardDto(int TotalCategories, int TotalStudent, int ApprovedStudents, int TotalQuizzes,int ActiveQuizzes);
   
}
