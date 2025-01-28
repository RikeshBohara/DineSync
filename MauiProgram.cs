using CommunityToolkit.Maui;
using DineSync.Data;
using DineSync.Repository.Implementations;
using DineSync.Repository.Interfaces;
using DineSync.ViewModels;
using DineSync.Views.Pages;
using Microsoft.Extensions.Logging;

namespace DineSync
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("Inter-Regular.ttf", "InterRegular");
                    fonts.AddFont("Inter-Semibold.ttf", "InterSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<DbConfig>();
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<SignupPage>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<MenuPage>();
            builder.Services.AddSingleton<OrderPage>();
            builder.Services.AddSingleton<TablePage>();
            builder.Services.AddSingleton<DashboardPage>();
            builder.Services.AddSingleton<EmployeePage>();
            builder.Services.AddTransient<MenuPage>();
            builder.Services.AddSingleton<LoginPageViewModel>();
            builder.Services.AddSingleton<SignupPageViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<TablePageViewModel>();
            builder.Services.AddTransient<MenuPageViewModel>();
            builder.Services.AddTransient<DashboardPageViewModel>();
            builder.Services.AddSingleton<App>();
            builder.Services.AddSingleton<AppShell>();

            builder.Services.AddSingleton<ISignupRepository, SignupRepository>();
            builder.Services.AddSingleton<ILoginRepository, LoginRepository>();
            builder.Services.AddSingleton<ITableRepository, TableRepository>();

            return builder.Build();
        }
    }
}
