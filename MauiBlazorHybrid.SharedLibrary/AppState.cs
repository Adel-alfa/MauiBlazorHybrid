using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorHybrid.SharedLibrary
{
    public class AppState : IAppState
    {
        public string? LoadingText { get; private set;}

        public event Action? OnToggleLoader;
        public event Action<string>? OnShowError;

        public void HideLoader()
        {
            LoadingText = null;
            OnToggleLoader?.Invoke();
        }

        public void ShowLoader(string loadingText)
        {
            LoadingText = loadingText ;
            OnToggleLoader?.Invoke();
        }
        public void ShowError(string errorText)
        {
            OnShowError?.Invoke(errorText);
        }
    }
}
