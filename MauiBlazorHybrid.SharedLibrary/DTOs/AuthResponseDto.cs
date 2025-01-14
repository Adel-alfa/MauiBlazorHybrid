﻿using MauiBlazorHybrid.SharedLibrary.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary.DTOs
{
    public record AuthResponseDto(LoggedInUser user, string? ErrorMessage = null)
    {
        [JsonIgnore]
        public bool HasError => ErrorMessage != null;
    }
}