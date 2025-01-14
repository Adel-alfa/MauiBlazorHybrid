using MauiBlazorHybrid.SharedLibrary;

namespace MauiBlazorHybrid.web
{
    public class WebPlatform : IPlatform
    {
        public bool IsMobile => false;

        public bool IsWeb => true;
    }
}
