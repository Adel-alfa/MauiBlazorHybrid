using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary
{
    public interface IAppState
    {
        string? LoadingText { get; }
        void ShowLoader(string loadingText);
        void HideLoader ();
        event Action? OnToggleLoader;
        void ShowError(string errorText);
        event Action<string>? OnShowError;
    }
}
