using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public record QuizApiResponse(bool IsSuccess, string ErrorMessage)
    {
        public static QuizApiResponse Success() => new(true, null);
        public static QuizApiResponse Fail(string errorMessage) => new(false, errorMessage);
    }
    public record QuizApiResponse<TData>(TData Data,bool IsSuccess, string ErrorMessage)
    {
        public static QuizApiResponse<TData> Success(TData Data) => new(Data,true, null);
        public static QuizApiResponse<TData> Fail(string errorMessage) => new(default,false, errorMessage);
    }
}
