using DineSync.Controls;
using DineSync.Data;
using DineSync.ViewModels;

namespace DineSync
{
    public partial class App : Application
    {
        private readonly DbConfig _DbConfig;
        public App(DbConfig dbConfig)
        {
            InitializeComponent();

            MainPage = new AppShell();

            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(BorderlessEntry), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

            Microsoft.Maui.Handlers.EditorHandler.Mapper.AppendToMapping(nameof(BorderlessEditor), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });
            Microsoft.Maui.Handlers.PickerHandler.Mapper.AppendToMapping(nameof(BorderlessPicker), (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
            });

            _DbConfig = dbConfig;
        }

        protected override async void OnStart()
        {
            base.OnStart();

            await _DbConfig.InitializeDatabaseAsync();
        }

    }
}
