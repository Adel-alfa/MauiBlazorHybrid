﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public record QuziQuestionAnswerDto(int StudentQuizId, int QuestionId, int OptionId);
    
}