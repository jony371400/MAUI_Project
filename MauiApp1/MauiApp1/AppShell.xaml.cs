namespace MauiApp1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // 註冊頁面
            Routing.RegisterRoute(nameof(BoxBindingPage), typeof(BoxBindingPage));
            Routing.RegisterRoute(nameof(DispatchFloorPage), typeof(DispatchFloorPage));
            Routing.RegisterRoute(nameof(SystemPage), typeof(SystemPage));
            Routing.RegisterRoute(nameof(ErrorPage), typeof(ErrorPage));
        }
    }
}
