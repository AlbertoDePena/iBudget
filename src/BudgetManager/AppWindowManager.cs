using System.Windows;
using Caliburn.Micro;

namespace BudgetManager
{
    public class AppWindowManager : WindowManager
    {
        protected override Window EnsureWindow(object model, object view, bool isDialog)
        {
            var window = base.EnsureWindow(model, view, isDialog);

            window.SizeToContent = SizeToContent.Manual;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            window.MaxWidth = 940;
            window.MinWidth = 540;
            window.MinHeight = 540;

            return window;
        }
    }
}